using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppName.Core
{
    public class EntryCellRenderer : Xamarin.Forms.Platform.Android.EntryCellRenderer, IDisposable
    {
        private bool disposed;

        private static bool _propertyNotAvailable;

        private static PropertyInfo _editTextProperty;

        private WeakReference<EditText> _editView;

        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            Android.Views.View cellCore = base.GetCellCore(item, convertView, parent, context);
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop && TryGetEditText(cellCore, out EditText edit))
            {
                _editView = new WeakReference<EditText>(edit);
                edit.FocusChange += FocusChanged;
            }
            return cellCore;
        }

        private void FocusChanged(object sender, Android.Views.View.FocusChangeEventArgs e)
        {
            EditText editText = sender as EditText;
            if (editText != null && editText.Background != null)
            {
                if (e.HasFocus)
                {
                    editText.Background.SetColorFilter(ColorCache.AccentColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                }
                else
                {
                    editText.Background.ClearColorFilter();
                }
            }
        }

        private static bool TryGetEditText(Android.Views.View cell, out EditText edit)
        {
            edit = null;
            if (_propertyNotAvailable)
            {
                return false;
            }
            if (_editTextProperty == null)
            {
                _editTextProperty = cell.GetType().GetProperty("EditText");
                if (_editTextProperty == null)
                {
                    _propertyNotAvailable = true;
                    return false;
                }
            }
            edit = (_editTextProperty.GetValue(cell) as EditText);
            return edit != null;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                disposed = true;
                if (_editView != null && _editView.TryGetTarget(out EditText target))
                {
                    target.FocusChange -= FocusChanged;
                }
            }
        }
    }
}
