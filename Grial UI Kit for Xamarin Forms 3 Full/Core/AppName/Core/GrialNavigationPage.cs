using Xamarin.Forms;

namespace AppName.Core
{
	public static class GrialNavigationPage
	{
		public static readonly BindableProperty AndroidTitleBaselineCorrectionProperty = BindableProperty.CreateAttached("AndroidTitleBaselineCorrection", typeof(int), typeof(GrialNavigationPage), 0);

		public static readonly BindableProperty TitleFontSizeProperty = BindableProperty.CreateAttached("TitleFontSize", typeof(double), typeof(GrialNavigationPage), -1.0);

		public static readonly BindableProperty TitleFontFamilyProperty = BindableProperty.CreateAttached("TitleFontFamily", typeof(string), typeof(GrialNavigationPage), null);

		public static readonly BindableProperty IsBarTransparentProperty = BindableProperty.CreateAttached("IsBarTransparent", typeof(bool), typeof(GrialNavigationPage), false);

		public static readonly BindableProperty BarBackgroundProperty = BindableProperty.CreateAttached("BarBackground", typeof(Gradient), typeof(GrialNavigationPage), null);

		public static readonly BindableProperty BarBackgroundHeightProperty = BindableProperty.CreateAttached("BarBackgroundHeight", typeof(double), typeof(GrialNavigationPage), -1.0);

		public static void SetAndroidTitleBaselineCorrection(NavigationPage page, int value)
		{
			page.SetValue(AndroidTitleBaselineCorrectionProperty, value);
		}

		public static int GetAndroidTitleBaselineCorrection(NavigationPage page)
		{
			return (int)page.GetValue(AndroidTitleBaselineCorrectionProperty);
		}

		public static void SetTitleFontSize(NavigationPage page, double value)
		{
			page.SetValue(TitleFontSizeProperty, value);
		}

		public static double GetTitleFontSize(NavigationPage page)
		{
			return (double)page.GetValue(TitleFontSizeProperty);
		}

		public static void SetTitleFontFamily(NavigationPage page, string value)
		{
			page.SetValue(TitleFontFamilyProperty, value);
		}

		public static string GetTitleFontFamily(NavigationPage page)
		{
			return (string)page.GetValue(TitleFontFamilyProperty);
		}

		public static void SetIsBarTransparent(NavigationPage page, bool value)
		{
			page.SetValue(IsBarTransparentProperty, value);
		}

		public static bool GetIsBarTransparent(NavigationPage page)
		{
			return (bool)page.GetValue(IsBarTransparentProperty);
		}

		public static void SetBarBackground(NavigationPage navigationPage, Gradient value)
		{
			navigationPage.SetValue(BarBackgroundProperty, value);
		}

		public static Gradient GetBarBackground(NavigationPage navigationPage)
		{
			return (Gradient)navigationPage.GetValue(BarBackgroundProperty);
		}

		public static void SetBarBackgroundHeight(NavigationPage navigationPage, double value)
		{
			navigationPage.SetValue(BarBackgroundHeightProperty, value);
		}

		public static double GetBarBackgroundHeight(NavigationPage navigationPage)
		{
			return (double)navigationPage.GetValue(BarBackgroundHeightProperty);
		}
	}
}
