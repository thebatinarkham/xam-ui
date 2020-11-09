using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class MoviesNavigationPage
    {
        public MoviesNavigationPage()
        {
        }

        public MoviesNavigationPage(Page root)
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
