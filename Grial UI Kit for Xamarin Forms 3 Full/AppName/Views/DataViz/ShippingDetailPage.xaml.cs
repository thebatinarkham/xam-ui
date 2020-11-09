using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class ShippingDetailPage : ContentPage
    {
        public ShippingDetailPage()
        {
            InitializeComponent();

            BindingContext = new ShippingDetailViewModel();
        }
    }
}
