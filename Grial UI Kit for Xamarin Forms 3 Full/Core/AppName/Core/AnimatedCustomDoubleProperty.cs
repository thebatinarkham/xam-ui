using Xamarin.Forms;

namespace AppName.Core
{
	public class AnimatedCustomDoubleProperty : AnimatedDouble
	{
		public static readonly BindableProperty TargetPropertyProperty = BindableProperty.Create("TargetProperty", typeof(BindableProperty), typeof(AnimatedCustomDoubleProperty), null, BindingMode.OneWay, null, AnimatedBaseBehavior.OnChanged);

		public BindableProperty TargetProperty
		{
			get
			{
				return (BindableProperty)GetValue(TargetPropertyProperty);
			}
			set
			{
				SetValue(TargetPropertyProperty, value);
			}
		}

		protected override void SetPropertyValue(double value)
		{
			base.Target.SetValue(TargetProperty, value);
		}
	}
}
