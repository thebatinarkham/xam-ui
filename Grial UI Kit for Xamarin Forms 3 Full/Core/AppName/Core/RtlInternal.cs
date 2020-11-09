using Xamarin.Forms;

namespace AppName.Core
{
	internal static class RtlInternal
	{
		public static readonly BindableProperty CurrentLayoutDirectionProperty = BindableProperty.CreateAttached("__CurrentLayoutDirection", typeof(LayoutDirection), typeof(RtlInternal), LayoutDirection.Ltr);

		public static readonly BindableProperty PendingMirrorInfoProperty = BindableProperty.CreateAttached("__PendingMirrorInfo", typeof(PendingMirrorInfo), typeof(RtlInternal), default(PendingMirrorInfo));

		public static LayoutDirection GetCurrentLayoutDirection(BindableObject bo)
		{
			return (LayoutDirection)bo.GetValue(CurrentLayoutDirectionProperty);
		}

		public static void SetCurrentLayoutDirection(BindableObject bo, LayoutDirection value)
		{
			bo.SetValue(CurrentLayoutDirectionProperty, value);
		}

		public static PendingMirrorInfo GetPendingMirrorInfo(BindableObject bo)
		{
			return (PendingMirrorInfo)bo.GetValue(PendingMirrorInfoProperty);
		}

		public static void SetPendingMirrorInfo(BindableObject bo, PendingMirrorInfo value)
		{
			bo.SetValue(PendingMirrorInfoProperty, value);
		}
	}
}
