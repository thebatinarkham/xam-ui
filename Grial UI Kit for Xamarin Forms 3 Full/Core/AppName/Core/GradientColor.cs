using Xamarin.Forms;

namespace AppName.Core
{
	public class GradientColor : BindableObject
	{
		public static readonly BindableProperty ColorProperty = BindableProperty.Create("Color", typeof(Color), typeof(GradientColor), Color.Transparent);

		public static readonly BindableProperty OpacityProperty = BindableProperty.Create("Opacity", typeof(double), typeof(GradientColor), 1.0);

		public static readonly BindableProperty PositionProperty = BindableProperty.Create("Position", typeof(double?), typeof(GradientColor));

		public Color Color
		{
			get
			{
				return (Color)GetValue(ColorProperty);
			}
			set
			{
				SetValue(ColorProperty, value);
			}
		}

		public double Opacity
		{
			get
			{
				return (double)GetValue(OpacityProperty);
			}
			set
			{
				SetValue(OpacityProperty, value);
			}
		}

		public double? Position
		{
			get
			{
				return (double?)GetValue(PositionProperty);
			}
			set
			{
				SetValue(PositionProperty, value);
			}
		}

		public Color FinalColor
		{
			get
			{
				if (Opacity >= 1.0)
				{
					return Color;
				}
				double num = Opacity;
				if (Opacity < 0.0)
				{
					num = 0.0;
				}
				return new Color(Color.R, Color.G, Color.B, Color.A * num);
			}
		}
	}
}
