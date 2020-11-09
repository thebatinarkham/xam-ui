using AppName.Core;
namespace AppName
{
    public partial class WalkthroughImagePage : WalkthroughBasePage
    {
        public WalkthroughImagePage()
        {
            InitializeComponent();

            BindingContext = new WalkthroughViewModel(Close, MoveNext);
        }
    }
}
