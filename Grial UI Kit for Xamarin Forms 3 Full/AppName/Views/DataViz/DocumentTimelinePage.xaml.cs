using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class DocumentTimelinePage : ContentPage
    {
        public DocumentTimelinePage()
        {
            InitializeComponent();

            BindingContext = new DocumentTimelineViewModel();
        }
    }
}
