using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class TabControlBottomPlacementSamplePage : ContentPage
    {
        public TabControlBottomPlacementSamplePage()
        {
            InitializeComponent();

            BindingContext = new
            {
                Social = new SocialViewModel(),
                Chat = new ChatMessagesListViewModel()
            };
        }
    }
}