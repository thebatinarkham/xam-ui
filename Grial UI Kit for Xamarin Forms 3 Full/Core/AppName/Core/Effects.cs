using System.Linq;
using Xamarin.Forms;

namespace AppName.Core
{
    public static class Effects
	{
		private class ApplyIOSSafeAreaAsPaddingEffect : RoutingEffect
		{
			public ApplyIOSSafeAreaAsPaddingEffect()
				: base("AppName.Core.ApplyIOSSafeAreaAsPaddingEffect")
			{
			}
		}

		private class IgnoreIOSSafeAreaOnScrollViewEffect : RoutingEffect
		{
			public IgnoreIOSSafeAreaOnScrollViewEffect()
				: base("AppName.Core.IgnoreIOSSafeAreaOnScrollViewEffect")
			{
			}
		}

		private class BackgroundGradientEffect : RoutingEffect
		{
			public BackgroundGradientEffect()
				: base("AppName.Core.BackgroundGradientEffect")
			{
			}
		}

		private class CornerRadiusEffect : RoutingEffect
		{
			public CornerRadiusEffect()
				: base("AppName.Core.CornerRadiusEffect")
			{
			}
		}

		private class ShadowEffect : RoutingEffect
		{
			public ShadowEffect()
				: base("AppName.Core.ShadowEffect")
			{
			}
		}

		public static readonly BindableProperty ApplyIOSSafeAreaAsPaddingProperty = BindableProperty.CreateAttached("ApplyIOSSafeAreaAsPadding", typeof(IOSSafeArea), typeof(Effects), IOSSafeArea.None, BindingMode.OneWay, null, OnChanged<ApplyIOSSafeAreaAsPaddingEffect, IOSSafeArea>);

		public static readonly BindableProperty IOSSafeAreaBottomSizeProperty = BindableProperty.CreateAttached("IOSSafeAreaBottomSize", typeof(double?), typeof(Effects), null);

		public static readonly BindableProperty IOSSafeAreaTopSizeProperty = BindableProperty.CreateAttached("IOSSafeAreaTopSize", typeof(double?), typeof(Effects), null);

		public static readonly BindableProperty IOSSafeAreaLeftSizeProperty = BindableProperty.CreateAttached("IOSSafeAreaLeftSize", typeof(double?), typeof(Effects), null);

		public static readonly BindableProperty IOSSafeAreaRightSizeProperty = BindableProperty.CreateAttached("IOSSafeAreaRightSize", typeof(double?), typeof(Effects), null);

		public static readonly BindableProperty IgnoreIOSSafeAreaOnScrollViewProperty = BindableProperty.CreateAttached("IgnoreIOSSafeAreaOnScrollView", typeof(IOSSafeArea), typeof(Effects), IOSSafeArea.None, BindingMode.OneWay, null, OnChanged<IgnoreIOSSafeAreaOnScrollViewEffect, IOSSafeArea>);

		public static readonly BindableProperty BackgroundGradientProperty = BindableProperty.CreateAttached("BackgroundGradient", typeof(Gradient), typeof(Effects), null, BindingMode.OneWay, null, OnChanged<BackgroundGradientEffect, Gradient>);

		public static readonly BindableProperty CornerRadiusProperty = BindableProperty.CreateAttached("CornerRadius", typeof(double), typeof(Effects), 0.0, BindingMode.OneWay, null, OnChanged<CornerRadiusEffect, double>);

		public static readonly BindableProperty ShadowProperty = BindableProperty.CreateAttached("Shadow", typeof(bool), typeof(Effects), false, BindingMode.OneWay, null, OnChanged<ShadowEffect, bool>);

		public static readonly BindableProperty ShadowIOSColorProperty = BindableProperty.CreateAttached("ShadowIOSColor", typeof(Color), typeof(Effects), Color.Gray);

		public static readonly BindableProperty ShadowSizeProperty = BindableProperty.CreateAttached("ShadowSize", typeof(float), typeof(Effects), 5f);

		public static readonly BindableProperty ShadowOpacityProperty = BindableProperty.CreateAttached("ShadowOpacity", typeof(float), typeof(Effects), 1f);

		public static void SetApplyIOSSafeAreaAsPadding(BindableObject view, IOSSafeArea value)
		{
			view.SetValue(ApplyIOSSafeAreaAsPaddingProperty, value);
		}

		public static IOSSafeArea GetApplyIOSSafeAreaAsPadding(BindableObject view)
		{
			return (IOSSafeArea)view.GetValue(ApplyIOSSafeAreaAsPaddingProperty);
		}

		public static void SetIOSSafeAreaBottomSize(BindableObject view, double? value)
		{
			view.SetValue(IOSSafeAreaBottomSizeProperty, value);
		}

		public static double? GetIOSSafeAreaBottomSize(BindableObject view)
		{
			return (double?)view.GetValue(IOSSafeAreaBottomSizeProperty);
		}

		public static void SetIOSSafeAreaTopSize(BindableObject view, double? value)
		{
			view.SetValue(IOSSafeAreaTopSizeProperty, value);
		}

		public static double? GetIOSSafeAreaTopSize(BindableObject view)
		{
			return (double?)view.GetValue(IOSSafeAreaTopSizeProperty);
		}

		public static void SetIOSSafeAreaLeftSize(BindableObject view, double? value)
		{
			view.SetValue(IOSSafeAreaLeftSizeProperty, value);
		}

		public static double? GetIOSSafeAreaLeftSize(BindableObject view)
		{
			return (double?)view.GetValue(IOSSafeAreaLeftSizeProperty);
		}

		public static void SetIOSSafeAreaRightSize(BindableObject view, double? value)
		{
			view.SetValue(IOSSafeAreaRightSizeProperty, value);
		}

		public static double? GetIOSSafeAreaRightSize(BindableObject view)
		{
			return (double?)view.GetValue(IOSSafeAreaRightSizeProperty);
		}

		public static void SetIgnoreIOSSafeAreaOnScrollView(BindableObject view, IOSSafeArea value)
		{
			view.SetValue(IgnoreIOSSafeAreaOnScrollViewProperty, value);
		}

		public static IOSSafeArea GetIgnoreIOSSafeAreaOnScrollView(BindableObject view)
		{
			return (IOSSafeArea)view.GetValue(IgnoreIOSSafeAreaOnScrollViewProperty);
		}

		public static void SetBackgroundGradient(BindableObject view, Gradient gradient)
		{
			view.SetValue(BackgroundGradientProperty, gradient);
		}

		public static Gradient GetBackgroundGradient(BindableObject view)
		{
			return (Gradient)view.GetValue(BackgroundGradientProperty);
		}

		public static void SetCornerRadius(BindableObject view, double radius)
		{
			view.SetValue(CornerRadiusProperty, radius);
		}

		public static double GetCornerRadius(BindableObject view)
		{
			return (double)view.GetValue(CornerRadiusProperty);
		}

		public static void SetShadow(BindableObject view, bool shadow)
		{
			view.SetValue(ShadowProperty, shadow);
		}

		public static bool GetShadow(BindableObject view)
		{
			return (bool)view.GetValue(ShadowProperty);
		}

		public static void SetShadowIOSColor(BindableObject view, Color color)
		{
			view.SetValue(ShadowIOSColorProperty, color);
		}

		public static Color GetShadowIOSColor(BindableObject view)
		{
			return (Color)view.GetValue(ShadowIOSColorProperty);
		}

		public static void SetShadowSize(BindableObject view, float offset)
		{
			view.SetValue(ShadowSizeProperty, offset);
		}

		public static float GetShadowSize(BindableObject view)
		{
			return (float)view.GetValue(ShadowSizeProperty);
		}

		public static void SetShadowOpacity(BindableObject view, float offset)
		{
			view.SetValue(ShadowOpacityProperty, offset);
		}

		public static float GetShadowOpacity(BindableObject view)
		{
			return (float)view.GetValue(ShadowOpacityProperty);
		}

		private static void OnChanged<TEffect, TProp>(BindableObject bindable, object oldValue, object newValue) where TEffect : Effect, new()
		{
			VisualElement visualElement = bindable as VisualElement;
			if (visualElement != null)
			{
				Effect effect = visualElement.Effects.FirstOrDefault((Effect e) => e is TEffect);
				if (effect != null)
				{
					visualElement.Effects.Remove(effect);
				}
				if (!object.Equals(newValue, default(TProp)))
				{
					visualElement.Effects.Add(new TEffect());
				}
			}
		}
	}
}
