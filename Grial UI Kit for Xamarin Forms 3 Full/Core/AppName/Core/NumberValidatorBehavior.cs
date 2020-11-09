using Xamarin.Forms;

namespace AppName.Core
{
	public class NumberValidatorBehavior : Behavior<Entry>
	{
		private static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(NumberValidatorBehavior), false);

		public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

		public bool IsValid
		{
			get
			{
				return (bool)GetValue(IsValidProperty);
			}
			private set
			{
				SetValue(IsValidPropertyKey, value);
			}
		}

		protected override void OnAttachedTo(Entry bindable)
		{
			bindable.TextChanged += OnTextChanged;
		}

		protected override void OnDetachingFrom(Entry bindable)
		{
			bindable.TextChanged -= OnTextChanged;
		}

		private void OnTextChanged(object sender, TextChangedEventArgs e)
		{
			IsValid = (!string.IsNullOrEmpty(e.NewTextValue) && double.TryParse(e.NewTextValue, out double _));
		}
	}
}
