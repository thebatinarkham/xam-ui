using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace AppName.Core
{
	internal abstract class DataGridRowBase : Grid, IDisposable
	{
		private class Separator : BoxView
		{
			protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
			{
				double width = (!double.IsInfinity(widthConstraint)) ? widthConstraint : ((base.WidthRequest >= 0.0) ? base.WidthRequest : 1.0);
				double height = (!double.IsInfinity(heightConstraint)) ? heightConstraint : ((base.HeightRequest >= 0.0) ? base.HeightRequest : 1.0);
				return new SizeRequest(new Size(width, height));
			}
		}

		protected BoxView _rowSeparator;

		protected IList<BoxView> _columnSeparators;

		protected readonly IList<DataGridColumn> _columns;

		protected readonly DataGrid _owner;

		public abstract BindableProperty OwnerGridRowHeightProperty
		{
			get;
		}

		public abstract bool HasRowSeparator
		{
			get;
		}

		public abstract string RowSeparatorHeightProperty
		{
			get;
		}

		public abstract string RowSeparatorColorProperty
		{
			get;
		}

		public abstract string ColumnSeparatorWidthProperty
		{
			get;
		}

		public abstract string ColumnSeparatorColorProperty
		{
			get;
		}

		public abstract bool HasColumnSeparator(int index);

		protected DataGridRowBase(DataGrid owner)
		{
			_owner = owner;
			_columns = owner.ColumnDefinitions;
			Initialize();
		}

		protected virtual void Initialize()
		{
			_rowSeparator = null;
			_columnSeparators = new List<BoxView>();
			base.RowSpacing = 0.0;
			base.ColumnSpacing = 0.0;
			base.RowDefinitions.Add(new RowDefinition());
			_owner.PropertyChanged += OnGridPropertyChanged;
			_columns.ForEach(CreateColumn);
			UpdateHeight();
			UpdateRowSeparator();
		}

		protected void UpdateHeight()
		{
			double num = (double)_owner.GetValue(OwnerGridRowHeightProperty);
			base.RowDefinitions[0].Height = ((num > 0.0) ? new GridLength(num, GridUnitType.Absolute) : GridLength.Auto);
		}

		protected void UpdateRowSeparator()
		{
			if (HasRowSeparator)
			{
				if (base.RowDefinitions.Count == 2)
				{
					RemoveRowSeparator();
				}
				AddRowSeparator();
			}
			else
			{
				RemoveRowSeparator();
			}
		}

		private void AddRowSeparator()
		{
			RowDefinition rowDefinition = new RowDefinition();
			rowDefinition.SetBinding(RowDefinition.HeightProperty, new Binding(RowSeparatorHeightProperty, BindingMode.Default, null, null, null, _owner));
			base.RowDefinitions.Add(rowDefinition);
			_rowSeparator = new Separator
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			_rowSeparator.SetBinding(VisualElement.HeightRequestProperty, new Binding(RowSeparatorHeightProperty, BindingMode.Default, null, null, null, _owner));
			_rowSeparator.SetBinding(VisualElement.BackgroundColorProperty, new Binding(RowSeparatorColorProperty, BindingMode.Default, null, null, null, _owner));
			Grid.SetRow(_rowSeparator, 1);
			if (base.ColumnDefinitions.Count > 0)
			{
				Grid.SetColumnSpan(_rowSeparator, base.ColumnDefinitions.Count);
			}
			base.Children.Add(_rowSeparator);
		}

		private void RemoveRowSeparator()
		{
			if (base.RowDefinitions.Count == 2)
			{
				base.RowDefinitions.RemoveAt(1);
				if (_rowSeparator != null)
				{
					base.Children.Remove(_rowSeparator);
					_rowSeparator = null;
				}
			}
		}

		private void AddColumnSeparator()
		{
			ColumnDefinition columnDefinition = new ColumnDefinition();
			columnDefinition.SetBinding(ColumnDefinition.WidthProperty, new Binding(ColumnSeparatorWidthProperty, BindingMode.Default, null, null, null, _owner));
			base.ColumnDefinitions.Add(columnDefinition);
			Separator separator = new Separator
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			separator.SetBinding(VisualElement.BackgroundColorProperty, new Binding(ColumnSeparatorColorProperty, BindingMode.Default, null, null, null, _owner));
			separator.SetBinding(VisualElement.WidthRequestProperty, new Binding(ColumnSeparatorWidthProperty, BindingMode.Default, null, null, null, _owner));
			_columnSeparators.Add(separator);
			base.Children.Add(separator, base.ColumnDefinitions.Count - 1, 0);
		}

		private void RemoveColumnSeparators()
		{
			for (int i = 0; i < _columnSeparators.Count; i++)
			{
				base.ColumnDefinitions.RemoveAt(i + 1);
				BoxView boxView = _columnSeparators.ElementAt(i);
				if (boxView != null)
				{
					base.Children.Remove(boxView);
				}
			}
		}

		protected virtual void OnGridPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == OwnerGridRowHeightProperty.PropertyName)
			{
				UpdateHeight();
			}
			else if (e.PropertyName == RowSeparatorHeightProperty)
			{
				UpdateRowSeparator();
			}
		}

		protected abstract ContentView CreateCell(DataGridColumn column, int index);

		private void CreateColumn(DataGridColumn column, int index)
		{
			base.ColumnDefinitions.Add(new ColumnDefinition
			{
				Width = column.ColumnWidth
			});
			ContentView view = CreateCell(column, index);
			base.Children.Add(view, base.ColumnDefinitions.Count - 1, 0);
			if (HasColumnSeparator(index))
			{
				AddColumnSeparator();
			}
		}

		public virtual void Dispose()
		{
			_owner.PropertyChanged -= OnGridPropertyChanged;
			for (int i = 0; i < base.Children.Count; i++)
			{
				(base.Children[i] as IDisposable)?.Dispose();
			}
		}
	}
}
