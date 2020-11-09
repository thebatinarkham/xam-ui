using Xamarin.Forms;

namespace AppName.Core
{
    public class GrialKit
    {
        public static void Init(ThemeColorsBase colors)
        {
            if (colors != null)
            {
                Appearance.Configure(colors);
            }
            DependencyService.Register<MirrorService>();
            DependencyService.Register<DeviceOrientationLocator>();
        }
    }
}
