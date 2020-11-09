using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class SamplesListPage : ContentPage
    {
        public SamplesListPage(int categoryId)
        {
            InitializeComponent();

            BindingContext = new SamplesCategoryViewModel(Navigation, categoryId);
        }

        public SamplesListPage(FlowType flowType)
        {
            InitializeComponent();

            BindingContext = new SamplesFlowViewModel(Navigation, flowType);
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            // Workaround to avoid using SelectedItem binding as it does ugly things with the 
            // selected cell background in iOS, and through SelectionMode=None we completely
            // remove the selection style in iOS through our cell renderer
            ((SampleListViewModelBase)BindingContext).SampleSelected((Sample)e.Item);
        }
    }
}