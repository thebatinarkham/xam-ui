namespace AppName.Core
{
	public class AnimatedRotationY : AnimatedDouble
	{
		protected override void SetPropertyValue(double value)
		{
			base.Target.RotationY = value;
		}

		protected override double GetDefaultEnd()
		{
			return 360.0;
		}
	}
}
