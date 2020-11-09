using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class IconsDashboardPage : ContentPage
    {
        public IconsDashboardPage()
        {
            InitializeComponent();

            BindingContext = new NavigationViewModel(variantPageName: $"{GetType().Name}.xaml");
        }
    }
}