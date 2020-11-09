using System;
using System.Collections.Generic;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AppName.Core
{
    public class ThemeColorsBase
    {
        private readonly Dictionary<string, Color> _colors;

        public ThemeColorsBase(Dictionary<string, Color> colors)
        {
            if (colors == null)
            {
                throw new ArgumentNullException("colors");
            }
            _colors = colors;
        }

        public UIColor GetColor(string name)
        {
            if (!_colors.TryGetValue(name, out Color value))
            {
                throw new ArgumentOutOfRangeException(name, "The provided ThemeColors instance does not define color for '" + name + "'.");
            }
            return value.ToUIColor();
        }
    }
}
