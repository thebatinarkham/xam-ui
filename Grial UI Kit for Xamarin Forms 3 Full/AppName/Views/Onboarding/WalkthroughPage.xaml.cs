using AppName.Core;
namespace AppName
{
    public partial class WalkthroughPage : WalkthroughBasePage
    {
        public WalkthroughPage()
        {
            InitializeComponent();

            BindingContext = new WalkthroughViewModel(Close, MoveNext);
        }
    }
}