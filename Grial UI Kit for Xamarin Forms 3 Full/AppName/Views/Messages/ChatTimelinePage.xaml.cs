using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class ChatTimelinePage : ContentPage
    {
        public ChatTimelinePage()
        {
            InitializeComponent();

            BindingContext = new ChatMessagesListViewModel(variantPageName: $"{GetType().Name}.xaml");
        }
    }
}
