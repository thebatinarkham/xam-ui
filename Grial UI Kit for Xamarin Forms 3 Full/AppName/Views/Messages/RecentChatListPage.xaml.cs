using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class RecentChatListPage : ContentPage
    {
        public RecentChatListPage()
        {
            InitializeComponent();

            BindingContext = new ChatMessagesListViewModel(variantPageName: $"{GetType().Name}.xaml");
        }
    }
}
