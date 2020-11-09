using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class NavigationListWithImagesPage : ContentPage
    {
        public NavigationListWithImagesPage()
        {
            InitializeComponent();

            BindingContext = new NavigationViewModel(variantPageName: $"{GetType().Name}.xaml");
        }
    }
}
