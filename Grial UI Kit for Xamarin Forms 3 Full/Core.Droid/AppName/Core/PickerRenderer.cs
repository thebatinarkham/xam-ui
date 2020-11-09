using Android.Content;
using Android.Views;
using Android.Widget;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppName.Core
{
    public class PickerRenderer : Xamarin.Forms.Platform.Android.AppCompat.PickerRenderer
    {
        private readonly PickerRendererHelper<Picker, EditText> _helper;

        public PickerRenderer(Context context)
            : base(context)
        {
            _helper = new PickerRendererHelper<Picker, EditText>(this, SetTextAlignment, context);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            _helper.OnElementChangedHandler(e);
        }

        private void SetTextAlignment(GravityFlags gravity)
        {
            base.Control.Gravity = gravity;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            _helper.OnElementPropertyChangedHandler(sender, e);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            _helper.OnSizeChangedHandler(w, h, oldw, oldh);
        }

        protected override void Dispose(bool disposing)
        {
            _helper.Dispose();
            base.Dispose(disposing);
        }
    }
}
