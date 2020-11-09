using Xamarin.Forms;

namespace AppName.Core
{
	public class RadialGradient : Gradient
	{
		public static readonly BindableProperty CenterXProperty = BindableProperty.Create("CenterX", typeof(double), typeof(GradientColor), 0.5);

		public static readonly BindableProperty CenterYProperty = BindableProperty.Create("CenterY", typeof(double), typeof(GradientColor), 0.5);

		public static readonly BindableProperty RadiusXProperty = BindableProperty.Create("RadiusX", typeof(double?), typeof(GradientColor));

		public static readonly BindableProperty RadiusYProperty = BindableProperty.Create("RadiusY", typeof(double?), typeof(GradientColor));

		public static readonly BindableProperty RadiusProperty = BindableProperty.Create("Radius", typeof(double?), typeof(GradientColor));

		public static readonly BindableProperty RadiusTypeProperty = BindableProperty.Create("RadiusType", typeof(RadiusType), typeof(GradientColor), RadiusType.Proportional);

		public double CenterX
		{
			get
			{
				return (double)GetValue(CenterXProperty);
			}
			set
			{
				SetValue(CenterXProperty, value);
			}
		}

		public double CenterY
		{
			get
			{
				return (double)GetValue(CenterYProperty);
			}
			set
			{
				SetValue(CenterYProperty, value);
			}
		}

		public double? RadiusX
		{
			get
			{
				return (double?)GetValue(RadiusXProperty);
			}
			set
			{
				SetValue(RadiusXProperty, value);
			}
		}

		public double? RadiusY
		{
			get
			{
				return (double?)GetValue(RadiusYProperty);
			}
			set
			{
				SetValue(RadiusYProperty, value);
			}
		}

		public double? Radius
		{
			get
			{
				return (double?)GetValue(RadiusProperty);
			}
			set
			{
				SetValue(RadiusProperty, value);
			}
		}

		public RadiusType RadiusType
		{
			get
			{
				return (RadiusType)GetValue(RadiusTypeProperty);
			}
			set
			{
				SetValue(RadiusTypeProperty, value);
			}
		}
	}
}
