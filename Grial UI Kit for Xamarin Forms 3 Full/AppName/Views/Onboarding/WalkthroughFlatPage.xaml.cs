using AppName.Core;
namespace AppName
{
    public partial class WalkthroughFlatPage : WalkthroughBasePage
    {
        public WalkthroughFlatPage()
        {
            InitializeComponent();

            BindingContext = new WalkthroughViewModel(Close, MoveNext);
        }
    }
}