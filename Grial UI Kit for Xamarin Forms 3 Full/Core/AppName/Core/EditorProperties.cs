using Xamarin.Forms;

namespace AppName.Core
{
	public class EditorProperties
	{
		public static readonly BindableProperty PlaceholderProperty = BindableProperty.CreateAttached("Placeholder", typeof(string), typeof(EditorProperties), "");

		public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.CreateAttached("PlaceholderColor", typeof(Color), typeof(EditorProperties), Color.Default);

		public static readonly BindableProperty BorderWidthProperty = BindableProperty.CreateAttached("BorderWidth", typeof(float), typeof(EditorProperties), 0.5f);

		public static readonly BindableProperty HorizontalPaddingProperty = BindableProperty.CreateAttached("HorizontalPadding", typeof(float), typeof(EditorProperties), 0f);

		public static readonly BindableProperty BorderCornerRadiusProperty = BindableProperty.CreateAttached("BorderCornerRadius", typeof(float), typeof(EditorProperties), 5f);

		public static readonly BindableProperty BorderColorProperty = BindableProperty.CreateAttached("BorderColor", typeof(Color), typeof(EditorProperties), Color.Default);

		public static readonly BindableProperty BorderStyleProperty = BindableProperty.CreateAttached("BorderStyle", typeof(BorderStyle), typeof(EditorProperties), BorderStyle.Default);

		public static string GetPlaceholder(BindableObject bo)
		{
			return (string)bo.GetValue(PlaceholderProperty);
		}

		public static void SetPlaceholder(BindableObject bo, string value)
		{
			bo.SetValue(PlaceholderProperty, value);
		}

		public static Color GetPlaceholderColor(BindableObject bo)
		{
			return (Color)bo.GetValue(PlaceholderColorProperty);
		}

		public static void SetPlaceholderColor(BindableObject bo, Color value)
		{
			bo.SetValue(PlaceholderColorProperty, value);
		}

		public static float GetBorderWidth(BindableObject bo)
		{
			return (float)bo.GetValue(BorderWidthProperty);
		}

		public static void SetBorderWidth(BindableObject bo, float value)
		{
			bo.SetValue(BorderWidthProperty, value);
		}

		public static float GetHorizontalPadding(BindableObject bo)
		{
			return (float)bo.GetValue(HorizontalPaddingProperty);
		}

		public static void SetHorizontalPadding(BindableObject bo, float value)
		{
			bo.SetValue(HorizontalPaddingProperty, value);
		}

		public static float GetBorderCornerRadius(BindableObject bo)
		{
			return (float)bo.GetValue(BorderCornerRadiusProperty);
		}

		public static void SetBorderCornerRadius(BindableObject bo, float value)
		{
			bo.SetValue(BorderCornerRadiusProperty, value);
		}

		public static Color GetBorderColor(BindableObject bo)
		{
			return (Color)bo.GetValue(BorderColorProperty);
		}

		public static void SetBorderColor(BindableObject bo, Color value)
		{
			bo.SetValue(BorderColorProperty, value);
		}

		public static BorderStyle GetBorderStyle(BindableObject bo)
		{
			return (BorderStyle)bo.GetValue(BorderStyleProperty);
		}

		public static void SetBorderStyle(BindableObject bo, BorderStyle value)
		{
			bo.SetValue(BorderStyleProperty, value);
		}
	}
}
