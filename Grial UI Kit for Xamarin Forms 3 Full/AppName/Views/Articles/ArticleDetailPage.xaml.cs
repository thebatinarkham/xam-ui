using System;
using AppName.Resx;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class ArticleDetailPage : ContentPage
    {
        public ArticleDetailPage() 
            : this(null)
        {
        }

        public ArticleDetailPage(ArticleData article)
        {
            InitializeComponent();

            BindingContext = new ArticleDetailViewModel(article?.Id);
        }

        public void OnMore(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            DisplayAlert(string.Format(AppResources.StringContextActionTapped, mi.Text), string.Format(AppResources.StringTappedOnContextAction, mi.Text), AppResources.StringOK);
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            DisplayAlert(string.Format(AppResources.StringContextActionTapped, mi.Text), string.Format(AppResources.StringTappedOnContextAction, mi.Text), AppResources.StringOK);
        }

        public void OnItemTapped(object sender, EventArgs e)
        {
            var comment = (ArticleCommentData)((ListView)sender).SelectedItem;
            DisplayAlert(AppResources.StringMessageTapped, string.Format(AppResources.StringTappedOnAuthorMessage, comment.From.Name), AppResources.StringOK);
        }

        public void OnPrimaryActionButtonClicked(object sender, EventArgs e)
        {
            DisplayAlert(AppResources.StringButtonTapped, AppResources.ButtonAddComment, AppResources.StringOK);
        }
    }
}