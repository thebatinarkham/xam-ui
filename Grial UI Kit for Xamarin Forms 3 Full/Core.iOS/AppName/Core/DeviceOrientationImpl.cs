using Foundation;
using System;
using UIKit;
using AppName.Core;

namespace AppName.Core
{
    internal class DeviceOrientationImpl : IDeviceOrientation
    {
        private static DeviceOrientationImpl _instance;

        public static DeviceOrientationImpl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DeviceOrientationImpl();
                }
                return _instance;
            }
        }

        public DeviceOrientation Orientation
        {
            get
            {
                UIInterfaceOrientation statusBarOrientation = UIApplication.SharedApplication.StatusBarOrientation;
                if (statusBarOrientation != UIInterfaceOrientation.Portrait && statusBarOrientation != UIInterfaceOrientation.PortraitUpsideDown)
                {
                    return DeviceOrientation.Landscape;
                }
                return DeviceOrientation.Portrait;
            }
        }

        public bool IsPortrait => Orientation == DeviceOrientation.Portrait;

        public bool IsLandscape => Orientation == DeviceOrientation.Landscape;

        public event EventHandler OrientationChanged;

        private DeviceOrientationImpl()
        {
            NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.DidChangeStatusBarOrientationNotification, DeviceOrientationDidChange);
            UIDevice.CurrentDevice.BeginGeneratingDeviceOrientationNotifications();
        }

        private void DeviceOrientationDidChange(NSNotification notification)
        {
            UIDeviceOrientation orientation = UIDevice.CurrentDevice.Orientation;
            if (this.OrientationChanged != null)
            {
                this.OrientationChanged(null, null);
            }
        }
    }
}
