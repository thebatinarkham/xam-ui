using Android.Content;
using Android.Views;
using Xamarin.Forms;

namespace AppName.Core
{
    public class ViewCellRenderer : Xamarin.Forms.Platform.Android.ViewCellRenderer
    {
        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            Android.Views.View cellCore = base.GetCellCore(item, convertView, parent, context);
            TextCellRenderer.SetColors(cellCore);
            return cellCore;
        }
    }
}
