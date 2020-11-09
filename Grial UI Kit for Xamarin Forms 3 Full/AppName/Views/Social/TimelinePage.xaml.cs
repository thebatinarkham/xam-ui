using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class TimelinePage : ContentPage
    {
        public TimelinePage()
        {
            InitializeComponent();

            BindingContext = new TimelineViewModel();
        }
    }
}