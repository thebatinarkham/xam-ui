using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class TabControlAndroidSamplePage : ContentPage
    {
        public TabControlAndroidSamplePage()
        {
            InitializeComponent();

            BindingContext = new
            {
                Social = new SocialViewModel(),
                Chat = new ChatMessagesListViewModel()
            };
        }
    }
}