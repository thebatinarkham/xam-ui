using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class ContactDetailPage : ContentPage
    {
        public ContactDetailPage()
            : this(null)
        {
        }

        public ContactDetailPage(FlowContactData contact = null)
        {
            InitializeComponent();

            BindingContext = new ContactDetailViewModel(contact?.Id);
        }

        private async void OnEdit(object sender, EventArgs e)
        {
#if !NAVIGATION
            var page = new AddContactPage(((BindableObject)sender).BindingContext as FlowContactData);
            await Navigation.PushAsync(page);
#endif
        }

        private async void OnMessage(object sender, EventArgs e)
        {
#if !NAVIGATION
            var prev = Navigation.NavigationStack.Count - 2;
            if (prev < 0 || !(Navigation.NavigationStack[prev] is ChatMessagesPage))
            {
                var page = new ChatMessagesPage(((BindableObject)sender).BindingContext as FlowContactData);
                await Navigation.PushAsync(page);
            }
            else
            {
                await Navigation.PopAsync();
            }
#endif
        }

        private async void OnEmail(object sender, EventArgs e)
        {
            await DisplayAlert("Email tapped", "Nothing to see here, try messages :)", Resx.AppResources.StringOK);
        }

        private async void OnHome(object sender, EventArgs e)
        {
            await DisplayAlert("Home tapped", "Nothing to see here, try messages :)", Resx.AppResources.StringOK);
        }

        private async void OnMobile(object sender, EventArgs e)
        {
            await DisplayAlert("Mobile tapped", "Nothing to see here, try messages :)", Resx.AppResources.StringOK);
        }
    }
}
