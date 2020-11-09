using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class DashboardAppNotificationItemTemplate : DashboardItemTemplateBase
    {
        public DashboardAppNotificationItemTemplate()
        {
            InitializeComponent();
        }

        protected override void OnTapped()
        {
            Application.Current.MainPage.DisplayAlert("Tile Tapped!", "You have tapped a DashboardAppNotificationItemTemplate", "OK");
        }
    }
}