using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class SocialCardPage : ContentPage
    {
        public SocialCardPage()
        {
            InitializeComponent();

            BindingContext = new SocialViewModel(variantPageName: "SocialCardPage.xaml");
        }
    }
}
