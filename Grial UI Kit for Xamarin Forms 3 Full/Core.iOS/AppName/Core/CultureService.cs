using Foundation;
using System.Globalization;
using System.Threading;

namespace AppName.Core
{
    internal class CultureService : ICultureService
    {
        private static CultureService _Instance;

        private CultureInfo _ci;

        private CultureInfo _deviceCI;

        public static CultureService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new CultureService();
                }
                return _Instance;
            }
        }

        public CultureInfo CurrentCulture
        {
            get
            {
                return _ci;
            }
            private set
            {
                if (value != _ci && (value == null || !value.Equals(_ci)))
                {
                    CultureInfo ci = _ci;
                    _ci = value;
                    Thread.CurrentThread.CurrentCulture = value;
                    Thread.CurrentThread.CurrentUICulture = value;
                    this.CurrentCultureChanged?.Invoke(this, new CultureChangeEventArgs(ci, value));
                }
            }
        }

        public event CultureChangedEventHandler CurrentCultureChanged;

        private CultureService()
        {
            _deviceCI = (_ci = CultureHelper.ReadSystemCulture());
            NSNotificationCenter.DefaultCenter.AddObserver(NSLocale.CurrentLocaleDidChangeNotification, OnLocaleChanged);
        }

        public void SimulateCultureChange(CultureInfo ci)
        {
            CurrentCulture = ci;
        }

        private void OnLocaleChanged(NSNotification notification)
        {
            CultureInfo cultureInfo = CultureHelper.ReadSystemCulture();
            if (!cultureInfo.Equals(_deviceCI))
            {
                _deviceCI = cultureInfo;
                CurrentCulture = cultureInfo;
            }
        }
    }
}
