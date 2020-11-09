using System.ComponentModel;
using Xamarin.Forms;

namespace AppName.Core
{
	internal class GrialNavigationBarRendererHelper
	{
		private readonly IGrialNavigationBarRenderer _renderer;

		private NavigationPage _navigationPage;

		private Color? _lastSetBackgroundColor;

		private Gradient _lastSetBackgroundGradient;

		private bool ApplyBackgroundColor => (_renderer.Element as GrialNavigationBar).UseNavigationPageBarBackgroundColor;

		private bool ApplyBackgroundGradient => (_renderer.Element as GrialNavigationBar).UseNavigationPageBarBackgroundGradient;

		private double HeightRequestBeyondNativeBar => (_renderer.Element as GrialNavigationBar).HeightRequestBeyondNativeBar;

		public GrialNavigationBarRendererHelper(IGrialNavigationBarRenderer renderer)
		{
			_renderer = renderer;
		}

		public void Reset()
		{
			if (_navigationPage != null)
			{
				_navigationPage.PropertyChanged -= NavigationPagePropertyChanged;
				_navigationPage = null;
			}
			if (_renderer.Element != null)
			{
				Effects.SetBackgroundGradient(_renderer.Element, null);
			}
		}

		public void Setup()
		{
			_navigationPage = FindNavigationPage();
			if (_navigationPage != null)
			{
				_navigationPage.PropertyChanged -= NavigationPagePropertyChanged;
				_navigationPage.PropertyChanged += NavigationPagePropertyChanged;
				Update();
			}
		}

		public void PropertyChanged(PropertyChangedEventArgs e)
		{
			if (_navigationPage != null)
			{
				if (e.PropertyName == GrialNavigationBar.UseNavigationPageBarBackgroundGradientProperty.PropertyName || e.PropertyName == GrialNavigationBar.UseNavigationPageBarBackgroundColorProperty.PropertyName)
				{
					UpdateBackground();
				}
				else if (e.PropertyName == GrialNavigationBar.HeightRequestBeyondNativeBarProperty.PropertyName)
				{
					Update();
				}
			}
		}

		private void NavigationPagePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == GrialNavigationPage.BarBackgroundHeightProperty.PropertyName || e.PropertyName == GrialNavigationPage.IsBarTransparentProperty.PropertyName || e.PropertyName == GrialNavigationBarExtensionsInternal.NavigationBarHeightProperty.PropertyName)
			{
				Update();
			}
			else if (e.PropertyName == GrialNavigationPage.BarBackgroundProperty.PropertyName || e.PropertyName == NavigationPage.BarBackgroundColorProperty.PropertyName)
			{
				UpdateBackground();
			}
		}

		private void Update()
		{
			UpdateBackground();
			double navigationBarHeight = GrialNavigationBarExtensionsInternal.GetNavigationBarHeight(_navigationPage);
			bool isBarTransparent = GrialNavigationPage.GetIsBarTransparent(_navigationPage);
			double barBackgroundHeight = GrialNavigationPage.GetBarBackgroundHeight(_navigationPage);
			double num = (isBarTransparent && barBackgroundHeight < 0.0) ? navigationBarHeight : barBackgroundHeight;
			double num2 = num;
			if (HeightRequestBeyondNativeBar >= 0.0)
			{
				(_renderer.Element as GrialNavigationBar).IsClippedToBounds = true;
				num2 = HeightRequestBeyondNativeBar + navigationBarHeight;
			}
			if (num > 0.0)
			{
				Gradient.SetBackgroundGradientForcedHeight(_renderer.Element, num);
				_renderer.Element.IsVisible = true;
				_renderer.Element.HeightRequest = num2;
				double num3 = isBarTransparent ? 0.0 : (0.0 - navigationBarHeight);
				_renderer.Element.Layout(new Rectangle(0.0, num3, _renderer.Element.Width, num2));
				_renderer.Element.Margin = new Thickness(0.0, num3, 0.0, 0.0);
			}
			else
			{
				_renderer.Element.IsVisible = false;
			}
		}

		private void UpdateBackground()
		{
			if (ApplyBackgroundColor)
			{
				_renderer.Element.BackgroundColor = _navigationPage.BarBackgroundColor;
				_lastSetBackgroundColor = _navigationPage.BarBackgroundColor;
			}
			else if (_lastSetBackgroundColor.HasValue)
			{
				if (_renderer.Element.BackgroundColor == _lastSetBackgroundColor.Value)
				{
					_renderer.Element.BackgroundColor = Color.Transparent;
				}
				_lastSetBackgroundColor = null;
			}
			if (ApplyBackgroundGradient)
			{
				Gradient barBackground = GrialNavigationPage.GetBarBackground(_navigationPage);
				Effects.SetBackgroundGradient(_renderer.Element, barBackground);
			}
			else if (_lastSetBackgroundGradient != null)
			{
				if (Effects.GetBackgroundGradient(_renderer.Element) == _lastSetBackgroundGradient)
				{
					Effects.SetBackgroundGradient(_renderer.Element, null);
				}
				_lastSetBackgroundGradient = null;
			}
		}

		private NavigationPage FindNavigationPage()
		{
			for (Element parent = _renderer.Element.Parent; parent != null; parent = parent.Parent)
			{
				NavigationPage navigationPage = parent as NavigationPage;
				if (navigationPage != null)
				{
					return navigationPage;
				}
			}
			return null;
		}
	}
}
