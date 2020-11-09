namespace AppName.Core
{
	public class AnimatedRotationX : AnimatedDouble
	{
		protected override void SetPropertyValue(double value)
		{
			base.Target.RotationX = value;
		}

		protected override double GetDefaultEnd()
		{
			return 360.0;
		}
	}
}
