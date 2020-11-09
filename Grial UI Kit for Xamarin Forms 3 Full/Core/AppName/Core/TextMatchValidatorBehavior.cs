using System;
using Xamarin.Forms;

namespace AppName.Core
{
	public class TextMatchValidatorBehavior : Behavior<Entry>
	{
		private Entry _entry;

		public static readonly BindableProperty TextToMatchProperty = BindableProperty.Create("TextToMatch", typeof(string), typeof(TextMatchValidatorBehavior), string.Empty, BindingMode.OneWay, null, delegate(BindableObject bindable, object oldValue, object newValue)
		{
			OnTextToMatchChanged(bindable, (string)oldValue, (string)newValue);
		});

		private static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(TextMatchValidatorBehavior), true);

		public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

		public string TextToMatch
		{
			get
			{
				return (string)GetValue(TextToMatchProperty);
			}
			set
			{
				SetValue(TextToMatchProperty, value);
			}
		}

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

		private static void OnTextToMatchChanged(BindableObject bo, string oldValue, string newValue)
		{
			TextMatchValidatorBehavior textMatchValidatorBehavior = (TextMatchValidatorBehavior)bo;
			if (textMatchValidatorBehavior._entry != null)
			{
				textMatchValidatorBehavior.IsValid = CheckIfValid(textMatchValidatorBehavior._entry, newValue);
			}
		}

		protected override void OnAttachedTo(Entry bindable)
		{
			if (_entry != null)
			{
				throw new InvalidOperationException($"Usage of the behavior {GetType().Name} associated to a style are not supported.");
			}
			_entry = bindable;
			bindable.TextChanged += OnTextChanged;
		}

		protected override void OnDetachingFrom(Entry bindable)
		{
			_entry = null;
			bindable.TextChanged -= OnTextChanged;
		}

		private void OnTextChanged(object sender, TextChangedEventArgs e)
		{
			IsValid = CheckIfValid((Entry)sender, TextToMatch);
		}

		private static bool CheckIfValid(Entry entry, string textoToMatch)
		{
			if (entry != null)
			{
				string a = entry.Text ?? string.Empty;
				string b = textoToMatch ?? string.Empty;
				return string.Equals(a, b);
			}
			return false;
		}
	}
}
