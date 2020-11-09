using Xamarin.Forms;

namespace AppName.Core
{
	internal class MirrorContentPage : MirrorViewBase<ContentPage>
	{
		protected override void Mirror(ContentPage target, LayoutDirection direction, bool childrenOnly)
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
