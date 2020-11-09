using System;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class ChatPreviewItemTemplate : ContentView
    {
        public ChatPreviewItemTemplate()
        {
            InitializeComponent();
        }

        private async void OnAvatarTapped(object sender, EventArgs e)
        {
            var popup = new ContactPreviewPopup(GetNavigation())
            {
                BindingContext = ((BindableObject)sender).BindingContext
            };

            await PopupNavigation.Instance.PushAsync(popup);
        }

        private INavigation GetNavigation()
        {
            // If the item is rendered inside a ListView we need to get the navigation proxy
            // from the ListView itself for navigation methods to work
            if (Parent is Cell cell)
            {
                if (cell.Parent is ListView list)
                {
                    return list.Navigation;
                }
            }

            return Navigation;
        }
    }
}
