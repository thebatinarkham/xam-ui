using Android.Content;
using Android.Views;
using Android.Widget;
using System.ComponentModel;
using Xamarin.Forms.Platform.Android;

namespace AppName.Core
{
    public class DatePickerRenderer : Xamarin.Forms.Platform.Android.DatePickerRenderer
    {
        private readonly PickerRendererHelper<Xamarin.Forms.DatePicker, EditText> _helper;

        public DatePickerRenderer(Context context)
            : base(context)
        {
            _helper = new PickerRendererHelper<Xamarin.Forms.DatePicker, EditText>(this, SetTextAlignment, context);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);
            _helper.OnElementChangedHandler(e);
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
            base.Dispose(disposing);
            _helper.Dispose();
        }

        private void SetTextAlignment(GravityFlags gravity)
        {
            base.Control.Gravity = gravity;
        }
    }
}
