using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class PerformanceDashboardMainPage : ContentPage
    {
        public PerformanceDashboardMainPage()
        {
            InitializeComponent();

            BindingContext = new PerformanceDashboardMainViewModel();
        }

        private async void OnItemSelected(object sender, System.EventArgs e)
        {
#if !NAVIGATION
            var page = new EmployeePerformanceDashboardPage(((DataGrid)sender).SelectedItem as FlowEmployeeData);

            await Navigation.PushAsync(page);
#endif
        }
    }
}
