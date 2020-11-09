using Android.Content.Res;
using Java.Lang;
using System;
using System.Globalization;

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
            private set
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
            _deviceNativeDirection = (_direction = ToDirection(Java.Util.Locale.Default));
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

        public void OnNotifyLayoutDirectionChanged(Configuration newConfig)
        {
            LayoutDirection layoutDirection = ToDirection(newConfig.Locale);
            if (layoutDirection != _deviceNativeDirection)
            {
                _deviceNativeDirection = layoutDirection;
                LayoutDirection = layoutDirection;
            }
        }

        private LayoutDirection ToDirection(Java.Util.Locale locale)
        {
            int directionality = Character.GetDirectionality(locale.DisplayName[0]);
            if (directionality == 1 || directionality == 2)
            {
                return LayoutDirection.Rtl;
            }
            return LayoutDirection.Ltr;
        }
    }
}
