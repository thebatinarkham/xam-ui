using Java.Util;
using System.Globalization;

namespace AppName.Core
{
    internal class CultureHelper
    {
        internal static CultureInfo ReadSystemCulture(Java.Util.Locale locale)
        {
            string text = "en";
            text = AndroidToDotnetLanguage(locale.ToString().Replace("_", "-"));

            try
            {
                return new CultureInfo(text);
            }
            catch (CultureNotFoundException)
            {
                try
                {
                    string name = ToDotnetFallbackLanguage(new PlatformCulture(text));
                    return new CultureInfo(name);
                }
                catch (CultureNotFoundException)
                {
                    return new CultureInfo("en");
                }
            }
        }

        private static string AndroidToDotnetLanguage(string androidLanguage)
        {
            string result = androidLanguage;
            switch (androidLanguage)
            {
                case "iw":
                    result = "he";
                    break;
                case "iw-IL":
                    result = "he-IL";
                    break;
                case "in":
                    result = "id";
                    break;
                case "ms-BN":
                case "ms-MY":
                case "ms-SG":
                    result = "ms";
                    break;
                case "in-ID":
                    result = "id-ID";
                    break;
                case "gsw-CH":
                    result = "de-CH";
                    break;
            }
            return result;
        }

        private static string ToDotnetFallbackLanguage(PlatformCulture platCulture)
        {
            string result = platCulture.LanguageCode;
            string languageCode = platCulture.LanguageCode;
            if (languageCode != null && languageCode == "gsw")
            {
                result = "de-CH";
            }
            return result;
        }
    }
}
