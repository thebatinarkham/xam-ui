using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class NavigationFlatListPage : ContentPage
    {
        public NavigationFlatListPage()
        {
            InitializeComponent();

            BindingContext = new NavigationViewModel(variantPageName: $"{GetType().Name}.xaml");
        }
    }
}
