using Xamarin.Forms;

namespace AppName.Core
{
	public class TableViewProperties
	{
		public static readonly BindableProperty HeaderFooterTextColorProperty = BindableProperty.CreateAttached("HeaderFooterTextColor", typeof(Color), typeof(TableViewProperties), Color.Default);

		public static readonly BindableProperty SelectedBackgroundColorProperty = BindableProperty.CreateAttached("SelectedBackgroundColor", typeof(Color), typeof(TableViewProperties), Color.Default);

		public static Color GetHeaderFooterTextColor(BindableObject bo)
		{
			return (Color)bo.GetValue(HeaderFooterTextColorProperty);
		}

		public static void SetHeaderFooterTextColor(BindableObject bo, Color value)
		{
			bo.SetValue(HeaderFooterTextColorProperty, value);
		}

		public static Color GetSelectedBackgroundColor(BindableObject bo)
		{
			return (Color)bo.GetValue(SelectedBackgroundColorProperty);
		}

		public static void SetSelectedBackgroundColor(BindableObject bo, Color value)
		{
			bo.SetValue(SelectedBackgroundColorProperty, value);
		}
	}
}
