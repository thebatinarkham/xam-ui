using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class TabControliOSSamplePage : ContentPage
    {
        public TabControliOSSamplePage()
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