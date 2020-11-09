using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class ContactPreviewPopup : PopupPage
    {
        private readonly INavigation _navigation;

        public ContactPreviewPopup(INavigation navigation)
        {
            _navigation = navigation;

            InitializeComponent();
        }

        private void OnClose(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        private async void OnMessageIconTapped(object sender, EventArgs e)
        {
#if !NAVIGATION
            await PopupNavigation.Instance.PopAsync();

            var last = _navigation.NavigationStack.Count - 1;
            if (last < 0 || !(_navigation.NavigationStack[last] is ChatMessagesPage))
            {
                var page = new ChatMessagesPage(BindingContext as FlowContactData);
                await _navigation.PushAsync(page);
            }
#endif
        }

        private async void OnPhoneIconTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Phone tapped", "Nothing to see here, try chat or info :)", Resx.AppResources.StringOK);
        }

        private async void OnVideoIconTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Video tapped", "Nothing to see here, try chat or info :)", Resx.AppResources.StringOK);
        }

        private async void OnInfoIconTapped(object sender, EventArgs e)
        {
#if !NAVIGATION
            await PopupNavigation.Instance.PopAsync();

            var page = new ContactDetailPage(BindingContext as FlowContactData);
            await _navigation.PushAsync(page);
#endif
        }
    }
}
