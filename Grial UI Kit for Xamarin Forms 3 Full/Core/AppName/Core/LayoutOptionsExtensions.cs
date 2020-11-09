using Xamarin.Forms;

namespace AppName.Core
{
	internal static class LayoutOptionsExtensions
	{
		public static LayoutOptions ToLayoutOptions(this DataGridVerticalAlignment verticalAlignment)
		{
			switch (verticalAlignment)
			{
			case DataGridVerticalAlignment.Start:
				return LayoutOptions.StartAndExpand;
			case DataGridVerticalAlignment.Center:
				return LayoutOptions.CenterAndExpand;
			default:
				return LayoutOptions.EndAndExpand;
			}
		}

		public static LayoutOptions ToLayoutOptions(this TextAlignment textAlignment)
		{
			switch (textAlignment)
			{
			case TextAlignment.Start:
				return LayoutOptions.StartAndExpand;
			case TextAlignment.Center:
				return LayoutOptions.CenterAndExpand;
			default:
				return LayoutOptions.EndAndExpand;
			}
		}
	}
}
