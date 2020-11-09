using Xamarin.Forms;

namespace AppName.Core
{
	public static class Rtl
	{
		public static readonly BindableProperty MirrorProperty = BindableProperty.CreateAttached("Mirror", typeof(bool), typeof(Rtl), false, BindingMode.OneWay, null, OnMirroredChanged);

		public static readonly BindableProperty MirrorBehaviorProperty = BindableProperty.CreateAttached("MirrorBehavior", typeof(MirrorBehavior), typeof(Rtl), MirrorBehavior.Normal, BindingMode.OneWay, null, OnMirrorBehaviorChanged);

		private static IMirrorService _MirrorService;

		private static IMirrorService MirrorService
		{
			get
			{
				if (_MirrorService == null)
				{
					_MirrorService = DependencyService.Get<IMirrorService>();
				}
				return _MirrorService;
			}
		}

		public static bool GetMirror(BindableObject bo)
		{
			return (bool)bo.GetValue(MirrorProperty);
		}

		public static void SetMirror(BindableObject bo, bool value)
		{
			bo.SetValue(MirrorProperty, value);
		}

		private static void OnMirroredChanged(BindableObject bindable, object oldValue, object newValue)
		{
			VisualElement visualElement = bindable as VisualElement;
			if (visualElement != null)
			{
				MirrorService.Mirror(visualElement, ((bool)newValue) ? LayoutDirection.Rtl : LayoutDirection.Ltr);
			}
		}

		public static MirrorBehavior GetMirrorBehavior(BindableObject bo)
		{
			return (MirrorBehavior)bo.GetValue(MirrorBehaviorProperty);
		}

		public static void SetMirrorBehavior(BindableObject bo, MirrorBehavior value)
		{
			bo.SetValue(MirrorBehaviorProperty, value);
		}

		private static void OnMirrorBehaviorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			VisualElement visualElement = bindable as VisualElement;
			if (visualElement != null)
			{
				if (visualElement.Resources == null)
				{
					visualElement.Resources = new ResourceDictionary();
				}
				if ((MirrorBehavior)newValue == MirrorBehavior.Skip)
				{
					visualElement.Resources["IsRtl"] = false;
				}
				else
				{
					visualElement.Resources.Remove("IsRtl");
				}
			}
		}
	}
}
