using Android.Content.Res;

namespace AppName.Core
{
    public class CultureServiceLocator : ICultureServiceLocator
    {
        public ICultureService Service => CultureService.Instance;

        public static void NotifyLocaleChanged(Configuration newConfig)
        {
            CultureService.Instance.OnLocaleChanged(newConfig);
        }
    }
}
