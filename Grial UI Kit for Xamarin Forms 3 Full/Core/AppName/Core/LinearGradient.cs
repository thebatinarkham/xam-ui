using Xamarin.Forms;

namespace AppName.Core
{
	public class LinearGradient : Gradient
	{
		public static readonly BindableProperty AngleProperty = BindableProperty.Create("Angle", typeof(double), typeof(GradientColor), 0.0);

		public double Angle
		{
			get
			{
				return (double)GetValue(AngleProperty);
			}
			set
			{
				SetValue(AngleProperty, value);
			}
		}
	}
}
