namespace AppName.Core
{
	public class AnimatedTranslationY : AnimatedDouble
	{
		protected override void SetPropertyValue(double value)
		{
			base.Target.TranslationY = value;
		}

		protected override double GetDefaultEnd()
		{
			return base.Progress ?? base.Target.Height;
		}
	}
}
