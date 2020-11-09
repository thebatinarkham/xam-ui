using Foundation;
using System;
using System.Globalization;
using UIKit;

namespace AppName.Core
{
    internal class LayoutDirectionService : ILayoutDirectionService
    {
        private static LayoutDirectionService _Instance;

        private LayoutDirection _direction;

        private LayoutDirection _deviceNativeDirection;

        public static LayoutDirectionService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new LayoutDirectionService();
                }
                return _Instance;
            }
        }

        public LayoutDirection DeviceNativeDirection => _deviceNativeDirection;

        public LayoutDirection LayoutDirection
        {
            get
            {
                return _direction;
            }
            set
            {
                if (_direction != value)
                {
                    _direction = value;
                    this.LayoutDirectionChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler LayoutDirectionChanged;

        private LayoutDirectionService()
        {
            _deviceNativeDirection = (_direction = ReadSystemLayoutDirection());
            NSNotificationCenter.DefaultCenter.AddObserver(NSLocale.CurrentLocaleDidChangeNotification, OnLocaleChanged);
        }

        public LayoutDirection ReadCultureLayoutDirection(CultureInfo ci)
        {
            if (!ci.TextInfo.IsRightToLeft)
            {
                return LayoutDirection.Ltr;
            }
            return LayoutDirection.Rtl;
        }

        public void SimulateLayoutDirectionChange(LayoutDirection direction)
        {
            LayoutDirection = direction;
        }

        private void OnLocaleChanged(NSNotification obj)
        {
            LayoutDirection layoutDirection = ReadSystemLayoutDirection();
            if (layoutDirection != _deviceNativeDirection)
            {
                _deviceNativeDirection = layoutDirection;
                LayoutDirection = layoutDirection;
            }
        }

        private LayoutDirection ReadSystemLayoutDirection()
        {
            if (UIView.GetUserInterfaceLayoutDirection(UISemanticContentAttribute.Unspecified) != UIUserInterfaceLayoutDirection.RightToLeft)
            {
                return LayoutDirection.Ltr;
            }
            return LayoutDirection.Rtl;
        }
    }
}
