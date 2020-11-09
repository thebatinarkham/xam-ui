using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class DashboardItemTemplate : DashboardItemTemplateBase
    {
        public DashboardItemTemplate()
        {
            InitializeComponent();
        }

        protected override void OnTapped()
        {
            Application.Current.MainPage.DisplayAlert("Item Tapped!", "You have tapped a DashboardItemTemplate", "OK");
        }
    }
}