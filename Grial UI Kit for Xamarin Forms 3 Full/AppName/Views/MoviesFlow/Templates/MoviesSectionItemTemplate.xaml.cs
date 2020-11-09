using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class MoviesSectionItemTemplate : ContentView
    {
        public MoviesSectionItemTemplate()
        {
            InitializeComponent();
        }
        
        private async void OnTapGestureRecognizerTapped(object sender, EventArgs args) 
        {
#if !NAVIGATION
            var movieDetailPage = new MovieDetailPage(((VisualElement)sender).BindingContext as FlowMovieData);

            await Navigation.PushAsync(movieDetailPage);
#endif
        }
    }
}
