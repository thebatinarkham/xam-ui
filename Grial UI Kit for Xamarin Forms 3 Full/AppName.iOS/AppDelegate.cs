using Foundation;
using UIKit;
using Xamarin.Forms;
using FFImageLoading.Forms.Platform;
using CarouselView.FormsPlugin.iOS;
using System;
using FFImageLoading.Svg.Forms;
using AppName.Core;

namespace AppName.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            global::Xamarin.Forms.Forms.Init();

            /*Esta línea permite que el dispositivo ingrese al modo de suspensión, por defecto debería 
             * ser falsa, pero sin esta asignación explícita nunca duerme con los últimos Xamarin.Forms. 
             * Configúrelo como verdadero si necesita evitar que el dispositivo entre en modo de suspensión 
             * mientras se muestra la aplicación*/
            UIApplication.SharedApplication.IdleTimerDisabled = false;

            var ignore = typeof(SvgCachedImage);
            CachedImageRenderer.Init(); // Inicializando FFImageLoading

            CarouselViewRenderer.Init(); // Inicializando CarouselView

            Rg.Plugins.Popup.Popup.Init();

            GrialKit.Init(new ThemeColors());

            // Xamarin Test Cloud Agent
#if ENABLE_TEST_CLOUD
            Xamarin.Calabash.Start();
#endif

            FormsHelper.ForceLoadingAssemblyContainingType<FFImageLoading.Transformations.BlurredTransformation>();

            ReferenceCalendars();

            LoadApplication(new App());

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        private void ReferenceCalendars()
        {
            // When compiling in release, you may need to instantiate the specific
            // calendar so it doesn't get stripped out by the linker. Just uncomment
            // the lines you need according to the localization needs of the app.
            // For instance, in 'ar' cultures UmAlQuraCalendar is required.
            // https://bugzilla.xamarin.com/show_bug.cgi?id=59077

            //new System.Globalization.UmAlQuraCalendar();
            // new System.Globalization.ChineseLunisolarCalendar();
            // new System.Globalization.ChineseLunisolarCalendar();
            // new System.Globalization.HebrewCalendar();
            // new System.Globalization.HijriCalendar();
            // new System.Globalization.IdnMapping();
            // new System.Globalization.JapaneseCalendar();
            // new System.Globalization.JapaneseLunisolarCalendar();
            // new System.Globalization.JulianCalendar();
            // new System.Globalization.KoreanCalendar();
            // new System.Globalization.KoreanLunisolarCalendar();
            // new System.Globalization.PersianCalendar();
            // new System.Globalization.TaiwanCalendar();
            // new System.Globalization.TaiwanLunisolarCalendar();
            // new System.Globalization.ThaiBuddhistCalendar();
        }
    }
}
