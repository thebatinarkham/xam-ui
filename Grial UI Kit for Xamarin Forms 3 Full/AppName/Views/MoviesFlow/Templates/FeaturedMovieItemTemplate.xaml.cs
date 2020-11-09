using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class FeaturedMovieItemTemplate : ContentView
    {
        public FeaturedMovieItemTemplate()
        {
            InitializeComponent();
        }

        private void OnMoveToSecond(object sender, System.EventArgs e)
        {
            carousel.Position = 1;
        }
    }
}
