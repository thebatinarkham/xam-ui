using Android.Content.Res;

namespace AppName.Core
{
    public class LayoutDirectionServiceLocator : ILayoutDirectionServiceLocator
    {
        public ILayoutDirectionService Service => LayoutDirectionService.Instance;

        public static void NotifyLayoutDirectionChanged(Configuration newConfig)
        {
            LayoutDirectionService.Instance.OnNotifyLayoutDirectionChanged(newConfig);
        }
    }
}
