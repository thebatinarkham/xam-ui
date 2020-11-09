using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class ChatMainPage : ContentPage
    {
        public ChatMainPage()
        {
            InitializeComponent();

            BindingContext = new ChatMainViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            contacts.SelectedItem = null;
            conversations.SelectedItem = null;
        }

        private async void OnMessageTapped(object sender, ItemTappedEventArgs e)
        {
#if !NAVIGATION
            var item = ((ListView)sender).SelectedItem as FlowConversationData;
            var page = new ChatMessagesPage(item?.From);

            await Navigation.PushAsync(page);
#endif
        }

        private async void OnContactTapped(object sender, ItemTappedEventArgs e)
        {
#if !NAVIGATION
            var contact = ((ListView)sender).SelectedItem as FlowContactData;
            await Navigation.PushAsync(new ContactDetailPage(contact));
#endif
        }

        private async void OnAddContactClicked(object sender, EventArgs e)
        {
#if !NAVIGATION
            await Navigation.PushAsync(new AddContactPage());
#endif
        }
    }
}
