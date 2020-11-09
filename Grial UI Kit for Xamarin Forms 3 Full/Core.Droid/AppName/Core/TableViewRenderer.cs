using Android.Content;
using Android.Views;
using Android.Widget;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppName.Core
{
    public class TableViewRenderer : Xamarin.Forms.Platform.Android.TableViewRenderer
    {
        private class CustomTableViewModelRenderer : TableViewModelRenderer
        {
            private readonly WeakReference<TableView> _tableView;

            public CustomTableViewModelRenderer(Context context, Android.Widget.ListView listView, TableView view)
                : base(context, listView, view)
            {
                _tableView = new WeakReference<TableView>(view);
            }

            public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
            {
                Android.Views.View view = base.GetView(position, convertView, parent);
                if (GetCellForPosition(position).GetType() != typeof(TextCell))
                {
                    return view;
                }
                if (_tableView.TryGetTarget(out TableView target))
                {
                    Xamarin.Forms.Color headerFooterTextColor = TableViewProperties.GetHeaderFooterTextColor(target);
                    if (headerFooterTextColor != Xamarin.Forms.Color.Default)
                    {
                        Android.Graphics.Color color = headerFooterTextColor.ToAndroid();
                        LinearLayout linearLayout = view as LinearLayout;
                        if (linearLayout != null && linearLayout.ChildCount >= 1)
                        {
                            LinearLayout linearLayout2 = linearLayout.GetChildAt(0) as LinearLayout;
                            if (linearLayout2 != null && linearLayout2.ChildCount >= 2)
                            {
                                LinearLayout linearLayout3 = linearLayout2.GetChildAt(1) as LinearLayout;
                                if (linearLayout3 != null && linearLayout3.ChildCount >= 2)
                                {
                                    (linearLayout3.GetChildAt(0) as TextView)?.SetTextColor(color);
                                    Android.Views.View childAt = linearLayout.GetChildAt(1);
                                    childAt.SetBackgroundColor(color);
                                }
                            }
                        }
                    }
                }
                return view;
            }
        }

        public TableViewRenderer(Context context)
            : base(context)
        {
        }

        protected override TableViewModelRenderer GetModelRenderer(Android.Widget.ListView listView, TableView view)
        {
            return new CustomTableViewModelRenderer(base.Context, listView, view);
        }
    }
}
