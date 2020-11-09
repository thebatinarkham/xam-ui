using System;
using Xamarin.Forms;

namespace AppName.Core
{
    internal static class ResponsiveHelper
	{
		private static IDeviceOrientation _Orientation;

		public static IDeviceOrientation Orientation
		{
			get
			{
				if (_Orientation == null)
				{
					IDeviceOrientationServiceLocator deviceOrientationServiceLocator = DependencyService.Get<IDeviceOrientationServiceLocator>();
					if (deviceOrientationServiceLocator == null)
					{
						//return null;
						throw new InvalidOperationException(string.Format(SR.MissingDependencyService, "IDeviceOrientationServiceLocator"));
					}
					_Orientation = deviceOrientationServiceLocator.Service;
				}
				return _Orientation;
			}
		}
	}
}
