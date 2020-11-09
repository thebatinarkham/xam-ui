using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics.Drawables;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using System.Threading.Tasks;

using Java.Util;
using AppName.Core;


namespace AppName.Droid
{
    //https://developer.android.com/guide/topics/manifest/activity-element.html
    [Activity(
        Label = "@string/app_name",
        Icon = "@drawable/icon",
        Theme = "@style/Theme.Splash",
        MainLauncher = true,
        LaunchMode = LaunchMode.SingleTask,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Locale | ConfigChanges.LayoutDirection
        )
    ]
    public class MainActivity : FormsAppCompatActivity
    {
        private Locale _locale;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.Window.RequestFeature(WindowFeatures.ActionBar);
            base.SetTheme(Resource.Style.AppTheme);

            FormsAppCompatActivity.ToolbarResource = Resource.Layout.Toolbar;
            FormsAppCompatActivity.TabLayoutResource = Resource.Layout.Tabbar;

            base.OnCreate(savedInstanceState);

            // Inicializando FFImageLoading
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: false);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            // Inicializando Xamarin.Essentials
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Inicializando Popups
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            GrialKit.Init(this);

            _locale = Resources.Configuration.Locale;

            ReferenceCalendars();

            LoadApplication(new App());
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            GrialKit.NotifyConfigurationChanged(newConfig);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
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

