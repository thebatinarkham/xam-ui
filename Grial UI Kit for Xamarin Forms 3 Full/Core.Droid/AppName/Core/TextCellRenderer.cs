using Android.Content;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppName.Core
{
    public class TextCellRenderer : Xamarin.Forms.Platform.Android.TextCellRenderer
    {
        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            Android.Views.View cellCore = base.GetCellCore(item, convertView, parent, context);
            SetColors(cellCore);
            return cellCore;
        }

        public static void SetColors(Android.Views.View view)
        {
            (view as BaseCellView)?.SetDefaultMainTextColor(ColorCache.ListViewItemTextColor);
        }
    }
}
