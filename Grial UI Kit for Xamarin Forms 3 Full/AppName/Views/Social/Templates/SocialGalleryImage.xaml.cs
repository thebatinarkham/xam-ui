using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class SocialGalleryImage : ContentView
    {
        public static readonly BindableProperty ImageProperty =
            BindableProperty.Create(
                nameof(Image),
                typeof(ImageSource),
                typeof(SocialGalleryImage),
                null);

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public SocialGalleryImage()
        {
            InitializeComponent();
        }

        private async void OnImageTapped(object sender, EventArgs e)
        {
#if !NAVIGATION
            var selectedItem = (SocialGalleryImage)sender;
            var socialGalleryImagePreview = new SocialGalleryImagePreviewPage(selectedItem.Image);

            await Navigation.PushModalAsync(new NavigationPage(socialGalleryImagePreview));
#endif
        }
    }
}