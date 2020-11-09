using FFImageLoading.Svg.Forms;
using FFImageLoading.Transformations;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class CurvedMask : ContentView
    {
        public static readonly BindableProperty MaskColorProperty =
            BindableProperty.Create(
                nameof(MaskColor),
                typeof(Color),
                typeof(CurvedMask),
                defaultValue: Color.Default,
                propertyChanged: OnMaskColorChanged);

        public Color MaskColor
        {
            get { return (Color)GetValue(MaskColorProperty); }
            set { SetValue(MaskColorProperty, value); }
        }

        public CurvedMask()
        {
            InitializeComponent();
        }

        private static void OnMaskColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((CurvedMask)bindable).Update();
        }

        private void Update()
        {
            root.Children.Clear();

            if (MaskColor != Color.Default)
            {
                var image = new SvgCachedImage
                {
                    Style = Resources["MaskImageStyle"] as Style
                };

                image.Transformations.Add(new TintTransformation(MaskColor.ToHexString()) { EnableSolidColor = true });
                root.Children.Add(image);
            }
        }
    }
}
