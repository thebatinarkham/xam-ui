using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class TaskBrowserPage : ContentPage
    {
        public TaskBrowserPage()
        {
            InitializeComponent();

            BindingContext = new TaskBrowserViewModel();
        }
    }
}
