using Xamarin.Forms;

namespace AppName
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //Idioma
            Resources["DefaultStringResources"] = new Resx.AppResources();

            SamplesCatalog.Initialize();

            //P�gina principal
            MainPage = GetMainPage();
        }

        public static Page GetMainPage()
{
            return new RootMasterDetailPage();
          }
    }
}
