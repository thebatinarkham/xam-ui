using Xamarin.Forms;

namespace AppName.Core
{
	public interface IMirrorService
	{
		void Mirror(VisualElement target, LayoutDirection direction);
	}
}
