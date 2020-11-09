using Xamarin.Forms;

namespace AppName.Core
{
	[ContentProperty("Action")]
	public class AnimatedTrigger : AnimatedBaseBehavior
	{
		public static readonly BindableProperty ThresholdProperty = BindableProperty.Create("Threshold", typeof(double), typeof(AnimatedTrigger), 0.0, BindingMode.OneWay, null, AnimatedBaseBehavior.OnChanged);

		public static readonly BindableProperty TriggerBeforeThresholdProperty = BindableProperty.Create("TriggerBeforeThreshold", typeof(bool), typeof(AnimatedBoolean), false, BindingMode.OneWay, null, AnimatedBaseBehavior.OnChanged);

		public static readonly BindableProperty ActionProperty = BindableProperty.Create("Action", typeof(ITriggerAction), typeof(AnimatedTrigger), null, BindingMode.OneWay, null, AnimatedBaseBehavior.OnChanged);

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

		public bool TriggerBeforeThreshold
		{
			get
			{
				return (bool)GetValue(TriggerBeforeThresholdProperty);
			}
			set
			{
				SetValue(TriggerBeforeThresholdProperty, value);
			}
		}

		public ITriggerAction Action
		{
			get
			{
				return (ITriggerAction)GetValue(ActionProperty);
			}
			set
			{
				SetValue(ActionProperty, value);
			}
		}

		protected override void OnUpdate()
		{
			bool flag = base.Progress >= Threshold;
			bool flag2 = TriggerBeforeThreshold ? (!flag) : flag;
			if (_oldValue != flag2)
			{
				_oldValue = flag2;
				if (flag2 && Action != null)
				{
					Action.Execute();
				}
			}
		}
	}
}
