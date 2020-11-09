using AppName.Core;
namespace AppName
{
    public partial class WalkthroughIllustrationPage : WalkthroughBasePage
    {
        public WalkthroughIllustrationPage()
        {
            InitializeComponent();

            BindingContext = new WalkthroughViewModel(Close, MoveNext);
        }
    }
}
