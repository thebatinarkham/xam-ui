using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace AppName.Core
{
    public class DataGrid : ContentView
	{
		internal class StripedListView : Xamarin.Forms.ListView
		{
			private readonly DataGrid _owner;

			private double? _measuredAutoHeight;

			private bool _autoSized;

			public bool AutoSized
			{
				get
				{
					return _autoSized;
				}
				set
				{
					if (value != _autoSized)
					{
						_autoSized = value;
						this.AutoSizedChanged?.Invoke(this, EventArgs.Empty);
					}
				}
			}

			private bool IsEligibleForAutoSize
			{
				get
				{
					if (!base.HasUnevenRows)
					{
						return base.RowHeight > 0;
					}
					return false;
				}
			}

			public event EventHandler AutoSizedChanged;

			public StripedListView(DataGrid owner, ListViewCachingStrategy strategy)
				: base(strategy)
			{
				_owner = owner;
				base.SelectionMode = ListViewSelectionMode.None;
				Xamarin.Forms.PlatformConfiguration.iOSSpecific.ListView.SetSeparatorStyle(this, SeparatorStyle.FullWidth);
			}

			protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
			{
				SizeRequest result = base.OnMeasure(widthConstraint, heightConstraint);
				if (IsEligibleForAutoSize && double.IsPositiveInfinity(heightConstraint))
				{
					_measuredAutoHeight = result.Request.Height;
				}
				else
				{
					_measuredAutoHeight = null;
				}
				double num = _owner.CalculateColumnWidth();
				if (!double.IsNaN(num))
				{
					result.Request = new Size(num, result.Request.Height);
				}
				return result;
			}

			protected override void OnSizeAllocated(double width, double height)
			{
				base.OnSizeAllocated(width, height);
				AutoSized = (_measuredAutoHeight.HasValue && _measuredAutoHeight.Value <= height);
			}

			protected override void SetupContent(Xamarin.Forms.Cell content, int index)
			{
				base.SetupContent(content, index);
				ViewCell viewCell = content as ViewCell;
				if (viewCell != null)
				{
					(viewCell.View as DataGridRow)?.SetBinding(DataGridRow.OriginalBackgroundColorProperty, new Binding((index % 2 == 0) ? "OddRowsBackgroundColor" : "EvenRowsBackgroundColor", BindingMode.Default, null, null, null, _owner));
				}
			}
		}

		private class DataGridRowViewCell : ViewCell
		{
			private readonly DataGrid _owner;

			public DataGridRowViewCell(DataGrid owner)
			{
				_owner = owner;
			}

			protected override void OnAppearing()
			{
				base.OnAppearing();
				_owner.RegisterRow(this);
			}

			protected override void OnDisappearing()
			{
				base.OnDisappearing();
				_owner.UnregisterRow(this);
			}
		}

		private HashSet<DataGridRowViewCell> _visibleRowCells;

		private StackLayout _mainStackLayout;

		private DataGridHeader _header;

		private Xamarin.Forms.ListView _rowsListView;

		private double _lastKnownWidth = double.NaN;

		private OrientationChangePatch _orientationChangePatch;

		private bool _ignoreSelectionItemChange;

		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(DataGrid));

		public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create("SelectedItem", typeof(object), typeof(DataGrid), null, BindingMode.TwoWay, null, OnSelectedItemChanged);

		public static readonly BindableProperty OddRowsBackgroundColorProperty = BindableProperty.Create("OddRowsBackgroundColor", typeof(Color), typeof(DataGrid), Color.Default);

		public static readonly BindableProperty EvenRowsBackgroundColorProperty = BindableProperty.Create("EvenRowsBackgroundColor", typeof(Color), typeof(DataGrid), Color.Default);

		public static readonly BindableProperty GridSeparatorVisibilityProperty = BindableProperty.Create("GridSeparatorVisibility", typeof(DataGridSeparatorVisibility), typeof(DataGrid), DataGridSeparatorVisibility.All);

		public static readonly BindableProperty RowSeparatorColorProperty = BindableProperty.Create("RowSeparatorColor", typeof(Color), typeof(DataGrid), Color.DarkGray);

		public static readonly BindableProperty RowSeparatorHeightProperty = BindableProperty.Create("RowSeparatorHeight", typeof(double), typeof(DataGrid), 1.0);

		public static readonly BindableProperty ColumnSeparatorColorProperty = BindableProperty.Create("ColumnSeparatorColor", typeof(Color), typeof(DataGrid), Color.DarkGray);

		public static readonly BindableProperty ColumnSeparatorWidthProperty = BindableProperty.Create("ColumnSeparatorWidth", typeof(double), typeof(DataGrid), 1.0);

		public static readonly BindableProperty RowHeightProperty = BindableProperty.Create("RowHeight", typeof(double), typeof(DataGrid), -1.0, BindingMode.OneWay, null, OnRowHeightChanged);

		public static readonly BindableProperty SelectedBackgroundColorProperty = BindableProperty.Create("SelectedBackgroundColor", typeof(Color), typeof(DataGrid), Color.LightGray);

		public static readonly BindableProperty HeaderRowHeightProperty = BindableProperty.Create("HeaderRowHeight", typeof(double), typeof(DataGrid), -1.0);

		public static readonly BindableProperty HeaderRowBackgroundColorProperty = BindableProperty.Create("HeaderRowBackgroundColor", typeof(Color), typeof(DataGrid), Color.Gray);

		public static readonly BindableProperty HeaderSeparatorVisibilityProperty = BindableProperty.Create("HeaderSeparatorVisibility", typeof(DataGridSeparatorVisibility), typeof(DataGrid), DataGridSeparatorVisibility.All);

		public static readonly BindableProperty HeaderRowSeparatorHeightProperty = BindableProperty.Create("HeaderRowSeparatorHeight", typeof(double), typeof(DataGrid), 1.0);

		public static readonly BindableProperty HeaderRowSeparatorColorProperty = BindableProperty.Create("HeaderRowSeparatorColor", typeof(Color), typeof(DataGrid), Color.DarkGray);

		public static readonly BindableProperty HeaderColumnSeparatorWidthProperty = BindableProperty.Create("HeaderColumnSeparatorWidth", typeof(double), typeof(DataGrid), 1.0);

		public static readonly BindableProperty HeaderColumnSeparatorColorProperty = BindableProperty.Create("HeaderColumnSeparatorColor", typeof(Color), typeof(DataGrid), Color.DarkGray);

		public static readonly BindableProperty SelectionModeProperty = BindableProperty.Create("SelectionMode", typeof(DataGridSelectionMode), typeof(DataGrid), DataGridSelectionMode.Row);

		public static readonly BindableProperty ItemSelectedCommandProperty = BindableProperty.Create("ItemSelectedCommand", typeof(ICommand), typeof(DataGrid));

		public static readonly BindableProperty HorizontalScrollBarVisibilityProperty = BindableProperty.Create("HorizontalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(DataGrid), ScrollBarVisibility.Default, BindingMode.OneWay, null, HorizontalScrollChanged);

		internal HashSet<DataGridRow> SelectedRows
		{
			get;
			private set;
		}

		public IList<DataGridColumn> ColumnDefinitions
		{
			get;
			set;
		} = new List<DataGridColumn>();


		public IEnumerable ItemsSource
		{
			get
			{
				return (IEnumerable)GetValue(ItemsSourceProperty);
			}
			set
			{
				SetValue(ItemsSourceProperty, value);
			}
		}

		public object SelectedItem
		{
			get
			{
				return GetValue(SelectedItemProperty);
			}
			set
			{
				SetValue(SelectedItemProperty, value);
			}
		}

		public Color OddRowsBackgroundColor
		{
			get
			{
				return (Color)GetValue(OddRowsBackgroundColorProperty);
			}
			set
			{
				SetValue(OddRowsBackgroundColorProperty, value);
			}
		}

		public Color EvenRowsBackgroundColor
		{
			get
			{
				return (Color)GetValue(EvenRowsBackgroundColorProperty);
			}
			set
			{
				SetValue(EvenRowsBackgroundColorProperty, value);
			}
		}

		public DataGridSeparatorVisibility GridSeparatorVisibility
		{
			get
			{
				return (DataGridSeparatorVisibility)GetValue(GridSeparatorVisibilityProperty);
			}
			set
			{
				SetValue(GridSeparatorVisibilityProperty, value);
			}
		}

		public Color RowSeparatorColor
		{
			get
			{
				return (Color)GetValue(RowSeparatorColorProperty);
			}
			set
			{
				SetValue(RowSeparatorColorProperty, value);
			}
		}

		public double RowSeparatorHeight
		{
			get
			{
				return (double)GetValue(RowSeparatorHeightProperty);
			}
			set
			{
				SetValue(RowSeparatorHeightProperty, value);
			}
		}

		public Color ColumnSeparatorColor
		{
			get
			{
				return (Color)GetValue(ColumnSeparatorColorProperty);
			}
			set
			{
				SetValue(ColumnSeparatorColorProperty, value);
			}
		}

		public double ColumnSeparatorWidth
		{
			get
			{
				return (double)GetValue(ColumnSeparatorWidthProperty);
			}
			set
			{
				SetValue(ColumnSeparatorWidthProperty, value);
			}
		}

		public double RowHeight
		{
			get
			{
				return (double)GetValue(RowHeightProperty);
			}
			set
			{
				SetValue(RowHeightProperty, value);
			}
		}

		public Color SelectedBackgroundColor
		{
			get
			{
				return (Color)GetValue(SelectedBackgroundColorProperty);
			}
			set
			{
				SetValue(SelectedBackgroundColorProperty, value);
			}
		}

		public double HeaderRowHeight
		{
			get
			{
				return (double)GetValue(HeaderRowHeightProperty);
			}
			set
			{
				SetValue(HeaderRowHeightProperty, value);
			}
		}

		public Color HeaderRowBackgroundColor
		{
			get
			{
				return (Color)GetValue(HeaderRowBackgroundColorProperty);
			}
			set
			{
				SetValue(HeaderRowBackgroundColorProperty, value);
			}
		}

		public DataGridSeparatorVisibility HeaderSeparatorVisibility
		{
			get
			{
				return (DataGridSeparatorVisibility)GetValue(HeaderSeparatorVisibilityProperty);
			}
			set
			{
				SetValue(HeaderSeparatorVisibilityProperty, value);
			}
		}

		public double HeaderRowSeparatorHeight
		{
			get
			{
				return (double)GetValue(HeaderRowSeparatorHeightProperty);
			}
			set
			{
				SetValue(HeaderRowSeparatorHeightProperty, value);
			}
		}

		public Color HeaderRowSeparatorColor
		{
			get
			{
				return (Color)GetValue(HeaderRowSeparatorColorProperty);
			}
			set
			{
				SetValue(HeaderRowSeparatorColorProperty, value);
			}
		}

		public double HeaderColumnSeparatorWidth
		{
			get
			{
				return (double)GetValue(HeaderColumnSeparatorWidthProperty);
			}
			set
			{
				SetValue(HeaderColumnSeparatorWidthProperty, value);
			}
		}

		public Color HeaderColumnSeparatorColor
		{
			get
			{
				return (Color)GetValue(HeaderColumnSeparatorColorProperty);
			}
			set
			{
				SetValue(HeaderColumnSeparatorColorProperty, value);
			}
		}

		public DataGridSelectionMode SelectionMode
		{
			get
			{
				return (DataGridSelectionMode)GetValue(SelectionModeProperty);
			}
			set
			{
				SetValue(SelectionModeProperty, value);
			}
		}

		public ICommand ItemSelectedCommand
		{
			get
			{
				return (ICommand)GetValue(ItemSelectedCommandProperty);
			}
			set
			{
				SetValue(ItemSelectedCommandProperty, value);
			}
		}

		public ScrollBarVisibility HorizontalScrollBarVisibility
		{
			get
			{
				return (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty);
			}
			set
			{
				SetValue(HorizontalScrollBarVisibilityProperty, value);
			}
		}

		public event EventHandler<DataGridSelectedItemChangedEventArgs> ItemSelected;

		public DataGrid()
		{
		}

		private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
		{
			DataGrid dataGrid = (DataGrid)bindable;
			if (!dataGrid._ignoreSelectionItemChange)
			{
				dataGrid.UpdateSelection(newValue, null);
			}
		}

		private static void OnRowHeightChanged(BindableObject bindable, object oldValue, object newValue)
		{
			DataGrid dataGrid = (DataGrid)bindable;
			if (dataGrid._rowsListView != null)
			{
				dataGrid._rowsListView.RowHeight = (int)dataGrid.RowHeight;
				dataGrid._rowsListView.HasUnevenRows = (dataGrid.RowHeight < 0.0);
			}
		}

		private static void HorizontalScrollChanged(BindableObject bindable, object oldValue, object newValue)
		{
			Xamarin.Forms.ScrollView scrollView = ((DataGrid)bindable).Content as Xamarin.Forms.ScrollView;
			if (scrollView != null)
			{
				scrollView.HorizontalScrollBarVisibility = (ScrollBarVisibility)newValue;
			}
		}

		protected override void OnPropertyChanged(string propertyName)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == ItemsSourceProperty.PropertyName)
			{
				CreateListViewDataGrid();
				_rowsListView.ItemsSource = ItemsSource;
			}
			else if (propertyName == Xamarin.Forms.VisualElement.WidthProperty.PropertyName)
			{
				_orientationChangePatch?.OnDimensionChanged(base.Width, delegate
				{
					_mainStackLayout.Spacing = 10.0;
				}, delegate
				{
					_mainStackLayout.Spacing = 0.0;
				});
			}
		}

		private void CreateListViewDataGrid()
		{
			ColumnDefinitions.ForEach(delegate(DataGridColumn x)
			{
				x.CalculatedCellValues.Reset();
			});
			SelectedRows = new HashSet<DataGridRow>();
			_visibleRowCells = new HashSet<DataGridRowViewCell>();
			_mainStackLayout = new StackLayout
			{
				Spacing = 0.0
			};
			base.Content = new Xamarin.Forms.ScrollView
			{
				Content = _mainStackLayout,
				IsClippedToBounds = true,
				Orientation = ScrollOrientation.Horizontal,
				HorizontalScrollBarVisibility = HorizontalScrollBarVisibility,
				VerticalScrollBarVisibility = ScrollBarVisibility.Never
			};
			CreateListViewHeader();
			CreateListViewRows();
		}

		private void CreateListViewHeader()
		{
			if (_header != null)
			{
				_mainStackLayout.Children.Remove(_header);
				_header.SortChanged -= ColumnSortChanged;
				_header.Dispose();
			}
			_header = new DataGridHeader(this);
			_header.SortChanged += ColumnSortChanged;
			_mainStackLayout.Children.Insert(0, _header);
		}

		private void CreateListViewRows()
		{
			_rowsListView = new StripedListView(this, ListViewCachingStrategy.RecycleElement)
			{
				SeparatorVisibility = SeparatorVisibility.None,
				RowHeight = (int)RowHeight,
				HasUnevenRows = (RowHeight < 0.0),
				ItemTemplate = new DataTemplate(delegate
				{
					DataGridRow dataGridRow = new DataGridRow(this)
					{
						OnTapCommand = new Command(OnRowGridTapped, (object _) => SelectionMode == DataGridSelectionMode.Row)
					};
					dataGridRow.SetBinding(DataGridRow.SelectedBackgroundColorProperty, new Binding("SelectedBackgroundColor", BindingMode.Default, null, null, null, this));
					return new DataGridRowViewCell(this)
					{
						View = dataGridRow
					};
				})
			};
			_mainStackLayout.Children.Add(_rowsListView);
		}

		private void OnRowGridTapped(object sender)
		{
			DataGridRow dataGridRow = sender as DataGridRow;
			if (dataGridRow != null && !SelectedRows.Contains(dataGridRow))
			{
				_ignoreSelectionItemChange = true;
				UpdateSelection(dataGridRow.BindingContext, dataGridRow);
				_ignoreSelectionItemChange = false;
			}
		}

		private void UpdateSelection(object item, DataGridRow row)
		{
			SelectedItem = item;
			this.ItemSelected?.Invoke(this, new DataGridSelectedItemChangedEventArgs(SelectedItem));
			ItemSelectedCommand?.Execute(SelectedItem);
			if (SelectedRows.Count > 0)
			{
				foreach (DataGridRow selectedRow in SelectedRows)
				{
					selectedRow.IsSelected = false;
				}
				SelectedRows.Clear();
			}
			if (SelectedItem != null)
			{
				if (row == null)
				{
					foreach (DataGridRowViewCell visibleRowCell in _visibleRowCells)
					{
						if (visibleRowCell.View?.BindingContext == item)
						{
							row = (visibleRowCell.View as DataGridRow);
							row.IsSelected = true;
							SelectedRows.Add(row);
						}
					}
					return;
				}
				row.IsSelected = true;
				SelectedRows.Add(row);
			}
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			_lastKnownWidth = width;
			base.OnSizeAllocated(width, height);
		}

		internal double CalculateColumnWidth()
		{
			double num = 0.0;
			for (int i = 0; i < ColumnDefinitions.Count; i++)
			{
				GridLength columnWidth = ColumnDefinitions[i].ColumnWidth;
				if (columnWidth.IsAbsolute)
				{
					num += columnWidth.Value;
					continue;
				}
				if (columnWidth.IsStar)
				{
					if (!(base.WidthRequest > 0.0))
					{
						return _lastKnownWidth;
					}
					return base.WidthRequest;
				}
				if (columnWidth.IsAuto)
				{
					throw new ArgumentException("DataGtidView ColumnWidth can't be Auto. Please use a number or a proportional value with *.");
				}
			}
			return num;
		}

		private void RegisterRow(DataGridRowViewCell cell)
		{
			_visibleRowCells.Add(cell);
		}

		private void UnregisterRow(DataGridRowViewCell cell)
		{
			_visibleRowCells.Remove(cell);
		}

		protected override void OnParentSet()
		{
			base.OnParentSet();
			DataGridHeader header = _header;
			if (header != null && header.ColumnDefinitions.Count == 0)
			{
				CreateListViewHeader();
			}
			_orientationChangePatch = new OrientationChangePatch("Android");
		}

		private void ColumnSortChanged(object sender, EventArgs e)
		{
			DataGridColumn column = sender as DataGridColumn;
			DataGridSortChangedEventArgs dataGridSortChangedEventArgs = e as DataGridSortChangedEventArgs;
			if (ItemsSource != null && dataGridSortChangedEventArgs.SortType != 0)
			{
				SelectedRows.Clear();
				if (column.SortCommand != null)
				{
					column.SortCommand.Execute(column.Sort);
				}
				else
				{
					_rowsListView.ItemsSource = ((dataGridSortChangedEventArgs.SortType == DataGridSortType.Ascending) ? (from object x in ItemsSource
						orderby x.EvaluateBindingPath(column.BindingPath)
						select x) : (from object x in ItemsSource
						orderby x.EvaluateBindingPath(column.BindingPath) descending
						select x));
				}
			}
		}
	}
}
