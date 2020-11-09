using System;
using System.Collections.Generic;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class NotificationsPage : ContentPage
    {
        public NotificationsPage()
        {
            InitializeComponent();

            BindingContext = new NotificationsViewModel();
        }
    }
}
