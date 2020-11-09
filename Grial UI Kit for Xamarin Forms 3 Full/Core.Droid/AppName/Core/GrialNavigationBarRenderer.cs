using Android.Content;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppName.Core
{
    public class GrialNavigationBarRenderer : VisualElementRenderer<View>, IGrialNavigationBarRenderer
    {
        private readonly GrialNavigationBarRendererHelper _helper;

        public GrialNavigationBarRenderer(Context context)
            : base(context)
        {
            _helper = new GrialNavigationBarRendererHelper(this);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            _helper.Reset();
            base.OnElementChanged(e);
            if (base.Element != null)
            {
                _helper.Setup();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            _helper.PropertyChanged(e);
        }

        protected override void Dispose(bool disposing)
        {
            _helper.Reset();
            base.Dispose(disposing);
        }

        View IGrialNavigationBarRenderer.Element => base.Element;
    }
}
