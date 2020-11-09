using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;

namespace AppName.Core
{
	internal static class YouTubeUtilities
	{
		private const string YouTubeSource = "://youtube.com";

		private const string YouTubeFullSource = "://www.youtube.com";

		private const string ShortYouTubeSource = "://youtu.be/";

		public static async Task<string> GetYouTubeUrl(string url)
		{
			string text = null;
			if (url.IndexOf("://youtube.com", StringComparison.InvariantCultureIgnoreCase) > 0 || url.IndexOf("://www.youtube.com", StringComparison.InvariantCultureIgnoreCase) > 0)
			{
				text = HttpUtility.ParseQueryString(new Uri(url).Query)["v"];
			}
			else
			{
				int num = url.IndexOf("://youtu.be/", StringComparison.InvariantCultureIgnoreCase);
				if (num > 0)
				{
					text = url.Substring(num + "://youtu.be/".Length);
				}
			}
			if (text == null)
			{
				return url;
			}
			string requestUri = "https://www.youtube.com/get_video_info?video_id=" + text;
			using (HttpClient client = new HttpClient())
			{
				Dictionary<string, string> dictionary = ParseQueryString(await client.GetStringAsync(requestUri));
				if (dictionary.ContainsKey("status"))
				{
					string text2 = dictionary["status"];
					if (text2.Equals("ok"))
					{
						Dictionary<string, string> dictionary2 = WebUtility.HtmlDecode(dictionary["url_encoded_fmt_stream_map"]).Split(new char[1]
						{
							','
						}).Select(ParseQueryString)
							.OrderBy(delegate(Dictionary<string, string> s)
							{
								if (s.ContainsKey("type"))
								{
									string text3 = s["type"];
									if (text3.Contains("video/mp4"))
									{
										return 10;
									}
									if (text3.Contains("video/3gpp"))
									{
										return 20;
									}
									if (text3.Contains("video/x-flv"))
									{
										return 30;
									}
									if (text3.Contains("video/webm"))
									{
										return 40;
									}
									return int.MaxValue;
								}
								return int.MaxValue;
							})
							.ThenBy(delegate(Dictionary<string, string> s)
							{
								if (s.ContainsKey("quality"))
								{
									string value = s["quality"];
									if (Device.Idiom == TargetIdiom.Phone)
									{
										return Array.IndexOf(new string[3]
										{
											"medium",
											"high",
											"small"
										}, value);
									}
									return Array.IndexOf(new string[3]
									{
										"high",
										"medium",
										"small"
									}, value);
								}
								return (Device.Idiom != TargetIdiom.Phone) ? 1 : 0;
							})
							.FirstOrDefault((Dictionary<string, string> s) => s.ContainsKey("url"));
						if (dictionary2 != null)
						{
							return dictionary2["url"];
						}
						throw new Exception("The url attribute of the youtube video was not found.");
					}
					if (dictionary.ContainsKey("reason"))
					{
						throw new Exception(dictionary["reason"]);
					}
					throw new Exception(text2);
				}
				throw new Exception("The status attribute of the youtube video was not found.");
			}
		}

		private static Dictionary<string, string> ParseQueryString(string query)
		{
			return ParseQueryString(query, Encoding.UTF8);
		}

		private static Dictionary<string, string> ParseQueryString(string query, Encoding encoding)
		{
			if (query == null)
			{
				throw new ArgumentNullException("query");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (query.Length == 0 || (query.Length == 1 && query[0] == '?'))
			{
				return new Dictionary<string, string>();
			}
			if (query[0] == '?')
			{
				query = query.Substring(1);
			}
			Dictionary<string, string> result = new Dictionary<string, string>();
			ParseQueryString(query, encoding, result);
			return result;
		}

		private static void ParseQueryString(string query, Encoding encoding, Dictionary<string, string> result)
		{
			if (query.Length == 0)
			{
				return;
			}
			string text = WebUtility.HtmlDecode(query);
			int length = text.Length;
			int num = 0;
			bool flag = true;
			while (num <= length)
			{
				int num2 = -1;
				int num3 = -1;
				for (int i = num; i < length; i++)
				{
					if (num2 == -1 && text[i] == '=')
					{
						num2 = i + 1;
					}
					else if (text[i] == '&')
					{
						num3 = i;
						break;
					}
				}
				if (flag)
				{
					flag = false;
					if (text[num] == '?')
					{
						num++;
					}
				}
				string key;
				if (num2 == -1)
				{
					key = null;
					num2 = num;
				}
				else
				{
					key = WebUtility.UrlDecode(text.Substring(num, num2 - num - 1));
				}
				if (num3 < 0)
				{
					num = -1;
					num3 = text.Length;
				}
				else
				{
					num = num3 + 1;
				}
				string value = WebUtility.UrlDecode(text.Substring(num2, num3 - num2));
				result.Add(key, value);
				if (num == -1)
				{
					break;
				}
			}
		}

		internal static bool IsYouTubeSource(string url)
		{
			if (url.IndexOf("://youtube.com", StringComparison.InvariantCultureIgnoreCase) <= 0 && url.IndexOf("://www.youtube.com", StringComparison.InvariantCultureIgnoreCase) <= 0)
			{
				return url.IndexOf("://youtu.be/", StringComparison.InvariantCultureIgnoreCase) > 0;
			}
			return true;
		}
	}
}
