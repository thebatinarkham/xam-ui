using AppName.Core;
namespace AppName
{
    public partial class WalkthroughMinimalPage : WalkthroughBasePage
    {
        public WalkthroughMinimalPage()
        {
            InitializeComponent();

            BindingContext = new WalkthroughViewModel(Close, MoveNext);
        }
    }
}