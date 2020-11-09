using Android.App;
using Android.Content.Res;
using Xamarin.Forms;

namespace AppName.Core
{
    public class GrialKit
    {
        public static void Init(Activity activity)
        {
            InitDeviceStuff(activity);
            DependencyService.Register<MirrorService>();
            DependencyService.Register<DeviceOrientationLocator>();
        }

        public static void NotifyConfigurationChanged(Configuration newConfig)
        {
            DeviceOrientationLocator.NotifyOrientationChanged();
            CultureServiceLocator.NotifyLocaleChanged(newConfig);
            LayoutDirectionServiceLocator.NotifyLayoutDirectionChanged(newConfig);
        }

        private static void InitDeviceStuff(Activity activity)
        {
            if (activity != null)
            {
                DeviceOrientationLocator.Init(activity);
            }
        }
    }
}
