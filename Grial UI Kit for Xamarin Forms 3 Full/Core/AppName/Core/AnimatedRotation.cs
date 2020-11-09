namespace AppName.Core
{
	public class AnimatedRotation : AnimatedDouble
	{
		protected override void SetPropertyValue(double value)
		{
			base.Target.Rotation = value;
		}

		protected override double GetDefaultEnd()
		{
			return 360.0;
		}
	}
}
