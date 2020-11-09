using UIKit;
using Xamarin.Forms;

namespace AppName.Core
{
    public class ImageCellRenderer : Xamarin.Forms.Platform.iOS.ImageCellRenderer
	{
		public ImageCellRenderer()
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
