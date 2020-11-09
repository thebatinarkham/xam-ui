using System;

namespace AppName.Core
{
	public class PlatformCulture
	{
		public string PlatformString
		{
			get;
			private set;
		}

		public string LanguageCode
		{
			get;
			private set;
		}

		public string LocaleCode
		{
			get;
			private set;
		}

		public PlatformCulture(string platformCultureString)
		{
			if (string.IsNullOrEmpty(platformCultureString))
			{
				throw new ArgumentException("Expected culture identifier", "platformCultureString");
			}
			PlatformString = platformCultureString.Replace("_", "-");
			if (PlatformString.IndexOf("-", StringComparison.Ordinal) > 0)
			{
				string[] array = PlatformString.Split(new char[1]
				{
					'-'
				});
				LanguageCode = array[0];
				LocaleCode = array[1];
			}
			else
			{
				LanguageCode = PlatformString;
				LocaleCode = "";
			}
		}

		public override string ToString()
		{
			return PlatformString;
		}
	}
}
