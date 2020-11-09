using Xamarin.Forms;

namespace AppName.Core
{
	internal class MirrorStackLayout : MirrorViewBase<StackLayout>
	{
		protected override void Mirror(StackLayout target, LayoutDirection direction, bool childrenOnly)
		{
			if (target.Children == null)
			{
				WaitForLoad(target, direction, childrenOnly);
				return;
			}
			RtlInternal.SetCurrentLayoutDirection(target, direction);
			MirrorChildren(target.Children, direction);
			if (!childrenOnly && target.Orientation != 0)
			{
				MirrorViewBase<StackLayout>.InvertList(target.Children);
			}
		}
	}
}
