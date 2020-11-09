using Android.App;
using System;

namespace AppName.Core
{
    internal class DeviceOrientationImpl : IDeviceOrientation
    {
        private static DeviceOrientationImpl _instance;

        private DeviceOrientation _lastKnownOrientation;

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

        internal Activity Activity
        {
            get;
            set;
        }

        public DeviceOrientation Orientation
        {
            get
            {
                if (Activity == null)
                {
                    return DeviceOrientation.Portrait;
                }
                switch (Activity.Resources.Configuration.Orientation)
                {
                    case Android.Content.Res.Orientation.Portrait:
                        return DeviceOrientation.Portrait;
                    case Android.Content.Res.Orientation.Landscape:
                        return DeviceOrientation.Landscape;
                    default:
                        return DeviceOrientation.Unknown;
                }
            }
        }

        public bool IsPortrait => Orientation == DeviceOrientation.Portrait;

        public bool IsLandscape => Orientation == DeviceOrientation.Landscape;

        public event EventHandler OrientationChanged;

        private DeviceOrientationImpl()
        {
        }

        internal void ProcessOrientationChange()
        {
            DeviceOrientation orientation = Orientation;
            if (_lastKnownOrientation != orientation)
            {
                _lastKnownOrientation = orientation;
                if (this.OrientationChanged != null)
                {
                    this.OrientationChanged(null, null);
                }
            }
        }
    }
}
