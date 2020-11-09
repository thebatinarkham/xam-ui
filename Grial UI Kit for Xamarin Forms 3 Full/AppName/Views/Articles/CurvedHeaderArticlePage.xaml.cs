using System;
using AppName.Resx;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class CurvedHeaderArticlePage : ContentPage
    {
        public CurvedHeaderArticlePage()
        {
            InitializeComponent();

            BindingContext = new ComplexArticleDetailViewModel(pageName: $"{GetType().Name}.xaml");
        }

        public void OnPrimaryActionButtonClicked(object sender, EventArgs e)
        {
            DisplayAlert(AppResources.StringButtonTapped, AppResources.ButtonAddComment, AppResources.StringOK);
        }
    }
}
