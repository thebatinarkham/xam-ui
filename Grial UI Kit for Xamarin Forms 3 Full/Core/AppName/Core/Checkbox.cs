using System;
using Xamarin.Forms;

namespace AppName.Core
{
    public class Checkbox : ContentView
	{
		private View _checked;

		private View _unchecked;

		public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create("IsChecked", typeof(bool), typeof(Checkbox), false, BindingMode.OneWay, null, OnCheckedChanged);

		public static BindableProperty CheckedBackgroundColorProperty = BindableProperty.Create("CheckedBackgroundColor", typeof(Color), typeof(Checkbox), Color.White);

		public static BindableProperty CheckedBorderColorProperty = BindableProperty.Create("CheckedBorderColor", typeof(Color), typeof(Checkbox), Color.Black);

		public static BindableProperty UncheckedBackgroundColorProperty = BindableProperty.Create("UncheckedBackgroundColor", typeof(Color), typeof(Checkbox), Color.White);

		public static BindableProperty UncheckedBorderColorProperty = BindableProperty.Create("UncheckedBorderColor", typeof(Color), typeof(Checkbox), Color.Black);

		public static BindableProperty IconColorProperty = BindableProperty.Create("IconColor", typeof(Color), typeof(Checkbox), Color.Black);

		public static BindableProperty IconFontSizeProperty = BindableProperty.Create("IconFontSize", typeof(double), typeof(Checkbox), 10.0);

		public bool IsChecked
		{
			get
			{
				return (bool)GetValue(IsCheckedProperty);
			}
			set
			{
				SetValue(IsCheckedProperty, value);
			}
		}

		public Color CheckedBackgroundColor
		{
			get
			{
				return (Color)GetValue(CheckedBackgroundColorProperty);
			}
			set
			{
				SetValue(CheckedBackgroundColorProperty, value);
			}
		}

		public Color CheckedBorderColor
		{
			get
			{
				return (Color)GetValue(CheckedBorderColorProperty);
			}
			set
			{
				SetValue(CheckedBorderColorProperty, value);
			}
		}

		public Color UncheckedBackgroundColor
		{
			get
			{
				return (Color)GetValue(UncheckedBackgroundColorProperty);
			}
			set
			{
				SetValue(UncheckedBackgroundColorProperty, value);
			}
		}

		public Color UncheckedBorderColor
		{
			get
			{
				return (Color)GetValue(UncheckedBorderColorProperty);
			}
			set
			{
				SetValue(UncheckedBorderColorProperty, value);
			}
		}

		public Color IconColor
		{
			get
			{
				return (Color)GetValue(IconColorProperty);
			}
			set
			{
				SetValue(IconColorProperty, value);
			}
		}

		public double IconFontSize
		{
			get
			{
				return (double)GetValue(IconFontSizeProperty);
			}
			set
			{
				SetValue(IconFontSizeProperty, value);
			}
		}

		public event EventHandler<IsCheckedChangedEventArgs> IsCheckedChanged;

		public Checkbox()
		{
			base.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command((Action<object>)delegate
				{
					IsChecked = !IsChecked;
				})
			});
		}

		private static void OnCheckedChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((Checkbox)bindable).Update(notify: true);
		}

		protected override void OnChildAdded(Element child)
		{
			base.OnChildAdded(child);
			_checked = child.FindByName<View>("Checked");
			_unchecked = child.FindByName<View>("Unchecked");
			Update(notify: false);
		}

		private void Update(bool notify)
		{
			if (_checked != null)
			{
				_checked.IsVisible = IsChecked;
			}
			if (_unchecked != null)
			{
				_unchecked.IsVisible = !IsChecked;
			}
			if (notify)
			{
				this.IsCheckedChanged?.Invoke(this, new IsCheckedChangedEventArgs
				{
					IsChecked = IsChecked
				});
			}
		}
	}
}
