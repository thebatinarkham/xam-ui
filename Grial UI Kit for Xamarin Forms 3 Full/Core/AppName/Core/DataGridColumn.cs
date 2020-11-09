using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppName.Core
{
    public class DataGridColumn : View
	{
		internal class InternalHeaderCalculatedProperties : INotifyPropertyChanged
		{
			private readonly DataGridColumn _column;

			private ImageSource _sortIcon;

			private string _sortIconText;

			public ImageSource SortIcon
			{
				get
				{
					if (!_column.IsSortable)
					{
						return null;
					}
					if (_sortIcon == null)
					{
						_sortIcon = _column.UnsortedIcon;
					}
					return _sortIcon;
				}
				set
				{
					if (_sortIcon != value)
					{
						_sortIcon = value;
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SortIcon"));
					}
				}
			}

			public string SortIconText
			{
				get
				{
					if (!_column.IsSortable)
					{
						return null;
					}
					if (string.IsNullOrEmpty(_sortIconText))
					{
						_sortIconText = _column.UnsortedIconText;
					}
					return _sortIconText;
				}
				set
				{
					_sortIconText = value;
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SortIconText"));
				}
			}

			public event PropertyChangedEventHandler PropertyChanged;

			public InternalHeaderCalculatedProperties(DataGridColumn column)
			{
				_column = column;
			}
		}

		internal class InternalCellCalculatedProperties : BindableObject
		{
			private readonly DataGridColumn _column;

			public static readonly BindableProperty CalculatedCellTextAlignmentProperty = BindableProperty.Create("CalculatedCellTextAlignment", typeof(TextAlignment), typeof(DataGridColumn), TextAlignment.Start);

			private TextAlignment? _calculatedCellTextAlignment;

			public TextAlignment CalculatedCellTextAlignment
			{
				get
				{
					return (TextAlignment)GetValue(CalculatedCellTextAlignmentProperty);
				}
				set
				{
					SetValue(CalculatedCellTextAlignmentProperty, value);
				}
			}

			public InternalCellCalculatedProperties(DataGridColumn column)
			{
				_column = column;
			}

			public void UpdateCalculatedCellTextAlignment(object rowBindingContext)
			{
				if (!_calculatedCellTextAlignment.HasValue)
				{
					Type type = rowBindingContext.EvaluateBindingPathTargetType(_column.BindingPath);
					if (type != null)
					{
						if (type.IsNumericType() || type.IsDateType())
						{
							_calculatedCellTextAlignment = TextAlignment.End;
						}
						else
						{
							_calculatedCellTextAlignment = TextAlignment.Start;
						}
					}
					else
					{
						_calculatedCellTextAlignment = TextAlignment.Start;
					}
				}
				CalculatedCellTextAlignment = (_column.CellTextAlignment ?? _calculatedCellTextAlignment.Value);
			}

			public void Reset()
			{
				_calculatedCellTextAlignment = null;
				CalculatedCellTextAlignment = _column.CellTextAlignment.GetValueOrDefault();
			}
		}

		public static readonly BindableProperty ColumnWidthProperty = BindableProperty.Create("ColumnWidth", typeof(GridLength), typeof(DataGridColumn), new GridLength(1.0, GridUnitType.Star));

		public static readonly BindableProperty BindingPathProperty = BindableProperty.Create("BindingPath", typeof(string), typeof(DataGridColumn));

		public static readonly BindableProperty ConverterProperty = BindableProperty.Create("Converter", typeof(IValueConverter), typeof(DataGridColumn));

		public static readonly BindableProperty ConverterParameterProperty = BindableProperty.Create("ConverterParameter", typeof(object), typeof(DataGridColumn));

		public static readonly BindableProperty StringFormatProperty = BindableProperty.Create("StringFormat", typeof(string), typeof(DataGridColumn), "{0}");

		public static readonly BindableProperty HeaderTextProperty = BindableProperty.Create("HeaderText", typeof(string), typeof(DataGridColumn), string.Empty);

		public static readonly BindableProperty HeaderTextColorProperty = BindableProperty.Create("HeaderTextColor", typeof(Color), typeof(DataGridColumn), Color.Black);

		public static readonly BindableProperty HeaderTextAlignmentProperty = BindableProperty.Create("HeaderTextAlignment", typeof(TextAlignment), typeof(DataGridColumn), TextAlignment.Start);

		public static readonly BindableProperty HeaderFontSizeProperty = BindableProperty.Create("HeaderFontSize", typeof(double), typeof(DataGridColumn), Device.GetNamedSize(NamedSize.Medium, typeof(Label)));

		public static readonly BindableProperty HeaderFontFamilyProperty = BindableProperty.Create("HeaderFontFamily", typeof(string), typeof(DataGridColumn));

		public static readonly BindableProperty HeaderFontAttributesProperty = BindableProperty.Create("HeaderFontAttributes", typeof(FontAttributes), typeof(DataGridColumn), FontAttributes.None);

		public static readonly BindableProperty HeaderVerticalAlignmentProperty = BindableProperty.Create("HeaderVerticalAlignment", typeof(DataGridVerticalAlignment), typeof(DataGridColumn), DataGridVerticalAlignment.Center);

		public static readonly BindableProperty HeaderBackgroundColorProperty = BindableProperty.Create("HeaderBackgroundColor", typeof(Color), typeof(DataGridColumn), Color.Default);

		public static readonly BindableProperty HeaderPaddingProperty = BindableProperty.Create("HeaderPadding", typeof(Thickness), typeof(DataGridColumn), new Thickness(4.0));

		public static readonly BindableProperty HeaderControlTemplateProperty = BindableProperty.Create("HeaderControlTemplate", typeof(ControlTemplate), typeof(DataGridColumn));

		public static readonly BindableProperty CellTextColorProperty = BindableProperty.Create("CellTextColor", typeof(Color), typeof(DataGridColumn), Color.Black);

		public static readonly BindableProperty CellTextAlignmentProperty = BindableProperty.Create("CellTextAlignment", typeof(TextAlignment?), typeof(DataGridColumn), null, BindingMode.OneWay, null, OnCellTextAlignmentChanged);

		public static readonly BindableProperty CellFontSizeProperty = BindableProperty.Create("CellFontSize", typeof(double), typeof(DataGridColumn), Device.GetNamedSize(NamedSize.Medium, typeof(Label)));

		public static readonly BindableProperty CellFontFamilyProperty = BindableProperty.Create("CellFontFamily", typeof(string), typeof(DataGridColumn));

		public static readonly BindableProperty CellFontAttributesProperty = BindableProperty.Create("CellFontAttributes", typeof(FontAttributes), typeof(DataGridColumn), FontAttributes.None);

		public static readonly BindableProperty CellBackgroundColorProperty = BindableProperty.Create("CellBackgroundColor", typeof(Color), typeof(DataGridColumn), Color.Default);

		public static readonly BindableProperty CellPaddingProperty = BindableProperty.Create("CellPadding", typeof(Thickness), typeof(DataGridColumn), new Thickness(4.0));

		public static readonly BindableProperty CellVerticalAlignmentProperty = BindableProperty.Create("CellVerticalAlignment", typeof(DataGridVerticalAlignment), typeof(DataGridColumn), DataGridVerticalAlignment.Center);

		public static readonly BindableProperty CellTemplateProperty = BindableProperty.Create("CellTemplate", typeof(DataTemplate), typeof(DataGridColumn));

		public static readonly BindableProperty CellTemplateBindingContextPathProperty = BindableProperty.Create("CellTemplateBindingContextPath", typeof(string), typeof(DataGridColumn));

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new static readonly BindableProperty HorizontalOptionsProperty;

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new static readonly BindableProperty MarginProperty;

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new static readonly BindableProperty VerticalOptionsProperty;

		public static readonly BindableProperty IsSortableProperty = BindableProperty.Create("IsSortable", typeof(bool), typeof(DataGridColumn), false);

		public static readonly BindablePropertyKey SortPropertyKey = BindableProperty.CreateReadOnly("Sort", typeof(DataGridSortType), typeof(DataGridColumn), DataGridSortType.Unsorted, BindingMode.OneWayToSource, null, SortTypeChanged);

		public static readonly BindableProperty SortProperty = SortPropertyKey.BindableProperty;

		public static readonly BindableProperty SortCommandProperty = BindableProperty.Create("SortCommand", typeof(ICommand), typeof(DataGridColumn));

		public static readonly BindableProperty UnsortedIconProperty = BindableProperty.Create("UnsortedIcon", typeof(ImageSource), typeof(DataGridColumn));

		public static readonly BindableProperty AscendingSortIconProperty = BindableProperty.Create("AscendingSortIcon", typeof(ImageSource), typeof(DataGridColumn));

		public static readonly BindableProperty DescendingSortIconProperty = BindableProperty.Create("DescendingSortIcon", typeof(ImageSource), typeof(DataGridColumn));

		public static readonly BindableProperty SortIconsFontFamilyProperty = BindableProperty.Create("SortIconsFontFamily", typeof(string), typeof(DataGridColumn));

		public static readonly BindableProperty SortIconsFontSizeProperty = BindableProperty.Create("SortIconsFontSize", typeof(double), typeof(DataGridColumn), Device.GetNamedSize(NamedSize.Medium, typeof(Label)));

		public static readonly BindableProperty UnsortedIconTextProperty = BindableProperty.Create("UnsortedIconText", typeof(string), typeof(DataGridColumn));

		public static readonly BindableProperty AscendingSortIconTextProperty = BindableProperty.Create("AscendingSortIconText", typeof(string), typeof(DataGridColumn));

		public static readonly BindableProperty DescendingSortIconTextProperty = BindableProperty.Create("DescendingSortIconText", typeof(string), typeof(DataGridColumn));

		public GridLength ColumnWidth
		{
			get
			{
				return (GridLength)GetValue(ColumnWidthProperty);
			}
			set
			{
				SetValue(ColumnWidthProperty, value);
			}
		}

		public string BindingPath
		{
			get
			{
				return (string)GetValue(BindingPathProperty);
			}
			set
			{
				SetValue(BindingPathProperty, value);
			}
		}

		public IValueConverter Converter
		{
			get
			{
				return (IValueConverter)GetValue(ConverterProperty);
			}
			set
			{
				SetValue(ConverterProperty, value);
			}
		}

		public object ConverterParameter
		{
			get
			{
				return GetValue(ConverterParameterProperty);
			}
			set
			{
				SetValue(ConverterParameterProperty, value);
			}
		}

		public string StringFormat
		{
			get
			{
				return (string)GetValue(StringFormatProperty);
			}
			set
			{
				SetValue(StringFormatProperty, value);
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

		public ControlTemplate HeaderControlTemplate
		{
			get
			{
				return (ControlTemplate)GetValue(HeaderControlTemplateProperty);
			}
			set
			{
				SetValue(HeaderControlTemplateProperty, value);
			}
		}

		public Color CellTextColor
		{
			get
			{
				return (Color)GetValue(CellTextColorProperty);
			}
			set
			{
				SetValue(CellTextColorProperty, value);
			}
		}

		public TextAlignment? CellTextAlignment
		{
			get
			{
				return (TextAlignment?)GetValue(CellTextAlignmentProperty);
			}
			set
			{
				SetValue(CellTextAlignmentProperty, value);
			}
		}

		public double CellFontSize
		{
			get
			{
				return (double)GetValue(CellFontSizeProperty);
			}
			set
			{
				SetValue(CellFontSizeProperty, value);
			}
		}

		public string CellFontFamily
		{
			get
			{
				return (string)GetValue(CellFontFamilyProperty);
			}
			set
			{
				SetValue(CellFontFamilyProperty, value);
			}
		}

		public FontAttributes CellFontAttributes
		{
			get
			{
				return (FontAttributes)GetValue(CellFontAttributesProperty);
			}
			set
			{
				SetValue(CellFontAttributesProperty, value);
			}
		}

		public Color CellBackgroundColor
		{
			get
			{
				return (Color)GetValue(CellBackgroundColorProperty);
			}
			set
			{
				SetValue(CellBackgroundColorProperty, value);
			}
		}

		public Thickness CellPadding
		{
			get
			{
				return (Thickness)GetValue(CellPaddingProperty);
			}
			set
			{
				SetValue(CellPaddingProperty, value);
			}
		}

		public DataGridVerticalAlignment CellVerticalAlignment
		{
			get
			{
				return (DataGridVerticalAlignment)GetValue(CellVerticalAlignmentProperty);
			}
			set
			{
				SetValue(CellVerticalAlignmentProperty, value);
			}
		}

		public DataTemplate CellTemplate
		{
			get
			{
				return (DataTemplate)GetValue(CellTemplateProperty);
			}
			set
			{
				SetValue(CellTemplateProperty, value);
			}
		}

		public string CellTemplateBindingContextPath
		{
			get
			{
				return (string)GetValue(CellTemplateBindingContextPathProperty);
			}
			set
			{
				SetValue(CellTemplateBindingContextPathProperty, value);
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new double AnchorX
		{
			get;
			set;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new double AnchorY
		{
			get;
			set;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new string AutomationId
		{
			get;
			set;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new Color BackgroundColor
		{
			get;
			set;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new Rectangle Bounds
		{
			get;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new FlowDirection FlowDirection
		{
			get;
			set;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new double HeightRequest
		{
			get;
			set;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new LayoutOptions HorizontalOptions
		{
			get;
			set;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new bool IsEnabled
		{
			get;
			set;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new bool IsFocused
		{
			get;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new Thickness Margin
		{
			get;
			set;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new double MinimumHeightRequest
		{
			get;
			set;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new double MinimumWidthRequest
		{
			get;
			set;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new Element Parent
		{
			get
			{
				return base.Parent;
			}
			set
			{
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new double Rotation
		{
			get;
			set;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new double RotationX
		{
			get;
			set;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new double RotationY
		{
			get;
			set;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new double Scale
		{
			get;
			set;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new double TranslationX
		{
			get;
			set;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new double TranslationY
		{
			get;
			set;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new LayoutOptions VerticalOptions
		{
			get;
			set;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new double WidthRequest
		{
			get;
			set;
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

		internal InternalHeaderCalculatedProperties CalculatedHeaderValues
		{
			get;
		}

		internal InternalCellCalculatedProperties CalculatedCellValues
		{
			get;
		}

		public DataGridColumn()
		{
			CalculatedCellValues = new InternalCellCalculatedProperties(this);
			CalculatedHeaderValues = new InternalHeaderCalculatedProperties(this);
		}

		private static void OnCellTextAlignmentChanged(BindableObject bindable, object oldValue, object newValue)
		{
			TextAlignment? textAlignment = (TextAlignment?)newValue;
			if (textAlignment.HasValue)
			{
				((DataGridColumn)bindable).CalculatedCellValues.CalculatedCellTextAlignment = textAlignment.Value;
			}
		}

		private static void SortTypeChanged(BindableObject bindable, object oldValue, object newValue)
		{
			DataGridColumn dataGridColumn = (DataGridColumn)bindable;
			ImageSource sortIcon;
			string sortIconText;
			switch ((DataGridSortType)newValue)
			{
			case DataGridSortType.Ascending:
				sortIcon = dataGridColumn.AscendingSortIcon;
				sortIconText = dataGridColumn.AscendingSortIconText;
				break;
			case DataGridSortType.Descending:
				sortIcon = dataGridColumn.DescendingSortIcon;
				sortIconText = dataGridColumn.DescendingSortIconText;
				break;
			default:
				sortIcon = dataGridColumn.UnsortedIcon;
				sortIconText = dataGridColumn.UnsortedIconText;
				break;
			}
			dataGridColumn.CalculatedHeaderValues.SortIcon = sortIcon;
			dataGridColumn.CalculatedHeaderValues.SortIconText = sortIconText;
		}
	}
}
