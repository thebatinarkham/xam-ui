using System.Windows.Input;
using Xamarin.Forms;

namespace AppName.Core
{
	public abstract class VideoPlayerSkinBase : ContentView
	{
		internal static readonly BindablePropertyKey IsFullScreenPropertyKey = BindableProperty.CreateReadOnly("IsFullScreen", typeof(bool), typeof(VideoPlayerSkinBase), false);

		public static readonly BindableProperty IsFullScreenProperty = IsFullScreenPropertyKey.BindableProperty;

		public bool IsFullScreen
		{
			get
			{
				return (bool)GetValue(IsFullScreenProperty);
			}
			internal set
			{
				SetValue(IsFullScreenPropertyKey, value);
			}
		}

		protected VideoPlayer Owner
		{
			get;
		}

		public ICommand ToggleFullScreenCommand
		{
			get;
		}

		protected VideoPlayerSkinBase(VideoPlayer owner)
		{
			Owner = owner;
			ToggleFullScreenCommand = new Command(Owner.ToggleFullScreen);
		}
	}
}
