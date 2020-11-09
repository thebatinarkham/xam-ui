using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AppName.Core
{
    public class ViewCellRenderer : Xamarin.Forms.Platform.iOS.ViewCellRenderer
    {
        public ViewCellRenderer()
        {
        }

        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            UITableViewCell cell = base.GetCell(item, reusableCell, tv);
            UpdateCell(item, cell);
            return cell;
        }

        public static void UpdateCell(Cell cell, UITableViewCell uiCell)
        {
            ListView listView = cell?.Parent as ListView;
            if (listView != null && listView.SelectionMode == ListViewSelectionMode.None)
            {
                uiCell.SelectionStyle = UITableViewCellSelectionStyle.None;
            }
            else
            {
                uiCell.SelectedBackgroundView = new UIView
                {
                    BackgroundColor = ColorCache.ListViewSelectedItemBackgroundColor.ToUIColor()
                };
            }
        }
    }
}
