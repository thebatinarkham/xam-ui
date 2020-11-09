using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class ArticlesBrowserPage : ContentPage
    {
        public ArticlesBrowserPage()
        {
            InitializeComponent();

            BindingContext = new ArticlesBrowserViewModel();
        }
    }
}
