using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace AppName.Core
{
    public class SwitchCellRenderer : Xamarin.Forms.Platform.Android.SwitchCellRenderer, IDisposable
    {
        private const double DefaultHeight = 30.0;

        private bool disposed;

        private SwitchCellView view;

        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
            {
                return GetCellCoreBase(item, convertView, parent, context);
            }
            return base.GetCellCore(item, convertView, parent, context);
        }

        private Android.Views.View GetCellCoreBase(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            SwitchCell cell = (SwitchCell)base.Cell;
            if ((view = (convertView as SwitchCellView)) == null)
            {
                view = new SwitchCellView(context, item);
            }
            view.Cell = cell;
            UpdateText();
            UpdateChecked();
            UpdateHeight();
            return view;
        }

        protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
            {
                if (args.PropertyName == SwitchCell.TextProperty.PropertyName)
                {
                    UpdateText();
                }
                else if (args.PropertyName == SwitchCell.OnProperty.PropertyName)
                {
                    UpdateChecked();
                }
                else if (args.PropertyName == "RenderHeight")
                {
                    UpdateHeight();
                }
            }
            else
            {
                base.OnCellPropertyChanged(sender, args);
            }
        }

        private void UpdateText()
        {
            view.MainText = ((SwitchCell)base.Cell).Text;
        }

        private void UpdateChecked()
        {
            ((SwitchCompat)view.AccessoryView).Checked = ((SwitchCell)base.Cell).On;
        }

        private void UpdateHeight()
        {
            view.SetRenderHeight(base.Cell.RenderHeight);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposing || disposed)
            {
                return;
            }
            disposed = true;
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
            {
                SwitchCellView switchCellView = view;
                if (switchCellView != null)
                {
                    SwitchCompat switchCompat = (SwitchCompat)switchCellView.AccessoryView;
                    switchCompat.SetOnCheckedChangeListener(null);
                }
            }
        }
    }
}
