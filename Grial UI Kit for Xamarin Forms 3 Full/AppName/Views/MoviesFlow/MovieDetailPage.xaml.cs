using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class MovieDetailPage : ContentPage
    {
        public MovieDetailPage()
            : this(null)
        {
        }

        public MovieDetailPage(FlowMovieData movie)
        {
            InitializeComponent();

            BindingContext = new MovieDetailViewModel(movie);
        }
    }
}