using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class NavigationCardsListPage : ContentPage
    {
        public NavigationCardsListPage()
        {
            InitializeComponent();

            BindingContext = new NavigationViewModel(variantPageName: $"{GetType().Name}.xaml");
        }
    }
}
