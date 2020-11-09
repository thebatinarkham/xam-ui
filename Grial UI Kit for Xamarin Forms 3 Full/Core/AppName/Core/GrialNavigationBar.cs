using Xamarin.Forms;

namespace AppName.Core
{
    public class GrialNavigationBar : Grid
	{
		public static readonly BindableProperty UseNavigationPageBarBackgroundGradientProperty = BindableProperty.Create("UseNavigationPageBarBackgroundGradient", typeof(bool), typeof(GrialNavigationBar), false);

		public static readonly BindableProperty UseNavigationPageBarBackgroundColorProperty = BindableProperty.Create("UseNavigationPageBarBackgroundColor", typeof(bool), typeof(GrialNavigationBar), false);

		public static readonly BindableProperty HeightRequestBeyondNativeBarProperty = BindableProperty.Create("HeightRequestBeyondNativeBar", typeof(double), typeof(GrialNavigationBar), -1.0);

		public bool UseNavigationPageBarBackgroundGradient
		{
			get
			{
				return (bool)GetValue(UseNavigationPageBarBackgroundGradientProperty);
			}
			set
			{
				SetValue(UseNavigationPageBarBackgroundGradientProperty, value);
			}
		}

		public bool UseNavigationPageBarBackgroundColor
		{
			get
			{
				return (bool)GetValue(UseNavigationPageBarBackgroundColorProperty);
			}
			set
			{
				SetValue(UseNavigationPageBarBackgroundColorProperty, value);
			}
		}

		public double HeightRequestBeyondNativeBar
		{
			get
			{
				return (double)GetValue(HeightRequestBeyondNativeBarProperty);
			}
			set
			{
				SetValue(HeightRequestBeyondNativeBarProperty, value);
			}
		}

		public GrialNavigationBar()
		{
			base.VerticalOptions = LayoutOptions.Start;
		}
	}
}
