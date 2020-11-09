using System.Windows.Input;
using Xamarin.Forms;

namespace AppName.Core
{
	public class VideoPlayerErrorSkin : VideoPlayerSkinBase
	{
		internal static readonly BindablePropertyKey MessagePropertyKey = BindableProperty.CreateReadOnly("Message", typeof(string), typeof(VideoPlayerErrorSkin), null);

		public static readonly BindableProperty MessageProperty = MessagePropertyKey.BindableProperty;

		public string Message
		{
			get
			{
				return (string)GetValue(MessageProperty);
			}
			internal set
			{
				SetValue(MessagePropertyKey, value);
			}
		}

		public ICommand RetryCommand
		{
			get;
		}

		public VideoPlayerErrorSkin(VideoPlayer owner)
			: base(owner)
		{
			RetryCommand = new Command(base.Owner.TryPlayAfterErrorFromSkin);
		}
	}
}
