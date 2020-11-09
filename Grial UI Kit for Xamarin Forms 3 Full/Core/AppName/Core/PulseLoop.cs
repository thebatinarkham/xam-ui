using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppName.Core
{
	public class PulseLoop : LoopBehaviorBase
	{
		protected override async Task Animate(VisualElement target, State state)
		{
			uint duration = (uint)((base.LoopDuration > 0) ? (base.LoopDuration / 2) : 1000);
			while (state.IsRunning)
			{
				await target.ScaleTo(1.4, duration, Easing.Linear);
				if (state.IsRunning)
				{
					await target.ScaleTo(1.0, duration, Easing.Linear);
				}
			}
		}
	}
}
