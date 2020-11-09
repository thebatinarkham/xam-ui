using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class NavigationListWithIconsPage : ContentPage
    {
        public NavigationListWithIconsPage()
        {
            InitializeComponent();

            BindingContext = new NavigationViewModel(variantPageName: $"{GetType().Name}.xaml");
        }
    }
}
