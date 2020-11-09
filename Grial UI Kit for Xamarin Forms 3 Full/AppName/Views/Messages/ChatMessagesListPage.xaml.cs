using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class ChatMessagesListPage : ContentPage
    {
        public ChatMessagesListPage()
        {
            InitializeComponent();

            BindingContext = new ChatMessagesListViewModel();
        }
    }
}