using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AppName.Core
{
    public class TableRenderer : TableViewRenderer
    {
        private class CustomTableModelRenderer : TableViewModelRenderer
        {
            public CustomTableModelRenderer(TableView model)
                : base(model)
            {
            }

            public override void WillDisplayHeaderView(UITableView tableView, UIView headerView, nint section)
            {
                TableView view = View;
                if (view == null)
                {
                    return;
                }
                Color headerFooterTextColor = TableViewProperties.GetHeaderFooterTextColor(view);
                if (headerFooterTextColor != Color.Default)
                {
                    UITableViewHeaderFooterView uITableViewHeaderFooterView = headerView as UITableViewHeaderFooterView;
                    if (uITableViewHeaderFooterView != null)
                    {
                        uITableViewHeaderFooterView.TextLabel.TextColor = headerFooterTextColor.ToUIColor();
                    }
                }
            }
        }

        public TableRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TableView> e)
        {
            base.OnElementChanged(e);
            if (base.Control != null && e.NewElement != null)
            {
                base.Control.Source = new CustomTableModelRenderer(e.NewElement);
            }
        }
    }
}
