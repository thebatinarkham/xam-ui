using System;
using System.Collections.Generic;

using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class VideoCarouselHighlightsPage : ContentPage
    {
        public VideoCarouselHighlightsPage()
        {
            InitializeComponent();

            BindingContext = new VideoCarouselHighlightsViewModel();
        }

        private async void OnCloseButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }
    }
}
