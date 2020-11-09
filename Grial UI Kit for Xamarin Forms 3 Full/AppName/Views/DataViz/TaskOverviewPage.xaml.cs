using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class TaskOverviewPage : ContentPage
    {
        public TaskOverviewPage()
        {
            InitializeComponent();

            BindingContext = new TaskOverviewViewModel();
        }
    }
}
