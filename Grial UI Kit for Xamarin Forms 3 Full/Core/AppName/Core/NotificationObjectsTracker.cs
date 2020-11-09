using System;
using System.Collections.Generic;

namespace AppName.Core
{
	internal class NotificationObjectsTracker
	{
		internal interface INotifier
		{
			void Notify();
		}

		private const int DefaultInitialNotificationListSize = 2000;

		private const int DefaultAutoCleanupTriggerCount = 1000;

		private const int DefaultPeriodBetweenAutoCleanups = 5000;

		private List<WeakReference<INotifier>> _notificationObjects;

		private int _initialNotificationListSize;

		private DateTime _lastCleanup;

		internal int AutoCleanupTriggerCount
		{
			get;
			set;
		}

		internal int InitialNotificationListSize
		{
			get
			{
				return _initialNotificationListSize;
			}
			set
			{
				if (_notificationObjects != null)
				{
					throw new InvalidOperationException("Size must be changed before the collection has been initialized.");
				}
				_initialNotificationListSize = value;
			}
		}

		internal TimeSpan PeriodBetweenAutoCleanups
		{
			get;
			set;
		}

		private List<WeakReference<INotifier>> NotificationObjects
		{
			get
			{
				if (_notificationObjects == null)
				{
					_notificationObjects = new List<WeakReference<INotifier>>(InitialNotificationListSize);
				}
				return _notificationObjects;
			}
		}

		public NotificationObjectsTracker()
		{
			AutoCleanupTriggerCount = 1000;
			InitialNotificationListSize = 2000;
			PeriodBetweenAutoCleanups = TimeSpan.FromMilliseconds(5000.0);
			_lastCleanup = DateTime.MinValue;
		}

		public void Add(INotifier notifier)
		{
			Cleanup();
			NotificationObjects.Add(new WeakReference<INotifier>(notifier));
		}

		public void NotifyAlive()
		{
			int num = 0;
			while (num < NotificationObjects.Count)
			{
				if (NotificationObjects[num].TryGetTarget(out INotifier target))
				{
					target.Notify();
					num++;
				}
				else
				{
					NotificationObjects.RemoveAt(num);
				}
			}
		}

		public void ForceCleanup()
		{
			try
			{
				int num = 0;
				while (num < NotificationObjects.Count)
				{
					if (!NotificationObjects[num].TryGetTarget(out INotifier _))
					{
						NotificationObjects.RemoveAt(num);
					}
					else
					{
						num++;
					}
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				_lastCleanup = DateTime.Now;
			}
		}

		private void Cleanup()
		{
			if (NotificationObjects.Count > AutoCleanupTriggerCount && DateTime.Now - _lastCleanup > PeriodBetweenAutoCleanups)
			{
				ForceCleanup();
			}
		}
	}
}
