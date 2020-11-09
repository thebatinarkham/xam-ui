namespace AppName.Core
{
	public class AnimatedInputTransparent : AnimatedBoolean
	{
		protected override void SetPropertyValue(bool value)
		{
			base.Target.InputTransparent = value;
		}
	}
}
