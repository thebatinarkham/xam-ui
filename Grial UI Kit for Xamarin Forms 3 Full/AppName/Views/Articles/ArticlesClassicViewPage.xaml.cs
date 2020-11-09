using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class ArticlesClassicViewPage : ContentPage
    {
        public ArticlesClassicViewPage()
        {
            InitializeComponent();

            BindingContext = new ArticlesListViewModel(variantPageName: $"{GetType().Name}.xaml");
        }

        private async void OnItemTapped(object sender, EventArgs e) 
        {
#if !NAVIGATION
            var selectedItem = ((ListView)sender).SelectedItem;
            var articlePage = new ArticleDetailPage(selectedItem as ArticleData);

            await Navigation.PushAsync(articlePage);
#endif
        }
    }
}
