using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class NotificationPopup : PopupPage
    {
        public static readonly BindableProperty MessageProperty =
            BindableProperty.Create(
                nameof(Message),
                typeof(string),
                typeof(NotificationPopup),
                defaultValue: string.Empty);

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public NotificationPopup()
        {
            InitializeComponent();
        }

        private void OnClose(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}
