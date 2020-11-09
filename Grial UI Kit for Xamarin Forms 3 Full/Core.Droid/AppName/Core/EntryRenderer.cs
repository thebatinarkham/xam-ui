using Android.Content;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppName.Core
{
    public class EntryRenderer : Xamarin.Forms.Platform.Android.EntryRenderer
    {
        private readonly EntryRendererHelper<FormsEditText> _helper;

        public EntryRenderer(Context context)
            : base(context)
        {
            _helper = new EntryRendererHelper<FormsEditText>(this, context);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
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
            _helper.Dispose();
            base.Dispose(disposing);
        }
    }
}
