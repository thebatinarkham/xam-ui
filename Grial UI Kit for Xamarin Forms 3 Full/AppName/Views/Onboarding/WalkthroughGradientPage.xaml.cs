using AppName.Core;
namespace AppName
{
    public partial class WalkthroughGradientPage : WalkthroughBasePage
    {
        public WalkthroughGradientPage()
        {
            InitializeComponent();

            BindingContext = new WalkthroughViewModel(Close, MoveNext);
        }
    }
}
