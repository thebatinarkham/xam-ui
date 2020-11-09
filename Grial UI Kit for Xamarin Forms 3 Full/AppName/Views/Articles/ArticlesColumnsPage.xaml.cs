using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class ArticlesColumnsPage : ContentPage
    {
        public ArticlesColumnsPage()
        {
            InitializeComponent();

            BindingContext = new ArticlesColumnsViewModel();
        }

        private async void OnItemTapped(object sender, EventArgs e)
        {
#if !NAVIGATION
            var item = ((BindableObject)sender).BindingContext;
            var articlePage = new ArticleDetailPage(item as ArticleData);

            await Navigation.PushAsync(articlePage);
#endif
        }
    }
}
