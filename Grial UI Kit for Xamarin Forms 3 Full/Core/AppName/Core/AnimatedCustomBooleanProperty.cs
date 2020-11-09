using Xamarin.Forms;

namespace AppName.Core
{
	public class AnimatedCustomBooleanProperty : AnimatedBoolean
	{
		public static readonly BindableProperty TargetPropertyProperty = BindableProperty.Create("TargetProperty", typeof(BindableProperty), typeof(AnimatedCustomBooleanProperty), null, BindingMode.OneWay, null, AnimatedBaseBehavior.OnChanged);

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

		protected override void SetPropertyValue(bool value)
		{
			base.Target.SetValue(TargetProperty, value);
		}
	}
}
