namespace AppName.Core
{
	public class AnimatedTranslationX : AnimatedDouble
	{
		protected override void SetPropertyValue(double value)
		{
			base.Target.TranslationX = value;
		}

		protected override double GetDefaultEnd()
		{
			return base.Progress ?? base.Target.Width;
		}
	}
}
