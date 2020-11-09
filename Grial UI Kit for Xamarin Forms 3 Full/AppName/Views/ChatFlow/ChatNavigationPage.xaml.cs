using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class ChatNavigationPage
    {
        public ChatNavigationPage()
        {
        }

        public ChatNavigationPage(Page root)
            : base(root)
        {
            InitializeComponent();
        }

        private void OnClose(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}
