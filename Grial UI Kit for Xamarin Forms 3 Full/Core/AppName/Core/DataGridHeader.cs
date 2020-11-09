using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace AppName.Core
{
	internal class DataGridHeader : DataGridRowBase
	{
		internal class InternalDataGridHeaderCell : DataGridHeaderCell, IDisposable
		{
			private enum SortType
			{
				None = 0,
				Default = 1,
				Image = 2,
				Text = 3
			}

			private readonly DataGridColumn _column;

			private readonly Label _label;

			private Grid _layout;

			private SortType _sort;

			public InternalDataGridHeaderCell(DataGridColumn column)
			{
				_column = column;
				base.VerticalOptions = LayoutOptions.FillAndExpand;
				base.HorizontalOptions = LayoutOptions.FillAndExpand;
				if (_column.HeaderControlTemplate != null)
				{
					SetBinding(DataGridHeaderCell.HeaderPaddingProperty, new Binding("HeaderPadding", BindingMode.Default, null, null, null, column));
					SetBinding(DataGridHeaderCell.HeaderBackgroundColorProperty, new Binding("HeaderBackgroundColor", BindingMode.Default, null, null, null, column));
					SetBinding(DataGridHeaderCell.HeaderTextProperty, new Binding("HeaderText", BindingMode.Default, null, null, null, column));
					SetBinding(DataGridHeaderCell.HeaderTextColorProperty, new Binding("HeaderTextColor", BindingMode.Default, null, null, null, column));
					SetBinding(DataGridHeaderCell.HeaderFontSizeProperty, new Binding("HeaderFontSize", BindingMode.Default, null, null, null, column));
					SetBinding(DataGridHeaderCell.HeaderFontFamilyProperty, new Binding("HeaderFontFamily", BindingMode.Default, null, null, null, column));
					SetBinding(DataGridHeaderCell.HeaderFontAttributesProperty, new Binding("HeaderFontAttributes", BindingMode.Default, null, null, null, column));
					SetBinding(DataGridHeaderCell.HeaderVerticalAlignmentProperty, new Binding("HeaderVerticalAlignment", BindingMode.Default, null, null, null, column));
					SetBinding(DataGridHeaderCell.HeaderTextAlignmentProperty, new Binding("HeaderTextAlignment", BindingMode.Default, null, null, null, column));
					SetBinding(DataGridHeaderCell.DescendingSortIconTextProperty, new Binding("DescendingSortIconText", BindingMode.Default, null, null, null, column));
					SetBinding(DataGridHeaderCell.AscendingSortIconTextProperty, new Binding("AscendingSortIconText", BindingMode.Default, null, null, null, column));
					SetBinding(DataGridHeaderCell.UnsortedIconTextProperty, new Binding("UnsortedIconText", BindingMode.Default, null, null, null, column));
					SetBinding(DataGridHeaderCell.SortIconsFontSizeProperty, new Binding("SortIconsFontSize", BindingMode.Default, null, null, null, column));
					SetBinding(DataGridHeaderCell.SortIconsFontFamilyProperty, new Binding("SortIconsFontFamily", BindingMode.Default, null, null, null, column));
					SetBinding(DataGridHeaderCell.AscendingSortIconProperty, new Binding("AscendingSortIcon", BindingMode.Default, null, null, null, column));
					SetBinding(DataGridHeaderCell.UnsortedIconProperty, new Binding("UnsortedIcon", BindingMode.Default, null, null, null, column));
					SetBinding(DataGridHeaderCell.SortCommandProperty, new Binding("SortCommand", BindingMode.Default, null, null, null, column));
					SetBinding(DataGridHeaderCell.SortProperty, new Binding("Sort", BindingMode.Default, null, null, null, column));
					SetBinding(DataGridHeaderCell.IsSortableProperty, new Binding("IsSortable", BindingMode.Default, null, null, null, column));
					SetBinding(DataGridHeaderCell.CurrentSortIconTextProperty, new Binding("SortIconText", BindingMode.Default, null, null, null, column.CalculatedHeaderValues));
					SetBinding(DataGridHeaderCell.CurrentSortIconProperty, new Binding("SortIcon", BindingMode.Default, null, null, null, column.CalculatedHeaderValues));
					base.ControlTemplate = _column.HeaderControlTemplate;
				}
				else
				{
					SetBinding(VisualElement.BackgroundColorProperty, new Binding("HeaderBackgroundColor", BindingMode.Default, null, null, null, column));
					SetBinding(Xamarin.Forms.Layout.PaddingProperty, new Binding("HeaderPadding", BindingMode.Default, null, null, null, column));
					_label = new Label();
					_label.SetBinding(Label.TextProperty, new Binding("HeaderText", BindingMode.Default, null, null, null, column));
					_label.SetBinding(Label.TextColorProperty, new Binding("HeaderTextColor", BindingMode.Default, null, null, null, column));
					_label.SetBinding(Label.FontSizeProperty, new Binding("HeaderFontSize", BindingMode.Default, null, null, null, column));
					_label.SetBinding(Label.FontFamilyProperty, new Binding("HeaderFontFamily", BindingMode.Default, null, null, null, column));
					_label.SetBinding(Label.FontAttributesProperty, new Binding("HeaderFontAttributes", BindingMode.Default, null, null, null, column));
					_label.SetBinding(View.VerticalOptionsProperty, new Binding("HeaderVerticalAlignment", BindingMode.Default, new LayoutOptionsConverter(), null, null, column));
					_label.SetBinding(View.HorizontalOptionsProperty, new Binding("HeaderTextAlignment", BindingMode.Default, new LayoutOptionsConverter(), null, null, column));
					UpdateSort();
					_column.PropertyChanged += OnColumnPropertyChanged;
				}
			}

			private void OnColumnPropertyChanged(object sender, PropertyChangedEventArgs e)
			{
				if (e.PropertyName == DataGridColumn.IsSortableProperty.PropertyName || e.PropertyName == DataGridColumn.AscendingSortIconProperty.PropertyName || e.PropertyName == DataGridColumn.DescendingSortIconProperty.PropertyName || e.PropertyName == DataGridColumn.AscendingSortIconTextProperty.PropertyName || e.PropertyName == DataGridColumn.DescendingSortIconTextProperty.PropertyName || e.PropertyName == DataGridColumn.SortIconsFontFamilyProperty.PropertyName)
				{
					UpdateSort();
				}
			}

			private void UpdateSort()
			{
				if (!_column.IsSortable)
				{
					_sort = SortType.None;
					_layout?.Children.Clear();
					base.Content = _label;
					return;
				}
				SortType sortType = SortType.None;
				sortType = ((_column.AscendingSortIcon != null && _column.DescendingSortIcon != null) ? SortType.Image : ((string.IsNullOrEmpty(_column.AscendingSortIconText) || string.IsNullOrEmpty(_column.DescendingSortIconText) || string.IsNullOrEmpty(_column.SortIconsFontFamily)) ? SortType.Default : SortType.Text));
				if (sortType != _sort)
				{
					_sort = sortType;
					if (_layout == null)
					{
						_layout = new Grid
						{
							ColumnSpacing = 0.0,
							HorizontalOptions = LayoutOptions.FillAndExpand,
							VerticalOptions = LayoutOptions.FillAndExpand
						};
						_layout.ColumnDefinitions.Add(new ColumnDefinition
						{
							Width = GridLength.Star
						});
						_layout.ColumnDefinitions.Add(new ColumnDefinition
						{
							Width = GridLength.Auto
						});
					}
					else
					{
						_layout.Children.Clear();
					}
					base.Content = _layout;
					_layout.Children.Add(_label);
					if (_sort == SortType.Text)
					{
						Label label = new Label();
						label.SetBinding(Label.FontFamilyProperty, new Binding("SortIconsFontFamily", BindingMode.Default, null, null, null, _column));
						label.SetBinding(Label.TextProperty, new Binding("SortIconText", BindingMode.Default, null, null, null, _column.CalculatedHeaderValues));
						label.SetBinding(Label.TextColorProperty, new Binding("HeaderTextColor", BindingMode.Default, null, null, null, _column));
						label.SetBinding(Label.FontSizeProperty, new Binding("SortIconsFontSize", BindingMode.Default, null, null, null, _column));
						label.SetBinding(View.VerticalOptionsProperty, new Binding("HeaderVerticalAlignment", BindingMode.Default, new LayoutOptionsConverter(), null, null, _column));
						Grid.SetColumn(label, 1);
						_layout.Children.Add(label);
						label = new Label();
						label.SetBinding(Label.FontFamilyProperty, new Binding("SortIconsFontFamily", BindingMode.Default, null, null, null, _column));
						label.SetBinding(Label.TextProperty, new Binding("AscendingSortIconText", BindingMode.Default, null, null, null, _column));
						label.SetBinding(Label.FontSizeProperty, new Binding("SortIconsFontSize", BindingMode.Default, null, null, null, _column));
						label.Opacity = 0.0;
						Grid.SetColumn(label, 1);
						_layout.Children.Add(label);
					}
					else if (_sort == SortType.Image)
					{
						Image image = new Image();
						image.SetBinding(Image.SourceProperty, new Binding("SortIcon", BindingMode.Default, null, null, null, _column.CalculatedHeaderValues));
						Grid.SetColumn(image, 1);
						_layout.Children.Add(image);
					}
					else
					{
						View view = BuildUpArrow();
						view.SetBinding(VisualElement.OpacityProperty, new Binding("Sort", BindingMode.Default, new AscendingOpacityConverter(), null, null, _column));
						view.SetBinding(View.VerticalOptionsProperty, new Binding("HeaderVerticalAlignment", BindingMode.Default, new LayoutOptionsConverter(), null, null, _column));
						Grid.SetColumn(view, 1);
						_layout.Children.Add(view);
						View view2 = BuildDownArrow();
						view2.SetBinding(VisualElement.OpacityProperty, new Binding("Sort", BindingMode.Default, new DescendingOpacityConverter(), null, null, _column));
						view2.SetBinding(View.VerticalOptionsProperty, new Binding("HeaderVerticalAlignment", BindingMode.Default, new LayoutOptionsConverter(), null, null, _column));
						Grid.SetColumn(view2, 1);
						_layout.Children.Add(view2);
					}
				}
			}

			public void Dispose()
			{
				_column.PropertyChanged -= OnColumnPropertyChanged;
			}

			private View BuildUpArrow()
			{
				Grid grid = BuildArrow();
				grid.Rotation = 180.0;
				grid.Margin = new Thickness(0.0, 0.0, 0.0, 6.0);
				return grid;
			}

			private View BuildDownArrow()
			{
				Grid grid = BuildArrow();
				grid.Margin = new Thickness(0.0, 6.0, 0.0, 0.0);
				return grid;
			}

			private Grid BuildArrow()
			{
				Grid obj = new Grid
				{
					HeightRequest = 10.0,
					WidthRequest = 14.0,
					IsClippedToBounds = true
				};
				BoxView boxView = new BoxView
				{
					WidthRequest = 10.0,
					HeightRequest = 10.0,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Start,
					Rotation = 45.0,
					TranslationY = -5.5
				};
				boxView.SetBinding(VisualElement.BackgroundColorProperty, new Binding("HeaderTextColor", BindingMode.Default, null, null, null, _column));
				obj.Children.Add(boxView);
				return obj;
			}
		}

		public override BindableProperty OwnerGridRowHeightProperty => DataGrid.HeaderRowHeightProperty;

		public override bool HasRowSeparator
		{
			get
			{
				DataGrid owner = _owner;
				if (owner == null || owner.HeaderSeparatorVisibility != DataGridSeparatorVisibility.All)
				{
					DataGrid owner2 = _owner;
					if (owner2 == null)
					{
						return false;
					}
					return owner2.HeaderSeparatorVisibility == DataGridSeparatorVisibility.Horizontal;
				}
				return true;
			}
		}

		public override string RowSeparatorHeightProperty => "HeaderRowSeparatorHeight";

		public override string RowSeparatorColorProperty => "HeaderRowSeparatorColor";

		public override string ColumnSeparatorWidthProperty => "HeaderColumnSeparatorWidth";

		public override string ColumnSeparatorColorProperty => "HeaderColumnSeparatorColor";

		public event EventHandler SortChanged;

		public override bool HasColumnSeparator(int index)
		{
			DataGrid owner = _owner;
			if (owner == null || owner.HeaderSeparatorVisibility != DataGridSeparatorVisibility.All)
			{
				DataGrid owner2 = _owner;
				if (owner2 == null || owner2.HeaderSeparatorVisibility != DataGridSeparatorVisibility.Vertical)
				{
					return false;
				}
			}
			return index < _columns.Count - 1;
		}

		public DataGridHeader(DataGrid owner)
			: base(owner)
		{
		}

		protected override void Initialize()
		{
			base.Initialize();
			SetBinding(VisualElement.BackgroundColorProperty, new Binding("HeaderRowBackgroundColor", BindingMode.Default, null, null, null, _owner));
		}

		protected override ContentView CreateCell(DataGridColumn column, int index)
		{
			return new InternalDataGridHeaderCell(column)
			{
				GestureRecognizers = 
				{
					(IGestureRecognizer)new TapGestureRecognizer
					{
						Command = new Command<DataGridColumn>(InternalSort, (DataGridColumn _) => column.IsSortable),
						CommandParameter = column
					}
				}
			};
		}

		private void InternalSort(DataGridColumn column)
		{
			if (column.IsSortable)
			{
				DataGridSortType dataGridSortType = DataGridSortType.Unsorted;
				DataGridSortType sort = column.Sort;
				dataGridSortType = ((sort == DataGridSortType.Unsorted || sort == DataGridSortType.Descending) ? DataGridSortType.Ascending : DataGridSortType.Descending);
				column.Sort = dataGridSortType;
				DataGridColumn dataGridColumn = _columns.FirstOrDefault((DataGridColumn col) => col.Id != column.Id && col.Sort != DataGridSortType.Unsorted);
				if (dataGridColumn != null)
				{
					dataGridColumn.Sort = DataGridSortType.Unsorted;
				}
				this.SortChanged?.Invoke(column, new DataGridSortChangedEventArgs(column.Sort));
			}
		}
	}
}
