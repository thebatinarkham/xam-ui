using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public enum NotificationType
    {
        Confirmation = 0,
        Notification, 
        Success, 
        Error, 
        Warning
    }

    public class NotificationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var notificationType = (NotificationType)value;

            if (targetType == typeof(Color))
            {
                return GetColor(notificationType);
            }
            else if (targetType == typeof(string))
            {
                return GetIcon(notificationType);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private string GetIcon(NotificationType notificationType)
        {
            switch (notificationType)
            {
                case NotificationType.Confirmation:
                    return GrialIconsFont.Check;

                case NotificationType.Notification:
                    return GrialIconsFont.Bell;

                case NotificationType.Success:
                    return GrialIconsFont.Check;

                case NotificationType.Warning:
                    return GrialIconsFont.AlertTriangle;

                default: // Error
                    return GrialIconsFont.Close;
            }
        }

        private Color GetColor(NotificationType notificationType)
        {
            string resourceName;

            switch (notificationType)
            {
                case NotificationType.Confirmation:
                    resourceName = "OkColor";
                    break;

                case NotificationType.Notification:
                    resourceName = "NotificationColor";
                    break;

                case NotificationType.Success:
                    resourceName = "OkColor";
                    break;

                case NotificationType.Warning:
                    resourceName = "WarningColor";
                    break;

                default: // Error
                    resourceName = "ErrorColor";
                    break;
            }

            return ResourceHelper.FindResource<Color>(resourceName);
        }
    }
}