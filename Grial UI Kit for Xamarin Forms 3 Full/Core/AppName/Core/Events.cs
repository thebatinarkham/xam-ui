using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppName.Core
{
	internal static class Events
	{
		private class LongPressEffect : RoutingEffect
		{
			public LongPressEffect()
				: base("AppName.LongPressEffect")
			{
			}
		}

		public static readonly BindableProperty LongPressCommandProperty = BindableProperty.Create("LongPressCommand", typeof(ICommand), typeof(Events), null, BindingMode.OneWay, null, OnLongPressCommandChanged);

		public static readonly BindableProperty LongPressCommandParameterProperty = BindableProperty.Create("LongPressCommandParameter", typeof(object), typeof(Events));

		public static ICommand GetLongPressCommand(BindableObject element)
		{
			return (ICommand)element.GetValue(LongPressCommandProperty);
		}

		public static void SetLongPressCommand(BindableObject element, ICommand command)
		{
			element.SetValue(LongPressCommandProperty, command);
		}

		public static object GetLongPressCommandParameter(BindableObject element)
		{
			return element.GetValue(LongPressCommandParameterProperty);
		}

		public static void SetLongPressCommandParameter(BindableObject element, object parameter)
		{
			element.SetValue(LongPressCommandParameterProperty, parameter);
		}

		private static void OnLongPressCommandChanged(BindableObject bindable, object oldValue, object newValue)
		{
			Element element = bindable as Element;
			if (element == null)
			{
				return;
			}
			if (newValue != null)
			{
				element.Effects.Add(new LongPressEffect());
				return;
			}
			Effect effect = element.Effects.FirstOrDefault((Effect x) => x is LongPressEffect);
			if (effect != null)
			{
				element.Effects.Remove(effect);
			}
		}
	}
}
