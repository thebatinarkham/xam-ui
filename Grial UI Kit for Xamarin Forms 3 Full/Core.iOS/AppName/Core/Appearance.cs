using UIKit;

namespace AppName.Core
{
    internal static class Appearance
    {
        private const string AccentColorKey = "AccentColor";

        private const string TextColorKey = "InverseTextColor";

        public static void Configure(ThemeColorsBase colors)
        {
            UIColor color = colors.GetColor("AccentColor");
            UIProgressView.Appearance.ProgressTintColor = (color);
            UISlider.Appearance.MinimumTrackTintColor = (color);
            UISlider.Appearance.MaximumTrackTintColor = (color);
            UISlider.Appearance.ThumbTintColor = (color);
            UISwitch.Appearance.OnTintColor = (color);
            UITableViewHeaderFooterView.Appearance.TintColor = (color);
            UITableView.Appearance.SectionIndexBackgroundColor = (color);
            UITableView.Appearance.SeparatorColor = (color);
            UITabBar.Appearance.TintColor = (color);
            UITextField.Appearance.TintColor = (color);
            UIStepper.Appearance.TintColor = (color);
        }
    }
}
