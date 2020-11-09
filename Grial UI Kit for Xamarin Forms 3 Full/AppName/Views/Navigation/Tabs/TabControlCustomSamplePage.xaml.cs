using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class TabControlCustomSamplePage : ContentPage
    {
        public TabControlCustomSamplePage()
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