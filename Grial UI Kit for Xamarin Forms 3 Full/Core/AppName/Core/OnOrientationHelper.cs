using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace AppName.Core
{
    internal static class OnOrientationHelper
    {
        private class MagnitudeBinder<T> : NotificationObjectsTracker.INotifier, INotifyPropertyChanged
        {
            private readonly IOnOrientationValues<T> _values;

            private readonly T _defaultValue;

            public T Magnitude
            {
                get
                {
                    T result = _defaultValue;
                    //return result;
                    if (_values.HasDefault)
                    {
                        result = _values.Default;
                    }
                    if (ResponsiveHelper.Orientation.IsPortrait)
                    {
                        if (_values.HasPortrait)
                        {
                            result = _values.Portrait;
                        }
                        if (Device.Idiom == TargetIdiom.Phone)
                        {
                            if (_values.HasPortraitPhone)
                            {
                                result = _values.PortraitPhone;
                            }
                        }
                        else if (Device.Idiom == TargetIdiom.Tablet)
                        {
                            if (_values.HasPortraitTablet)
                            {
                                result = _values.PortraitTablet;
                            }
                        }
                        else if (Device.Idiom == TargetIdiom.Desktop && _values.HasPortraitDesktop)
                        {
                            result = _values.PortraitDesktop;
                        }
                    }
                    else
                    {
                        if (_values.HasLandscape)
                        {
                            result = _values.Landscape;
                        }
                        if (Device.Idiom == TargetIdiom.Phone)
                        {
                            if (_values.HasLandscapePhone)
                            {
                                result = _values.LandscapePhone;
                            }
                        }
                        else if (Device.Idiom == TargetIdiom.Tablet)
                        {
                            if (_values.HasLandscapeTablet)
                            {
                                result = _values.LandscapeTablet;
                            }
                        }
                        else if (Device.Idiom == TargetIdiom.Desktop && _values.HasLandscapeDesktop)
                        {
                            result = _values.LandscapeDesktop;
                        }
                    }
                    return result;
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            internal MagnitudeBinder(IOnOrientationValues<T> values, T defaultValue)
            {
                _values = values;
                _defaultValue = defaultValue;
            }

            public void Notify()
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Magnitude"));
                }
            }
        }

        private static NotificationObjectsTracker _notificationTracker;

        internal static object GetBindingSource<T>(IOnOrientationValues<T> values)
        {
            if (_notificationTracker == null)
            {
                Initialize();
            }
            return GetBindingSource(values, default(T));
        }

        internal static object GetBindingSource<T>(IOnOrientationValues<T> values, T defaultValue)
        {
            if (_notificationTracker == null)
            {
                Initialize();
                if (_notificationTracker == null)
                {
                    return null;
                }
            }
            MagnitudeBinder<T> magnitudeBinder = new MagnitudeBinder<T>(values, defaultValue);
            _notificationTracker.Add(magnitudeBinder);
            return magnitudeBinder;
        }

        private static void Initialize()
        {
            _notificationTracker = new NotificationObjectsTracker();
            if (ResponsiveHelper.Orientation != null)
            {
                ResponsiveHelper.Orientation.OrientationChanged += OnOrientationChanged;
            }
        }

        private static void OnOrientationChanged(object sender, EventArgs e)
        {
            _notificationTracker.NotifyAlive();
        }
    }
}
