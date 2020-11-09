using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class PerformanceDashboardNavigationPage
    {
        public PerformanceDashboardNavigationPage()
        {
        }

        public PerformanceDashboardNavigationPage(Page root)
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
