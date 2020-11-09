namespace AppName.Core
{
	public static class LayoutDirectionServiceExtensions
	{
		public static bool IsFakingDirection(this ILayoutDirectionService service)
		{
			return service.LayoutDirection != service.DeviceNativeDirection;
		}

		public static void SimulateLayoutDirectionSwap(this ILayoutDirectionService service)
		{
			service.SimulateLayoutDirectionChange((service.LayoutDirection == LayoutDirection.Ltr) ? LayoutDirection.Rtl : LayoutDirection.Ltr);
		}
	}
}
