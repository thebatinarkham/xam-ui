using AppName.Core;
namespace AppName
{
    public partial class WalkthroughVariantPage : WalkthroughBasePage
    {
        public WalkthroughVariantPage()
        {
            InitializeComponent();

            BindingContext = new WalkthroughViewModel(Close, MoveNext);
        }
    }
}