using System.ComponentModel;
using Xamarin.Forms;

namespace AppName.Core
{
	public static class PickerProperties
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.CreateAttached("PlaceholderColor", typeof(Color), typeof(PickerProperties), Color.Default);

		public static readonly BindableProperty BorderStyleProperty = BindableProperty.CreateAttached("BorderStyle", typeof(BorderStyle), typeof(PickerProperties), BorderStyle.Default);

		public static readonly BindableProperty BorderWidthProperty = BindableProperty.CreateAttached("BorderWidth", typeof(float), typeof(PickerProperties), 1f);

		public static readonly BindableProperty HorizontalPaddingProperty = BindableProperty.CreateAttached("HorizontalPadding", typeof(float), typeof(PickerProperties), 0f);

		public static readonly BindableProperty BorderCornerRadiusProperty = BindableProperty.CreateAttached("BorderCornerRadius", typeof(float), typeof(PickerProperties), 10f);

		public static readonly BindableProperty BorderColorProperty = BindableProperty.CreateAttached("BorderColor", typeof(Color), typeof(PickerProperties), Color.Default);

		public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.CreateAttached("HorizontalTextAlignment", typeof(TextAlignment), typeof(PickerProperties), TextAlignment.Start);

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static Color GetPlaceholderColor(BindableObject bo)
		{
			return (Color)bo.GetValue(PlaceholderColorProperty);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void SetPlaceholderColor(BindableObject bo, Color value)
		{
			bo.SetValue(PlaceholderColorProperty, value);
		}

		public static BorderStyle GetBorderStyle(BindableObject bo)
		{
			return (BorderStyle)bo.GetValue(BorderStyleProperty);
		}

		public static void SetBorderStyle(BindableObject bo, BorderStyle value)
		{
			bo.SetValue(BorderStyleProperty, value);
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

		public static TextAlignment GetHorizontalTextAlignment(BindableObject bo)
		{
			return (TextAlignment)bo.GetValue(HorizontalTextAlignmentProperty);
		}

		public static void SetHorizontalTextAlignment(BindableObject bo, TextAlignment value)
		{
			bo.SetValue(HorizontalTextAlignmentProperty, value);
		}
	}
}
