using Foundation;
using System.Globalization;
using AppName.Core;

namespace AppName.Core
{
    internal class CultureHelper
    {
        internal static CultureInfo ReadSystemCulture()
        {
            string text = "en";
            if (NSLocale.PreferredLanguages.Length != 0)
            {
                text = IOSToDotnetLanguage(NSLocale.PreferredLanguages[0]);
            }

            try
            {
                return new CultureInfo(text);
            }
            catch (CultureNotFoundException)
            {
                try
                {
                    return new CultureInfo(ToDotnetFallbackLanguage(new PlatformCulture(text)));
                }
                catch (CultureNotFoundException)
                {
                    return new CultureInfo("en");
                }
            }
        }

        private static string IOSToDotnetLanguage(string iOSLanguage)
        {
            string result = iOSLanguage;
            if (iOSLanguage != null)
            {
                if (!(iOSLanguage == "ms-MY") && !(iOSLanguage == "ms-SG"))
                {
                    if (iOSLanguage == "gsw-CH")
                    {
                        result = "de-CH";
                    }
                }
                else
                {
                    result = "ms";
                }
            }
            return result;
        }

        private static string ToDotnetFallbackLanguage(PlatformCulture platCulture)
        {
            string result = platCulture.LanguageCode;
            switch (platCulture.LanguageCode)
            {
                case "pt":
                    result = "pt-PT";
                    break;
                case "gsw":
                    result = "de-CH";
                    break;
            }
            return result;
        }
    }
}
