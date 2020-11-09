using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class DashboardMultipleTilesPage : ContentPage
    {
        public DashboardMultipleTilesPage()
        {
            InitializeComponent();

            BindingContext = new DashboardMultipleTilesViewModel();
        }
    }
}