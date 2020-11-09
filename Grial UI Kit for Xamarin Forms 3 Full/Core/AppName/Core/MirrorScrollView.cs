using Xamarin.Forms;

namespace AppName.Core
{
	internal class MirrorScrollView : MirrorViewBase<ScrollView>
	{
		protected override void Mirror(ScrollView target, LayoutDirection direction, bool childrenOnly)
		{
			if (target.Content == null)
			{
				WaitForLoad(target, direction, childrenOnly);
				return;
			}
			RtlInternal.SetCurrentLayoutDirection(target, direction);
			MirrorChild(target.Content, direction);
		}
	}
}
