using Xamarin.Forms;

namespace AppName.Core
{
	internal class MirrorLayout : MirrorViewBase<Layout<View>>
	{
		protected override void Mirror(Layout<View> target, LayoutDirection direction, bool childrenOnly)
		{
			if (target.Children == null)
			{
				WaitForLoad(target, direction, childrenOnly);
				return;
			}
			RtlInternal.SetCurrentLayoutDirection(target, direction);
			MirrorChildren(target.Children, direction);
		}
	}
}
