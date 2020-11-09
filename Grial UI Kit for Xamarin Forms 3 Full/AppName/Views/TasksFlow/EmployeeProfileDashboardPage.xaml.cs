using System;

using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class EmployeeProfileDashboardPage : ContentPage
    {
        public EmployeeProfileDashboardPage()
            : this(null)
        {
        }

        public EmployeeProfileDashboardPage(FlowEmployeeData employee)
        {
            InitializeComponent();

            BindingContext = new EmployeeProfileDashboardViewModel(employee);
        }

        private async void OnEmployee(object sender, EventArgs e)
        {
#if !NAVIGATION
            var page = new EmployeePerformanceDashboardPage
            {
                BindingContext = new EmployeePerformanceDashboardViewModel(((BindableObject)sender).BindingContext as FlowEmployeeData)
            };

            await Navigation.PushAsync(page);
#endif
        }
    }
}
