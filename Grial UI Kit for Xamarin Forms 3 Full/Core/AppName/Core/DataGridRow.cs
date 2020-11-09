using System.Windows.Input;
using Xamarin.Forms;

namespace AppName.Core
{
	internal class DataGridRow : DataGridRowBase
	{
		private class CellContent : ContentView
		{
			private readonly Binding _backgroundColorBinding;

			public CellContent(DataGridColumn column)
			{
				_backgroundColorBinding = new Binding("CellBackgroundColor", BindingMode.Default, null, null, null, column);
				base.VerticalOptions = LayoutOptions.FillAndExpand;
				base.HorizontalOptions = LayoutOptions.FillAndExpand;
				SetBinding(Xamarin.Forms.Layout.PaddingProperty, new Binding("CellPadding", BindingMode.Default, null, null, null, column));
				if (column.CellTemplate != null)
				{
					object obj = column.CellTemplate.CreateContent();
					ViewCell viewCell = obj as ViewCell;
					if (viewCell != null)
					{
						base.Content = viewCell.View;
					}
					else
					{
						View view = obj as View;
						if (view != null)
						{
							base.Content = view;
						}
					}
					if (column.CellTemplateBindingContextPath != null)
					{
						try
						{
							Binding binding = new Binding(column.CellTemplateBindingContextPath);
							SetBinding(BindableObject.BindingContextProperty, binding);
						}
						catch
						{
						}
					}
				}
				else if (column.BindingPath != null)
				{
					Label label = new Label();
					label.SetBinding(Label.TextColorProperty, new Binding("CellTextColor", BindingMode.Default, null, null, null, column));
					label.SetBinding(Label.FontSizeProperty, new Binding("CellFontSize", BindingMode.Default, null, null, null, column));
					label.SetBinding(Label.FontFamilyProperty, new Binding("CellFontFamily", BindingMode.Default, null, null, null, column));
					label.SetBinding(Label.FontAttributesProperty, new Binding("CellFontAttributes", BindingMode.Default, null, null, null, column));
					label.SetBinding(View.VerticalOptionsProperty, new Binding("CellVerticalAlignment", BindingMode.Default, new LayoutOptionsConverter(), null, null, column));
					label.SetBinding(View.HorizontalOptionsProperty, new Binding("CalculatedCellTextAlignment", BindingMode.Default, new LayoutOptionsConverter(), null, null, column.CalculatedCellValues));
					try
					{
						Binding binding2 = new Binding(column.BindingPath, BindingMode.Default, column.Converter, column.ConverterParameter, column.StringFormat);
						label.SetBinding(Label.TextProperty, binding2);
					}
					catch
					{
						label.Text = "Invalid binding path";
					}
					base.Content = label;
				}
				UpdateBackground(clear: false);
			}

			public void UpdateBackground(bool clear)
			{
				if (clear)
				{
					base.BackgroundColor = Color.Transparent;
				}
				else
				{
					SetBinding(VisualElement.BackgroundColorProperty, _backgroundColorBinding);
				}
			}
		}

		private bool _isSelected;

		public static readonly BindableProperty OriginalBackgroundColorProperty = BindableProperty.Create("OriginalBackgroundColor", typeof(Color), typeof(DataGridRow), Color.Default, BindingMode.OneWay, null, delegate(BindableObject b, object o, object n)
		{
			((DataGridRow)b).UpdateBackground();
		});

		public static readonly BindableProperty SelectedBackgroundColorProperty = BindableProperty.Create("SelectedBackgroundColor", typeof(Color), typeof(DataGridRow), Color.Default, BindingMode.OneWay, null, delegate(BindableObject b, object o, object n)
		{
			((DataGridRow)b).UpdateBackground();
		});

		public override BindableProperty OwnerGridRowHeightProperty => DataGrid.RowHeightProperty;

		public override bool HasRowSeparator
		{
			get
			{
				DataGrid owner = _owner;
				if (owner == null || owner.GridSeparatorVisibility != DataGridSeparatorVisibility.All)
				{
					DataGrid owner2 = _owner;
					if (owner2 == null)
					{
						return false;
					}
					return owner2.GridSeparatorVisibility == DataGridSeparatorVisibility.Horizontal;
				}
				return true;
			}
		}

		public override string RowSeparatorHeightProperty => "RowSeparatorHeight";

		public override string RowSeparatorColorProperty => "RowSeparatorColor";

		public override string ColumnSeparatorWidthProperty => "ColumnSeparatorWidth";

		public override string ColumnSeparatorColorProperty => "ColumnSeparatorColor";

		public Color OriginalBackgroundColor
		{
			get
			{
				return (Color)GetValue(OriginalBackgroundColorProperty);
			}
			set
			{
				SetValue(OriginalBackgroundColorProperty, value);
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

		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				if (value != _isSelected)
				{
					_isSelected = value;
					UpdateBackground();
					base.Children.ForEach(delegate(View child)
					{
						(child as CellContent)?.UpdateBackground(value);
					});
				}
			}
		}

		public ICommand OnTapCommand
		{
			get;
			set;
		}

		public override bool HasColumnSeparator(int index)
		{
			DataGrid owner = _owner;
			if (owner == null || owner.GridSeparatorVisibility != DataGridSeparatorVisibility.All)
			{
				DataGrid owner2 = _owner;
				if (owner2 == null || owner2.GridSeparatorVisibility != DataGridSeparatorVisibility.Vertical)
				{
					return false;
				}
			}
			return index < _columns.Count - 1;
		}

		public DataGridRow(DataGrid owner)
			: base(owner)
		{
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			IsSelected = (_owner.SelectedItem != null && _owner.SelectedItem.Equals(base.BindingContext));
			if (IsSelected)
			{
				_owner.SelectedRows.Add(this);
			}
			else
			{
				_owner.SelectedRows.Remove(this);
			}
			if (base.BindingContext != null)
			{
				_columns.ForEach(delegate(DataGridColumn column, int index)
				{
					column.CalculatedCellValues.UpdateCalculatedCellTextAlignment(base.BindingContext);
				});
			}
		}

		protected override void Initialize()
		{
			base.Initialize();
			base.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(OnRowTapped),
				CommandParameter = this
			});
		}

		protected override ContentView CreateCell(DataGridColumn column, int index)
		{
			return new CellContent(column);
		}

		private void OnRowTapped(object sender)
		{
			if (OnTapCommand != null && OnTapCommand.CanExecute(sender))
			{
				OnTapCommand.Execute(sender);
			}
		}

		private void UpdateBackground()
		{
			base.BackgroundColor = (IsSelected ? SelectedBackgroundColor : OriginalBackgroundColor);
		}
	}
}
