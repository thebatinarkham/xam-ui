using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();

            BindingContext = new ProfileViewModel();
        }
    }
}