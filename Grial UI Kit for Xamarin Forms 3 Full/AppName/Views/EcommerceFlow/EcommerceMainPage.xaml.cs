using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class EcommerceMainPage : ContentPage
    {
        public EcommerceMainPage()
        {
            InitializeComponent();

            BindingContext = new EcommerceMainViewModel();
        }
    }
}
