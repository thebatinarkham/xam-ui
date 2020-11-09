using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class DashboardCarouselPage : ContentPage
    {
        public DashboardCarouselPage()
        {
            InitializeComponent();

            BindingContext = new DashboardCarouselViewModel();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            Triggers.Clear();
        }
    }
}