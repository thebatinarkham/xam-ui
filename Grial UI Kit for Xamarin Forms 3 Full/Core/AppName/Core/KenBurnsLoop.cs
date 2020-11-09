using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppName.Core
{
	public class KenBurnsLoop : LoopBehaviorBase
	{
		protected override void InitializeTarget(VisualElement target)
		{
			target.TranslationY = -10.0;
			target.Scale = 1.2;
		}

		protected override async Task Animate(VisualElement target, State state)
		{
			uint duration = (uint)((base.LoopDuration > 0) ? (base.LoopDuration / 2) : 10500);
			while (state.IsRunning)
			{
				await Task.WhenAll<bool>(target.ScaleTo(1.05, duration, Easing.SinInOut), target.TranslateTo(0.0, 0.0, duration, Easing.SinInOut));
				if (state.IsRunning)
				{
					await Task.WhenAll<bool>(target.TranslateTo(0.0, -10.0, duration, Easing.SinInOut), target.ScaleTo(1.15, duration, Easing.SinInOut));
				}
			}
		}
	}
}
