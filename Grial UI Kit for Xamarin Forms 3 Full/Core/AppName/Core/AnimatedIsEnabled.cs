namespace AppName.Core
{
	public class AnimatedIsEnabled : AnimatedBoolean
	{
		protected override void SetPropertyValue(bool value)
		{
			base.Target.IsEnabled = value;
		}
	}
}
