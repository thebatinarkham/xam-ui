using Xamarin.Forms;

namespace AppName.Core
{
	public abstract class AnimatedColor : AnimatedIntervalBaseBehavior
	{
		public static readonly BindableProperty StartProperty = BindableProperty.Create("Start", typeof(Color), typeof(AnimatedColor), Color.Default, BindingMode.OneWay, null, AnimatedBaseBehavior.OnChanged);

		public static readonly BindableProperty EndProperty = BindableProperty.Create("End", typeof(Color), typeof(AnimatedColor), Color.Default, BindingMode.OneWay, null, AnimatedBaseBehavior.OnChanged);

		public Color Start
		{
			get
			{
				return (Color)GetValue(StartProperty);
			}
			set
			{
				SetValue(StartProperty, value);
			}
		}

		public Color End
		{
			get
			{
				return (Color)GetValue(EndProperty);
			}
			set
			{
				SetValue(EndProperty, value);
			}
		}

		protected override void OnUpdate()
		{
			if (!(Start == Color.Default) || !(End == Color.Default))
			{
				if (Start == Color.Default)
				{
					SetPropertyValue(End);
					return;
				}
				if (End == Color.Default)
				{
					SetPropertyValue(Start);
					return;
				}
				Color propertyValue = Interpolate(Start, End, (double p) => AnimColor(p, Start, End));
				SetPropertyValue(propertyValue);
			}
		}

		protected abstract void SetPropertyValue(Color value);

		private static Color AnimColor(double t, Color fromColor, Color toColor)
		{
			return Color.FromRgba(fromColor.R + t * (toColor.R - fromColor.R), fromColor.G + t * (toColor.G - fromColor.G), fromColor.B + t * (toColor.B - fromColor.B), fromColor.A + t * (toColor.A - fromColor.A));
		}
	}
}
