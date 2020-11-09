namespace AppName.Core
{
	internal class DeviceOrientationLocator : IDeviceOrientationServiceLocator
	{
		public IDeviceOrientation Service => DeviceOrientationImpl.Instance;
	}
}
