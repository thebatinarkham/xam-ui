using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Widget;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppName.Core
{
    public class SwitchCellView : BaseCellView, CompoundButton.IOnCheckedChangeListener, IJavaObject, IDisposable
    {
        public SwitchCell Cell
        {
            get;
            set;
        }

        public SwitchCellView(Context context, Cell cell)
            : base(context, cell)
        {
            SwitchCompat switchCompat = new SwitchCompat(context);
            switchCompat.SetOnCheckedChangeListener(this);
            SetAccessoryView(switchCompat);
            SetImageVisible(visible: false);
        }

        public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        {
            Cell.On = isChecked;
        }
    }
}
