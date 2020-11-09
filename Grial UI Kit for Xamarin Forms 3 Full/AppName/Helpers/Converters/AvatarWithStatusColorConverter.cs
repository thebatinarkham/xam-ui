using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public class AvatarWithStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string resourceName;

            var stringValue = value != null ? value.ToString() : string.Empty;

            switch (stringValue)
            {
                case nameof(AvatarStatus.Online):
                    resourceName = "OkColor";
                    break;

                case nameof(AvatarStatus.Busy):
                    resourceName = "ErrorColor";
                    break;

                case nameof(AvatarStatus.Away):
                    resourceName = "WarningColor";
                    break;

                default: // Offline
                    resourceName = "DisabledColor";
                    break;
            }

            return ResourceHelper.FindResource<Color>(resourceName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
