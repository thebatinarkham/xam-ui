using Xamarin.Forms;

namespace AppName.Core
{
	public class SearchBarProperties
	{
		public static readonly BindableProperty FieldBackgroundColorProperty = BindableProperty.CreateAttached("FieldBackgroundColor", typeof(Color), typeof(SearchBarProperties), Color.Default);

		public static readonly BindableProperty IconColorProperty = BindableProperty.CreateAttached("IconColor", typeof(Color), typeof(SearchBarProperties), Color.Default);

		public static readonly BindableProperty BorderWidthProperty = BindableProperty.CreateAttached("BorderWidth", typeof(float), typeof(SearchBarProperties), 0f);

		public static readonly BindableProperty BorderColorProperty = BindableProperty.CreateAttached("BorderColor", typeof(Color), typeof(SearchBarProperties), Color.Default);

		public static Color GetFieldBackgroundColor(BindableObject bo)
		{
			return (Color)bo.GetValue(FieldBackgroundColorProperty);
		}

		public static void SetFieldBackgroundColor(BindableObject bo, Color value)
		{
			bo.SetValue(FieldBackgroundColorProperty, value);
		}

		public static Color GetIconColor(BindableObject bo)
		{
			return (Color)bo.GetValue(IconColorProperty);
		}

		public static void SetIconColor(BindableObject bo, Color value)
		{
			bo.SetValue(IconColorProperty, value);
		}

		public static float GetBorderWidth(BindableObject bo)
		{
			return (float)bo.GetValue(BorderWidthProperty);
		}

		public static void SetBorderWidth(BindableObject bo, float value)
		{
			bo.SetValue(BorderWidthProperty, value);
		}

		public static Color GetBorderColor(BindableObject bo)
		{
			return (Color)bo.GetValue(BorderColorProperty);
		}

		public static void SetBorderColor(BindableObject bo, Color value)
		{
			bo.SetValue(BorderColorProperty, value);
		}
	}
}
