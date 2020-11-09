using Xamarin.Forms;

namespace AppName.Core
{
	public abstract class AnimatedDouble : AnimatedIntervalBaseBehavior
	{
		public static readonly BindableProperty MultiplyValueProperty = BindableProperty.Create("MultiplyValue", typeof(double), typeof(AnimatedDouble), 1.0, BindingMode.OneWay, null, AnimatedBaseBehavior.OnChanged);

		public static readonly BindableProperty EndAddValueProperty = BindableProperty.Create("EndAddValue", typeof(double), typeof(AnimatedDouble), 0.0, BindingMode.OneWay, null, AnimatedBaseBehavior.OnChanged);

		public static readonly BindableProperty StartAddValueProperty = BindableProperty.Create("StartAddValue", typeof(double), typeof(AnimatedDouble), 0.0, BindingMode.OneWay, null, AnimatedBaseBehavior.OnChanged);

		public static readonly BindableProperty StartProperty = BindableProperty.Create("Start", typeof(double?), typeof(AnimatedDouble), null, BindingMode.OneWay, null, AnimatedBaseBehavior.OnChanged);

		public static readonly BindableProperty EndProperty = BindableProperty.Create("End", typeof(double?), typeof(AnimatedDouble), null, BindingMode.OneWay, null, AnimatedBaseBehavior.OnChanged);

		public double MultiplyValue
		{
			get
			{
				return (double)GetValue(MultiplyValueProperty);
			}
			set
			{
				SetValue(MultiplyValueProperty, value);
			}
		}

		public double EndAddValue
		{
			get
			{
				return (double)GetValue(EndAddValueProperty);
			}
			set
			{
				SetValue(EndAddValueProperty, value);
			}
		}

		public double StartAddValue
		{
			get
			{
				return (double)GetValue(StartAddValueProperty);
			}
			set
			{
				SetValue(StartAddValueProperty, value);
			}
		}

		public double? Start
		{
			get
			{
				return (double?)GetValue(StartProperty);
			}
			set
			{
				SetValue(StartProperty, value);
			}
		}

		public double? End
		{
			get
			{
				return (double?)GetValue(EndProperty);
			}
			set
			{
				SetValue(EndProperty, value);
			}
		}

		protected override void OnUpdate()
		{
			double start = (Start ?? GetDefaultStart()) + StartAddValue;
			double end = (End ?? GetDefaultEnd()) + EndAddValue;
			double num = Interpolate(start, end, (double p) => start + p * (end - start));
			if (base.IsSymmetric && base.Progress < 0.0)
			{
				num = 0.0 - num;
			}
			SetPropertyValue(num * MultiplyValue);
		}

		protected virtual double GetDefaultStart()
		{
			return 0.0;
		}

		protected virtual double GetDefaultEnd()
		{
			return 1.0;
		}

		protected abstract void SetPropertyValue(double value);
	}
}
