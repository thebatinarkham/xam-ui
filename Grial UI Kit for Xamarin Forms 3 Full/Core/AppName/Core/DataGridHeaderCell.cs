using System.Windows.Input;
using Xamarin.Forms;

namespace AppName.Core
{
	public class DataGridHeaderCell : ContentView
	{
		public static readonly BindableProperty HeaderBackgroundColorProperty = BindableProperty.Create("HeaderBackgroundColor", typeof(Color), typeof(DataGridHeaderCell), Color.Default);

		public static readonly BindableProperty HeaderPaddingProperty = BindableProperty.Create("HeaderPadding", typeof(Thickness), typeof(DataGridHeaderCell), new Thickness(4.0));

		public static readonly BindableProperty HeaderTextProperty = BindableProperty.Create("HeaderText", typeof(string), typeof(DataGridHeaderCell), string.Empty);

		public static readonly BindableProperty HeaderTextColorProperty = BindableProperty.Create("HeaderTextColor", typeof(Color), typeof(DataGridHeaderCell), Color.Black);

		public static readonly BindableProperty HeaderTextAlignmentProperty = BindableProperty.Create("TextAlignment", typeof(TextAlignment), typeof(DataGridHeaderCell), TextAlignment.Start);

		public static readonly BindableProperty HeaderFontSizeProperty = BindableProperty.Create("HeaderFontSize", typeof(double), typeof(DataGridHeaderCell), Device.GetNamedSize(NamedSize.Medium, typeof(Label)));

		public static readonly BindableProperty HeaderFontFamilyProperty = BindableProperty.Create("HeaderFontFamily", typeof(string), typeof(DataGridHeaderCell));

		public static readonly BindableProperty HeaderFontAttributesProperty = BindableProperty.Create("HeaderFontAttributes", typeof(FontAttributes), typeof(DataGridHeaderCell), FontAttributes.None);

		public static readonly BindableProperty HeaderVerticalAlignmentProperty = BindableProperty.Create("HeaderVerticalAlignment", typeof(DataGridVerticalAlignment), typeof(DataGridHeaderCell), DataGridVerticalAlignment.Center);

		public static readonly BindableProperty IsSortableProperty = BindableProperty.Create("IsSortable", typeof(bool), typeof(DataGridHeaderCell), false);

		public static readonly BindablePropertyKey SortPropertyKey = BindableProperty.CreateReadOnly("Sort", typeof(DataGridSortType), typeof(DataGridHeaderCell), DataGridSortType.Unsorted);

		public static readonly BindableProperty SortProperty = SortPropertyKey.BindableProperty;

		public static readonly BindableProperty SortCommandProperty = BindableProperty.Create("SortCommand", typeof(ICommand), typeof(DataGridHeaderCell));

		public static readonly BindableProperty UnsortedIconProperty = BindableProperty.Create("UnsortedIcon", typeof(ImageSource), typeof(DataGridHeaderCell));

		public static readonly BindableProperty AscendingSortIconProperty = BindableProperty.Create("AscendingSortIcon", typeof(ImageSource), typeof(DataGridHeaderCell));

		public static readonly BindableProperty DescendingSortIconProperty = BindableProperty.Create("DescendingSortIcon", typeof(ImageSource), typeof(DataGridHeaderCell));

		public static readonly BindableProperty SortIconsFontFamilyProperty = BindableProperty.Create("SortIconsFontFamily", typeof(string), typeof(DataGridHeaderCell));

		public static readonly BindableProperty SortIconsFontSizeProperty = BindableProperty.Create("SortIconsFontSize", typeof(double), typeof(DataGridHeaderCell), Device.GetNamedSize(NamedSize.Medium, typeof(Label)));

		public static readonly BindableProperty UnsortedIconTextProperty = BindableProperty.Create("UnsortedIconText", typeof(string), typeof(DataGridHeaderCell));

		public static readonly BindableProperty AscendingSortIconTextProperty = BindableProperty.Create("AscendingSortIconText", typeof(string), typeof(DataGridHeaderCell));

		public static readonly BindableProperty DescendingSortIconTextProperty = BindableProperty.Create("DescendingSortIconText", typeof(string), typeof(DataGridHeaderCell));

		public static readonly BindableProperty CurrentSortIconTextProperty = BindableProperty.Create("CurrentSortIconText", typeof(string), typeof(DataGridHeaderCell));

		public static readonly BindableProperty CurrentSortIconProperty = BindableProperty.Create("CurrentSortIcon", typeof(ImageSource), typeof(DataGridHeaderCell));

		public Color HeaderBackgroundColor
		{
			get
			{
				return (Color)GetValue(HeaderBackgroundColorProperty);
			}
			set
			{
				SetValue(HeaderBackgroundColorProperty, value);
			}
		}

		public Thickness HeaderPadding
		{
			get
			{
				return (Thickness)GetValue(HeaderPaddingProperty);
			}
			set
			{
				SetValue(HeaderPaddingProperty, value);
			}
		}

		public string HeaderText
		{
			get
			{
				return (string)GetValue(HeaderTextProperty);
			}
			set
			{
				SetValue(HeaderTextProperty, value);
			}
		}

		public Color HeaderTextColor
		{
			get
			{
				return (Color)GetValue(HeaderTextColorProperty);
			}
			set
			{
				SetValue(HeaderTextColorProperty, value);
			}
		}

		public TextAlignment HeaderTextAlignment
		{
			get
			{
				return (TextAlignment)GetValue(HeaderTextAlignmentProperty);
			}
			set
			{
				SetValue(HeaderTextAlignmentProperty, value);
			}
		}

		public double HeaderFontSize
		{
			get
			{
				return (double)GetValue(HeaderFontSizeProperty);
			}
			set
			{
				SetValue(HeaderFontSizeProperty, value);
			}
		}

		public string HeaderFontFamily
		{
			get
			{
				return (string)GetValue(HeaderFontFamilyProperty);
			}
			set
			{
				SetValue(HeaderFontFamilyProperty, value);
			}
		}

		public FontAttributes HeaderFontAttributes
		{
			get
			{
				return (FontAttributes)GetValue(HeaderFontAttributesProperty);
			}
			set
			{
				SetValue(HeaderFontAttributesProperty, value);
			}
		}

		public DataGridVerticalAlignment HeaderVerticalAlignment
		{
			get
			{
				return (DataGridVerticalAlignment)GetValue(HeaderVerticalAlignmentProperty);
			}
			set
			{
				SetValue(HeaderVerticalAlignmentProperty, value);
			}
		}

		public bool IsSortable
		{
			get
			{
				return (bool)GetValue(IsSortableProperty);
			}
			set
			{
				SetValue(IsSortableProperty, value);
			}
		}

		public DataGridSortType Sort
		{
			get
			{
				return (DataGridSortType)GetValue(SortProperty);
			}
			internal set
			{
				SetValue(SortPropertyKey, value);
			}
		}

		public ICommand SortCommand
		{
			get
			{
				return (ICommand)GetValue(SortCommandProperty);
			}
			set
			{
				SetValue(SortCommandProperty, value);
			}
		}

		public ImageSource UnsortedIcon
		{
			get
			{
				return (ImageSource)GetValue(UnsortedIconProperty);
			}
			set
			{
				SetValue(UnsortedIconProperty, value);
			}
		}

		public ImageSource AscendingSortIcon
		{
			get
			{
				return (ImageSource)GetValue(AscendingSortIconProperty);
			}
			set
			{
				SetValue(AscendingSortIconProperty, value);
			}
		}

		public ImageSource DescendingSortIcon
		{
			get
			{
				return (ImageSource)GetValue(DescendingSortIconProperty);
			}
			set
			{
				SetValue(DescendingSortIconProperty, value);
			}
		}

		public string SortIconsFontFamily
		{
			get
			{
				return (string)GetValue(SortIconsFontFamilyProperty);
			}
			set
			{
				SetValue(SortIconsFontFamilyProperty, value);
			}
		}

		public double SortIconsFontSize
		{
			get
			{
				return (double)GetValue(SortIconsFontSizeProperty);
			}
			set
			{
				SetValue(SortIconsFontSizeProperty, value);
			}
		}

		public string UnsortedIconText
		{
			get
			{
				return (string)GetValue(UnsortedIconTextProperty);
			}
			set
			{
				SetValue(UnsortedIconTextProperty, value);
			}
		}

		public string AscendingSortIconText
		{
			get
			{
				return (string)GetValue(AscendingSortIconTextProperty);
			}
			set
			{
				SetValue(AscendingSortIconTextProperty, value);
			}
		}

		public string DescendingSortIconText
		{
			get
			{
				return (string)GetValue(DescendingSortIconTextProperty);
			}
			set
			{
				SetValue(DescendingSortIconTextProperty, value);
			}
		}

		public string CurrentSortIconText
		{
			get
			{
				return (string)GetValue(CurrentSortIconTextProperty);
			}
			set
			{
				SetValue(CurrentSortIconTextProperty, value);
			}
		}

		public ImageSource CurrentSortIcon
		{
			get
			{
				return (ImageSource)GetValue(CurrentSortIconProperty);
			}
			set
			{
				SetValue(CurrentSortIconProperty, value);
			}
		}
	}
}
