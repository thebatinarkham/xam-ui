using System;
using Xamarin.Forms;

namespace AppName.Core
{
    internal static class LayoutDirectionExtensionStaticNotifier
    {
        private static readonly NotificationObjectsTracker _notificationTracker = new NotificationObjectsTracker();

        private static ILayoutDirectionService _LayoutDirectionService
        {
            get;
            set;
        }

        public static ILayoutDirectionService LayoutDirectionService => _LayoutDirectionService;

        public static void Initialize()
        {
            if (_LayoutDirectionService != null)
            {
                return;
            }
            ILayoutDirectionServiceLocator layoutDirectionServiceLocator = DependencyService.Get<ILayoutDirectionServiceLocator>();
            if (layoutDirectionServiceLocator == null)
            {
                throw new InvalidOperationException(string.Format(SR.MissingDependencyService, "ILayoutDirectionServiceLocator"));
            }
            else
            {
                _LayoutDirectionService = layoutDirectionServiceLocator.Service;
                _LayoutDirectionService.LayoutDirectionChanged += OnLayoutDirectionChanged;
            }
        }

        public static void Add(NotificationObjectsTracker.INotifier notifier)
        {
            _notificationTracker.Add(notifier);
        }

        private static void OnLayoutDirectionChanged(object sender, EventArgs e)
        {
            _notificationTracker.NotifyAlive();
        }
    }
}
