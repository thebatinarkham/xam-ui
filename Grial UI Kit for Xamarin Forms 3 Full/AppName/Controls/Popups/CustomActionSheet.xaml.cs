using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using AppName.Core;

namespace AppName
{
    public partial class CustomActionSheet : PopupPage
    {
        public CustomActionSheet()
        {
            InitializeComponent();
        }

        private void OnClose(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}
