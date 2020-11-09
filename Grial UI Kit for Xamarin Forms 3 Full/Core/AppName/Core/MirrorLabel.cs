using System.Collections.Generic;
using Xamarin.Forms;

namespace AppName.Core
{
	internal class MirrorLabel : MirrorViewBase<Label>
	{
		protected override void Mirror(Label target, LayoutDirection direction, bool childrenOnly)
		{
			if (target.FormattedText != null)
			{
				RtlInternal.SetCurrentLayoutDirection(target, direction);
				IList<Span> spans = target.FormattedText.Spans;
				if (spans != null && spans.Count > 1)
				{
					MirrorViewBase<Label>.InvertList(spans);
				}
			}
		}
	}
}
