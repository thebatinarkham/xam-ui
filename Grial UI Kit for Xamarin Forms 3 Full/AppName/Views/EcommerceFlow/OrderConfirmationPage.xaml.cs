using System;
using System.Collections.Generic;

using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class OrderConfirmationPage : ContentPage
    {
        public OrderConfirmationPage()
            : this(null)
        {
        }

        public OrderConfirmationPage(FlowProductData product)
        {
            InitializeComponent();

            BindingContext = new OrderConfirmationViewModel(product);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            Triggers.Clear();
        }

        private async void OnClicked(object sender, EventArgs e)
        {
#if !NAVIGATION
            await Navigation.PushAsync(new CheckoutPage((OrderConfirmationViewModel)BindingContext));
#endif
        }

        private async void OnDelete(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
