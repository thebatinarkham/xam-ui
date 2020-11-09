using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class PopupsPage : ContentPage
    {
        public PopupsPage()
        {
            InitializeComponent();
        }

        private async void OpenSimpleDialog(object sender, EventArgs e)
        {
            var dialog = new SimpleDialog();
            await PopupNavigation.Instance.PushAsync(dialog);
        }

        private async void OpenSimpleDialogNoTitle(object sender, EventArgs e)
        {
            var dialog = new SimpleDialogNoTitle();
            await PopupNavigation.Instance.PushAsync(dialog);
        }

        private async void OpenSimpleDialogNoTitleInverse(object sender, EventArgs e)
        {
            var dialog = new SimpleDialogNoTitleInverse();
            await PopupNavigation.Instance.PushAsync(dialog);
        }

        private async void OpenIrregularDialog(object sender, EventArgs e)
        {
            var dialog = new IrregularDialog();
            await PopupNavigation.Instance.PushAsync(dialog);
        }

        private async void OpenNotificationPopup(object sender, System.EventArgs e)
        {
            var dialog = new NotificationPopup { Message = "Use this notification to provide visual feedback about user actions." };
            await PopupNavigation.Instance.PushAsync(dialog);
        }

        private async void OpenCustomActionSheet(object sender, System.EventArgs e)
        {
            var dialog = new CustomActionSheet();

            // Use custom view model to display sample data
            dialog.BindingContext = new CustomActionSheetViewModel();

            await PopupNavigation.Instance.PushAsync(dialog);
        }

        private void ClosePopup(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}
