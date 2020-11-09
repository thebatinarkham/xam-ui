namespace AppName.Core
{
	public class AnimatedScale : AnimatedDouble
	{
		protected override void SetPropertyValue(double value)
		{
			base.Target.Scale = value;
		}

		protected override double GetDefaultStart()
		{
			return 1.0;
		}

		protected override double GetDefaultEnd()
		{
			return 2.0;
		}
	}
}
