using Xamarin.Forms;

namespace AppName.Core
{
	public static class ColorCache
	{
		private static Color? _accentColor;

		private static Color? _listViewItemTextColor;

		private static Color? _listViewSelectedItemBackgroundColor;

		public static Color AccentColor
		{
			get
			{
				if (!_accentColor.HasValue || !_accentColor.HasValue || _accentColor.Value == Color.Default)
				{
					object value = null;
					bool flag = false;
					if (Application.Current != null && Application.Current.Resources != null)
					{
						flag = Application.Current.Resources.TryGetValue("AccentColor", out value);
					}
					if (flag && value != null)
					{
						_accentColor = ResolveResourceColor(value);
					}
					else
					{
						_accentColor = Color.Default;
					}
				}
				if (_accentColor.HasValue)
				{
					return _accentColor.Value;
				}
				return Color.Default;
			}
		}

		public static Color ListViewItemTextColor
		{
			get
			{
				if (!_listViewItemTextColor.HasValue || !_listViewItemTextColor.HasValue || _listViewItemTextColor.Value == Color.Default)
				{
					object value = null;
					bool flag = false;
					if (Application.Current != null && Application.Current.Resources != null)
					{
						flag = Application.Current.Resources.TryGetValue("ListViewItemTextColor", out value);
					}
					if (flag && value != null)
					{
						_listViewItemTextColor = ResolveResourceColor(value);
					}
					else
					{
						_listViewItemTextColor = Color.Default;
					}
				}
				if (_listViewItemTextColor.HasValue)
				{
					return _listViewItemTextColor.Value;
				}
				return Color.Default;
			}
		}

		public static Color ListViewSelectedItemBackgroundColor
		{
			get
			{
				if (!_listViewSelectedItemBackgroundColor.HasValue || !_listViewSelectedItemBackgroundColor.HasValue || _listViewSelectedItemBackgroundColor.Value == Color.Default)
				{
					object value = null;
					bool flag = false;
					if (Application.Current != null && Application.Current.Resources != null)
					{
						flag = Application.Current.Resources.TryGetValue("ListViewSelectedItemBackgroundColor", out value);
					}
					if (flag && value != null)
					{
						_listViewSelectedItemBackgroundColor = ResolveResourceColor(value);
					}
				}
				if (_listViewSelectedItemBackgroundColor.HasValue)
				{
					return _listViewSelectedItemBackgroundColor.Value;
				}
				return AccentColor;
			}
		}

		public static void InvalidateCache()
		{
			_accentColor = null;
			_listViewItemTextColor = null;
			_listViewSelectedItemBackgroundColor = null;
		}

		public static Color GetDisabledColorFor(Color color)
		{
			return color.AddLuminosity((0.0 - color.Luminosity) / 2.0);
		}

		private static Color ResolveResourceColor(object resource)
		{
			if (resource is Color)
			{
				return (Color)resource;
			}
			OnPlatform<Color> onPlatform = resource as OnPlatform<Color>;
			if (onPlatform != null)
			{
				return onPlatform;
			}
			return Color.Default;
		}
	}
}
