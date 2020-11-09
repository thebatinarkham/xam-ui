using Xamarin.Forms;

namespace AppName.Core
{
	public abstract class AnimatedBoolean : AnimatedBaseBehavior
	{
		public static readonly BindableProperty ThresholdProperty = BindableProperty.Create("Threshold", typeof(double), typeof(AnimatedBoolean), 0.0, BindingMode.OneWay, null, AnimatedBaseBehavior.OnChanged);

		public static readonly BindableProperty ValueBeforeThresholdProperty = BindableProperty.Create("ValueBeforeThreshold", typeof(bool), typeof(AnimatedBoolean), false, BindingMode.OneWay, null, AnimatedBaseBehavior.OnChanged);

		private bool? _oldValue;

		public double Threshold
		{
			get
			{
				return (double)GetValue(ThresholdProperty);
			}
			set
			{
				SetValue(ThresholdProperty, value);
			}
		}

		public bool ValueBeforeThreshold
		{
			get
			{
				return (bool)GetValue(ValueBeforeThresholdProperty);
			}
			set
			{
				SetValue(ValueBeforeThresholdProperty, value);
			}
		}

		protected override void OnUpdate()
		{
			bool flag = base.Progress >= Threshold;
			bool flag2 = ValueBeforeThreshold ? (!flag) : flag;
			if (_oldValue != flag2)
			{
				_oldValue = flag2;
				SetPropertyValue(flag2);
			}
		}

		protected abstract void SetPropertyValue(bool value);
	}
}
