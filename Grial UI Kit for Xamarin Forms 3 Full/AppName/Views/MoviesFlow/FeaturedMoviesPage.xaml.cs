using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class FeaturedMoviesPage : ContentPage
    {
        public FeaturedMoviesPage()
            : this(null)
        {
        }

        public FeaturedMoviesPage(FeaturedMoviesViewModel context)
        {
            InitializeComponent();

            BindingContext = context ?? new FeaturedMoviesViewModel();
        }
    }
}
