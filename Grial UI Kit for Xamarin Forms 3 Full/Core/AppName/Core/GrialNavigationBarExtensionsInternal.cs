using Xamarin.Forms;

namespace AppName.Core
{
	internal static class GrialNavigationBarExtensionsInternal
	{
		public static readonly BindableProperty NavigationBarHeightProperty = BindableProperty.CreateAttached("__NavigationBarHeight", typeof(double), typeof(GrialNavigationBarExtensionsInternal), -1.0);

		public static void SetNavigationBarHeight(NavigationPage page, double value)
		{
			page.SetValue(NavigationBarHeightProperty, value);
		}

		public static double GetNavigationBarHeight(NavigationPage page)
		{
			return (double)page.GetValue(NavigationBarHeightProperty);
		}
	}
}
