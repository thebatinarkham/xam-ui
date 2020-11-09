namespace AppName.Core
{
	public class AnimatedOpacity : AnimatedDouble
	{
		protected override void SetPropertyValue(double value)
		{
			base.Target.Opacity = value;
		}
	}
}
