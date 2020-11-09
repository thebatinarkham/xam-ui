using Xamarin.Forms;

namespace AppName.Core
{
	internal interface IMirror
	{
		bool TryHandleMirror(VisualElement target, LayoutDirection direction, bool childrenOnly);

		void SetCoordinator(IMirrorService coordinator);
	}
}
