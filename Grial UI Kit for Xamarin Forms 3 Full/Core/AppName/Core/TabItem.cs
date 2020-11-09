using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppName.Core
{
	[ContentProperty("Content")]
	public class TabItem : TemplatedView
	{
		public static readonly BindableProperty TapCommandProperty = BindableProperty.Create("TapCommand", typeof(ICommand), typeof(TabItem));

		public static readonly BindableProperty ContentProperty = BindableProperty.Create("Content", typeof(View), typeof(TabItem));

		public static readonly BindableProperty HeaderProperty = BindableProperty.Create("Header", typeof(View), typeof(TabItem));

		private static readonly BindablePropertyKey IsSelectedPropertyKey = BindableProperty.CreateReadOnly("IsSelected", typeof(bool), typeof(TabItem), false, BindingMode.OneWayToSource, null, delegate(BindableObject s, object o, object n)
		{
			((TabItem)s).UpdateCurrentIcons();
		});

		public static readonly BindableProperty IsSelectedProperty = IsSelectedPropertyKey.BindableProperty;

		public static readonly BindableProperty IconTextFontFamilyProperty = BindableProperty.Create("IconTextFontFamily", typeof(string), typeof(TabItem));

		public static readonly BindableProperty IconTextFontSizeProperty = BindableProperty.Create("IconTextFontSize", typeof(double), typeof(TabItem), Device.GetNamedSize(NamedSize.Medium, typeof(Label)));

		public static readonly BindableProperty FontSizeProperty = BindableProperty.Create("FontSize", typeof(double), typeof(TabItem), Device.GetNamedSize(NamedSize.Medium, typeof(Label)));

		public static readonly BindableProperty IconTextProperty = BindableProperty.Create("IconText", typeof(string), typeof(TabItem), null, BindingMode.OneWay, null, OnSelectionDependentPropertyChanged);

		public static readonly BindableProperty IconTextSelectedProperty = BindableProperty.Create("IconTextSelected", typeof(string), typeof(TabItem), null, BindingMode.OneWay, null, OnSelectionDependentPropertyChanged);

		private static readonly BindablePropertyKey CurrentIconTextPropertyKey = BindableProperty.CreateReadOnly("CurrentIconText", typeof(string), typeof(TabItem), null);

		public static readonly BindableProperty CurrentIconTextProperty = CurrentIconTextPropertyKey.BindableProperty;

		public static readonly BindableProperty IconTextColorProperty = BindableProperty.Create("IconTextColor", typeof(Color), typeof(TabItem), Color.Black, BindingMode.OneWay, null, OnSelectionDependentPropertyChanged);

		public static readonly BindableProperty IconTextColorSelectedProperty = BindableProperty.Create("IconTextColorSelected", typeof(Color), typeof(TabItem), Color.Default, BindingMode.OneWay, null, OnSelectionDependentPropertyChanged);

		private static readonly BindablePropertyKey CurrentIconTextColorPropertyKey = BindableProperty.CreateReadOnly("CurrentIconTextColor", typeof(Color), typeof(TabItem), Color.Black);

		public static readonly BindableProperty CurrentIconTextColorProperty = CurrentIconTextColorPropertyKey.BindableProperty;

		public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(TabItem));

		public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Color), typeof(TabItem), Color.Black, BindingMode.OneWay, null, OnSelectionDependentPropertyChanged);

		public static readonly BindableProperty TextColorSelectedProperty = BindableProperty.Create("TextColorSelected", typeof(Color), typeof(TabItem), Color.Default, BindingMode.OneWay, null, OnSelectionDependentPropertyChanged);

		private static readonly BindablePropertyKey CurrentTextColorPropertyKey = BindableProperty.CreateReadOnly("CurrentTextColor", typeof(Color), typeof(TabItem), Color.Black);

		public static readonly BindableProperty CurrentTextColorProperty = CurrentTextColorPropertyKey.BindableProperty;

		public static readonly BindableProperty IconImageProperty = BindableProperty.Create("IconImage", typeof(ImageSource), typeof(TabItem), null, BindingMode.OneWay, null, OnSelectionDependentPropertyChanged);

		public static readonly BindableProperty IconImageSelectedProperty = BindableProperty.Create("IconImageSelected", typeof(ImageSource), typeof(TabItem), null, BindingMode.OneWay, null, OnSelectionDependentPropertyChanged);

		private static readonly BindablePropertyKey CurrentIconImagePropertyKey = BindableProperty.CreateReadOnly("CurrentIconImage", typeof(ImageSource), typeof(TabItem), null);

		public static readonly BindableProperty CurrentIconImageProperty = CurrentIconImagePropertyKey.BindableProperty;

		public static readonly BindableProperty BadgeTextProperty = BindableProperty.Create("BadgeText", typeof(string), typeof(TabItem));

		public static readonly BindableProperty BadgeTextColorProperty = BindableProperty.Create("BadgeTextColor", typeof(Color), typeof(TabItem), Color.Black);

		public static readonly BindableProperty BadgeBackgroundColorProperty = BindableProperty.Create("BadgeBackgroundColor", typeof(Color), typeof(TabItem), Color.Red);

		public ICommand TapCommand
		{
			get
			{
				return (ICommand)GetValue(TapCommandProperty);
			}
			set
			{
				SetValue(TapCommandProperty, value);
			}
		}

		public View Content
		{
			get
			{
				return (View)GetValue(ContentProperty);
			}
			set
			{
				SetValue(ContentProperty, value);
			}
		}

		public View Header
		{
			get
			{
				return (View)GetValue(HeaderProperty);
			}
			set
			{
				SetValue(HeaderProperty, value);
			}
		}

		public bool IsSelected
		{
			get
			{
				return (bool)GetValue(IsSelectedProperty);
			}
			internal set
			{
				SetValue(IsSelectedPropertyKey, value);
			}
		}

		public string IconTextFontFamily
		{
			get
			{
				return (string)GetValue(IconTextFontFamilyProperty);
			}
			set
			{
				SetValue(IconTextFontFamilyProperty, value);
			}
		}

		public double IconTextFontSize
		{
			get
			{
				return (double)GetValue(IconTextFontSizeProperty);
			}
			set
			{
				SetValue(IconTextFontSizeProperty, value);
			}
		}

		public string IconText
		{
			get
			{
				return (string)GetValue(IconTextProperty);
			}
			set
			{
				SetValue(IconTextProperty, value);
			}
		}

		public string IconTextSelected
		{
			get
			{
				return (string)GetValue(IconTextSelectedProperty);
			}
			set
			{
				SetValue(IconTextSelectedProperty, value);
			}
		}

		public string CurrentIconText
		{
			get
			{
				return (string)GetValue(CurrentIconTextProperty);
			}
			private set
			{
				SetValue(CurrentIconTextPropertyKey, value);
			}
		}

		public Color IconTextColor
		{
			get
			{
				return (Color)GetValue(IconTextColorProperty);
			}
			set
			{
				SetValue(IconTextColorProperty, value);
			}
		}

		public Color IconTextColorSelected
		{
			get
			{
				return (Color)GetValue(IconTextColorSelectedProperty);
			}
			set
			{
				SetValue(IconTextColorSelectedProperty, value);
			}
		}

		public Color CurrentIconTextColor
		{
			get
			{
				return (Color)GetValue(CurrentIconTextColorProperty);
			}
			private set
			{
				SetValue(CurrentIconTextColorPropertyKey, value);
			}
		}

		public string Text
		{
			get
			{
				return (string)GetValue(TextProperty);
			}
			set
			{
				SetValue(TextProperty, value);
			}
		}

		public double FontSize
		{
			get
			{
				return (double)GetValue(FontSizeProperty);
			}
			set
			{
				SetValue(FontSizeProperty, value);
			}
		}

		public Color TextColor
		{
			get
			{
				return (Color)GetValue(TextColorProperty);
			}
			set
			{
				SetValue(TextColorProperty, value);
			}
		}

		public Color TextColorSelected
		{
			get
			{
				return (Color)GetValue(TextColorSelectedProperty);
			}
			set
			{
				SetValue(TextColorSelectedProperty, value);
			}
		}

		public Color CurrentTextColor
		{
			get
			{
				return (Color)GetValue(CurrentTextColorProperty);
			}
			private set
			{
				SetValue(CurrentTextColorPropertyKey, value);
			}
		}

		[TypeConverter(typeof(ImageSourceConverter))]
		public ImageSource IconImage
		{
			get
			{
				return (ImageSource)GetValue(IconImageProperty);
			}
			set
			{
				SetValue(IconImageProperty, value);
			}
		}

		[TypeConverter(typeof(ImageSourceConverter))]
		public ImageSource IconImageSelected
		{
			get
			{
				return (ImageSource)GetValue(IconImageSelectedProperty);
			}
			set
			{
				SetValue(IconImageSelectedProperty, value);
			}
		}

		public ImageSource CurrentIconImage
		{
			get
			{
				return (ImageSource)GetValue(CurrentIconImageProperty);
			}
			private set
			{
				SetValue(CurrentIconImagePropertyKey, value);
			}
		}

		public string BadgeText
		{
			get
			{
				return (string)GetValue(BadgeTextProperty);
			}
			set
			{
				SetValue(BadgeTextProperty, value);
			}
		}

		public Color BadgeTextColor
		{
			get
			{
				return (Color)GetValue(BadgeTextColorProperty);
			}
			set
			{
				SetValue(BadgeTextColorProperty, value);
			}
		}

		public Color BadgeBackgroundColor
		{
			get
			{
				return (Color)GetValue(BadgeBackgroundColorProperty);
			}
			set
			{
				SetValue(BadgeBackgroundColorProperty, value);
			}
		}

		public event EventHandler Tapped;

		internal void RaiseTapped()
		{
			this.Tapped?.Invoke(this, EventArgs.Empty);
			TapCommand?.Execute(null);
		}

		private static void OnSelectionDependentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((TabItem)bindable).UpdateCurrentIcons();
		}

		private void UpdateCurrentIcons()
		{
			CurrentIconText = ((!IsSelected || IconTextSelected == null) ? IconText : IconTextSelected);
			CurrentIconImage = ((!IsSelected || IconImageSelected == null) ? IconImage : IconImageSelected);
			CurrentIconTextColor = ((!IsSelected || IconTextColorSelected == Color.Default) ? IconTextColor : IconTextColorSelected);
			CurrentTextColor = ((!IsSelected || TextColorSelected == Color.Default) ? TextColor : TextColorSelected);
		}
	}
}
