using System;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class ChatMessagesPage : ContentPage
    {
        public ChatMessagesPage()
            : this(null)
        {
        }

        public ChatMessagesPage(FlowContactData contact)
        {
            InitializeComponent();

            BindingContext = new ChatMessagesViewModel(contact?.Id);
        }

        private async void OnAvatarTapped(object sender, EventArgs args)
        {
#if !NAVIGATION
            var popup = new ContactPreviewPopup(Navigation)
            { 
                BindingContext = ((BindableObject)sender).BindingContext 
            };

            await PopupNavigation.Instance.PushAsync(popup);
#endif
        }

        private async void OnLabelTapped(object sender, EventArgs args)
        {
            var contact = ((VisualElement)sender).BindingContext as FlowContactData;
#if !NAVIGATION
            await Navigation.PushAsync(new ContactDetailPage(contact));
#endif
        }
    }
}
