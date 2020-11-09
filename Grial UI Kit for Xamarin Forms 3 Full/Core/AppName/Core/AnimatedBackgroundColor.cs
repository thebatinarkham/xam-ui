using Xamarin.Forms;

namespace AppName.Core
{
	public class AnimatedBackgroundColor : AnimatedColor
	{
		protected override void SetPropertyValue(Color value)
		{
			base.Target.BackgroundColor = value;
		}
	}
}
