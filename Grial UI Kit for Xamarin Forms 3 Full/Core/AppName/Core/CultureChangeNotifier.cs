using System;
using Xamarin.Forms;

namespace AppName.Core
{
    public class CultureChangeNotifier : NotificationObjectsTracker.INotifier
	{
		private static readonly NotificationObjectsTracker _notificationTracker = new NotificationObjectsTracker();

		private static CultureChangeEventArgs _LastEventArgs;

		private static ICultureService _CultureService;

		public event CultureChangedEventHandler CultureChanged;

		public CultureChangeNotifier()
		{
			if (_CultureService == null)
			{
				ICultureServiceLocator cultureServiceLocator = DependencyService.Get<ICultureServiceLocator>();
				if (cultureServiceLocator == null)
				{
					//return;
					throw new InvalidOperationException(string.Format(SR.MissingDependencyService, "ICultureServiceLocator"));
				}
				_CultureService = cultureServiceLocator.Service;
				_CultureService.CurrentCultureChanged += OnCurrentCultureChanged;
			}
			_notificationTracker.Add(this);
		}

		private static void OnCurrentCultureChanged(object sender, CultureChangeEventArgs e)
		{
			_LastEventArgs = e;
			_notificationTracker.NotifyAlive();
		}

		public void Notify()
		{
			this.CultureChanged?.Invoke(this, _LastEventArgs);
		}
	}
}
