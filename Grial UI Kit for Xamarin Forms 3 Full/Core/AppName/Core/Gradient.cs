using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace AppName.Core
{
    [ContentProperty("Colors")]
	public abstract class Gradient : BindableObject
	{
		internal static readonly BindableProperty BackgroundGradientForcedHeightProperty = BindableProperty.CreateAttached("__BackgroundGradientForcedHeight", typeof(double), typeof(Gradient), -1.0);

		public ObservableCollection<GradientColor> Colors
		{
			get;
			private set;
		}

		internal static void SetBackgroundGradientForcedHeight(BindableObject view, double value)
		{
			view.SetValue(BackgroundGradientForcedHeightProperty, value);
		}

		internal static double GetBackgroundGradientForcedHeight(BindableObject view)
		{
			return (double)view.GetValue(BackgroundGradientForcedHeightProperty);
		}

		protected Gradient()
		{
			Colors = new ObservableCollection<GradientColor>();
		}
	}
}
