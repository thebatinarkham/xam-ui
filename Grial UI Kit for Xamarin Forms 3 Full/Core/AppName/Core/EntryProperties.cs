using System.ComponentModel;
using Xamarin.Forms;

namespace AppName.Core
{
	public class EntryProperties
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.CreateAttached("PlaceholderColor", typeof(Color), typeof(EntryProperties), Color.Default);

		public static readonly BindableProperty BorderStyleProperty = BindableProperty.CreateAttached("BorderStyle", typeof(BorderStyle), typeof(EntryProperties), BorderStyle.Default);

		public static readonly BindableProperty BorderWidthProperty = BindableProperty.CreateAttached("BorderWidth", typeof(float), typeof(EntryProperties), 1f);

		public static readonly BindableProperty HorizontalPaddingProperty = BindableProperty.CreateAttached("HorizontalPadding", typeof(float), typeof(EntryProperties), 0f);

		public static readonly BindableProperty BorderCornerRadiusProperty = BindableProperty.CreateAttached("BorderCornerRadius", typeof(float), typeof(EntryProperties), 10f);

		public static readonly BindableProperty BorderColorProperty = BindableProperty.CreateAttached("BorderColor", typeof(Color), typeof(EntryProperties), Color.Default);

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
	}
}
