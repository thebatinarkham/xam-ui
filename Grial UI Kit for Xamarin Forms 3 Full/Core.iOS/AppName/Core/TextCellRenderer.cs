using UIKit;
using Xamarin.Forms;

namespace AppName.Core
{
    public class TextCellRenderer : Xamarin.Forms.Platform.iOS.TextCellRenderer
	{
		public TextCellRenderer()
		{
		}

		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			UITableViewCell cell = base.GetCell(item, reusableCell, tv);
			ViewCellRenderer.UpdateCell(item, cell);
			return cell;
		}
	}
}
