using Xamarin.Forms;

namespace AppName.Core
{
	internal class MirrorContentView : MirrorViewBase<ContentView>
	{
		protected override void Mirror(ContentView target, LayoutDirection direction, bool childrenOnly)
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
