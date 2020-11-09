using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppName.Core
{
    public class GridView : Grid, ILayoutDirectionAware
	{
		private int _realColumnCount;

		private int _realRowCount;

		private bool _rowAndColumnsInvalid = true;

		public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(GridView));

		public static BindableProperty RowCountProperty = BindableProperty.Create("RowCount", typeof(int), typeof(GridView), 0, BindingMode.OneWay, null, delegate(BindableObject bindable, object oldValue, object newValue)
		{
			((GridView)bindable).RowOrColumnCountChanged();
		});

		public static BindableProperty ColumnCountProperty = BindableProperty.Create("ColumnCount", typeof(int), typeof(GridView), 0, BindingMode.OneWay, null, delegate(BindableObject bindable, object oldValue, object newValue)
		{
			((GridView)bindable).RowOrColumnCountChanged();
		});

		public static BindableProperty ItemHeightProperty = BindableProperty.Create("ItemHeight", typeof(double), typeof(GridView), 0.0, BindingMode.OneWay, null, delegate(BindableObject bindable, object oldValue, object newValue)
		{
			((GridView)bindable).RowOrColumnCountChanged();
		});

		public static BindableProperty ItemHeightAutoProperty = BindableProperty.Create("ItemHeightAuto", typeof(bool), typeof(GridView), false, BindingMode.OneWay, null, delegate(BindableObject bindable, object oldValue, object newValue)
		{
			((GridView)bindable).RowOrColumnCountChanged();
		});

		public static BindableProperty ItemWidthProperty = BindableProperty.Create("ItemWidth", typeof(double), typeof(GridView), 0.0, BindingMode.OneWay, null, delegate(BindableObject bindable, object oldValue, object newValue)
		{
			((GridView)bindable).RowOrColumnCountChanged();
		});

		public static BindableProperty ItemWidthAutoProperty = BindableProperty.Create("ItemWidthAuto", typeof(bool), typeof(GridView), false, BindingMode.OneWay, null, delegate(BindableObject bindable, object oldValue, object newValue)
		{
			((GridView)bindable).RowOrColumnCountChanged();
		});

		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IEnumerable<object>), typeof(GridView), null, BindingMode.OneWay, null, OnItemsChanged);

		public static BindableProperty ItemClickCommandProperty = BindableProperty.Create("ItemClickCommand", typeof(ICommand), typeof(GridView), null, BindingMode.OneWay, null, OnItemClickCommandChanged);

		private LayoutDirection _layoutDirection;

		private double _lastKnownLayoutWidth;

		private double _lastKnownLayoutHeight;

		public IEnumerable<object> ItemsSource
		{
			get
			{
				return (IEnumerable<object>)GetValue(ItemsSourceProperty);
			}
			set
			{
				SetValue(ItemsSourceProperty, value);
			}
		}

		public ICommand ItemClickCommand
		{
			get
			{
				return (ICommand)GetValue(ItemClickCommandProperty);
			}
			set
			{
				SetValue(ItemClickCommandProperty, value);
			}
		}

		public DataTemplate ItemTemplate
		{
			get
			{
				return (DataTemplate)GetValue(ItemTemplateProperty);
			}
			set
			{
				SetValue(ItemTemplateProperty, value);
			}
		}

		public int RowCount
		{
			get
			{
				return (int)GetValue(RowCountProperty);
			}
			set
			{
				SetValue(RowCountProperty, value);
			}
		}

		public int ColumnCount
		{
			get
			{
				return (int)GetValue(ColumnCountProperty);
			}
			set
			{
				SetValue(ColumnCountProperty, value);
			}
		}

		public double ItemHeight
		{
			get
			{
				return (double)GetValue(ItemHeightProperty);
			}
			set
			{
				SetValue(ItemHeightProperty, value);
			}
		}

		public bool ItemHeightAuto
		{
			get
			{
				return (bool)GetValue(ItemHeightAutoProperty);
			}
			set
			{
				SetValue(ItemHeightAutoProperty, value);
			}
		}

		public double ItemWidth
		{
			get
			{
				return (double)GetValue(ItemWidthProperty);
			}
			set
			{
				SetValue(ItemWidthProperty, value);
			}
		}

		public bool ItemWidthAuto
		{
			get
			{
				return (bool)GetValue(ItemWidthAutoProperty);
			}
			set
			{
				SetValue(ItemWidthAutoProperty, value);
			}
		}

		public int RealRowCount => _realRowCount;

		public int RealColumnCount => _realColumnCount;

		public GridView()
		{
		}

		public void SetLayoutDirection(LayoutDirection layoutDirection)
		{
			if (_layoutDirection != layoutDirection)
			{
				_layoutDirection = layoutDirection;
				InvertDirection();
			}
		}

		private void InvertDirection()
		{
			if (base.Children != null)
			{
				foreach (View child in base.Children)
				{
					int column = Grid.GetColumn(child);
					Grid.SetColumn(child, RealColumnCount - column - 1);
				}
			}
		}

		private bool RowOrColumnCountChanged(bool reloadItemSource = true)
		{
			_rowAndColumnsInvalid = true;
			int num = ColumnCount;
			int num2 = RowCount;
			if (num <= 0 && num2 <= 0)
			{
				return false;
			}
			if (num <= 0 || num2 <= 0)
			{
				if (ItemsSource == null)
				{
					return false;
				}
				int num3 = 0;
				foreach (object item in ItemsSource)
				{
					_ = item;
					num3++;
				}
				if (num3 == 0)
				{
					return false;
				}
				if (num <= 0)
				{
					num = (int)Math.Ceiling((double)num3 / (double)num2);
				}
				else
				{
					num2 = (int)Math.Ceiling((double)num3 / (double)num);
				}
			}
			_rowAndColumnsInvalid = false;
			_realRowCount = num2;
			_realColumnCount = num;
			GridLength width;
			if (ItemWidth > 0.0)
			{
				width = new GridLength(ItemWidth);
			}
			else if (ItemWidthAuto)
			{
				width = GridLength.Auto;
			}
			else
			{
				int num4 = 100 / num;
				width = new GridLength(num4, GridUnitType.Star);
			}
			GridLength height;
			if (ItemHeight > 0.0)
			{
				height = new GridLength(ItemHeight);
			}
			else if (ItemHeightAuto)
			{
				height = GridLength.Auto;
			}
			else
			{
				int num5 = 100 / num2;
				height = new GridLength(num5, GridUnitType.Star);
			}
			base.ColumnDefinitions.Clear();
			for (int i = 0; i < num; i++)
			{
				base.ColumnDefinitions.Add(new ColumnDefinition
				{
					Width = width
				});
			}
			base.RowDefinitions.Clear();
			for (int j = 0; j < num2; j++)
			{
				base.RowDefinitions.Add(new RowDefinition
				{
					Height = height
				});
			}
			if (reloadItemSource && ItemsSource != null)
			{
				ReloadItemSource();
			}
			return true;
		}

		private void ReloadItemSource()
		{
			if ((_rowAndColumnsInvalid || ColumnCount <= 0 || RowCount <= 0) && !RowOrColumnCountChanged(reloadItemSource: false))
			{
				base.Children.Clear();
				return;
			}
			int realColumnCount = RealColumnCount;
			int realRowCount = RealRowCount;
			IEnumerable<object> itemsSource = ItemsSource;
			if (itemsSource != null && realColumnCount > 0 && realRowCount > 0)
			{
				int num = 0;
				int num2 = 0;
				base.Children.Clear();
				foreach (object item in itemsSource)
				{
					base.Children.Add(CreateItem(item, num, num2));
					num++;
					if (num >= realColumnCount)
					{
						num = 0;
						num2++;
						if (num2 >= realRowCount)
						{
							break;
						}
					}
				}
			}
		}

		private View CreateItem(object elem, int column, int row)
		{
			object obj;
			if (ItemTemplate != null)
			{
				DataTemplateSelector dataTemplateSelector = ItemTemplate as DataTemplateSelector;
				obj = ((dataTemplateSelector == null) ? ItemTemplate.CreateContent() : dataTemplateSelector.SelectTemplate(elem, null).CreateContent());
			}
			else
			{
				DataTemplate dataTemplate = elem as DataTemplate;
				obj = ((dataTemplate == null) ? new Label
				{
					Text = "MISSING DATA TEMPLATE",
					TextColor = Color.Red
				} : dataTemplate.CreateContent());
			}
			if (!(obj is View) && !(obj is ViewCell))
			{
				throw new InvalidOperationException($"DataTemplate content's element type is {obj.GetType().Name}, while View or ViewCell were expected");
			}
			View view = (obj is View) ? (obj as View) : ((ViewCell)obj).View;
			view.BindingContext = elem;
			if (ItemClickCommand != null)
			{
				view.GestureRecognizers.Add(new TapGestureRecognizer
				{
					Command = ItemClickCommand,
					CommandParameter = elem
				});
			}
			Grid.SetRow(view, row);
			if (_layoutDirection == LayoutDirection.Ltr)
			{
				Grid.SetColumn(view, column);
			}
			else
			{
				Grid.SetColumn(view, RealColumnCount - column - 1);
			}
			return view;
		}

		private static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
		{
			GridView gridView = (GridView)bindable;
			INotifyCollectionChanged notifyCollectionChanged = oldValue as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				notifyCollectionChanged.CollectionChanged -= gridView.OnObservableCollectionChanged;
			}
			INotifyCollectionChanged notifyCollectionChanged2 = newValue as INotifyCollectionChanged;
			if (notifyCollectionChanged2 != null)
			{
				notifyCollectionChanged2.CollectionChanged += gridView.OnObservableCollectionChanged;
			}
			gridView.ReloadItemSource();
		}

		private static void OnItemClickCommandChanged(BindableObject bindable, object oldValue, object newValue)
		{
			GridView gridView = (GridView)bindable;
			if (oldValue != null)
			{
				IGridList<View> children = gridView.Children;
				for (int i = 0; i < children.Count; i++)
				{
					IList<IGestureRecognizer> gestureRecognizers = children[i].GestureRecognizers;
					for (int j = 0; j < gestureRecognizers.Count; j++)
					{
						TapGestureRecognizer tapGestureRecognizer = gestureRecognizers[j] as TapGestureRecognizer;
						if (tapGestureRecognizer != null && tapGestureRecognizer.Command == oldValue)
						{
							gestureRecognizers.Remove(tapGestureRecognizer);
							break;
						}
					}
				}
			}
			if (newValue != null)
			{
				IGridList<View> children2 = gridView.Children;
				for (int k = 0; k < children2.Count; k++)
				{
					View view = children2[k];
					TapGestureRecognizer item = new TapGestureRecognizer
					{
						Command = (ICommand)newValue,
						CommandParameter = view.BindingContext
					};
					view.GestureRecognizers.Add(item);
				}
			}
		}

		private void OnObservableCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			ReloadItemSource();
		}

		protected override void LayoutChildren(double x, double y, double width, double height)
		{
			if (((ItemWidth <= 0.0 && !ItemWidthAuto) || (ItemHeight <= 0.0 && !ItemHeightAuto)) && (width != _lastKnownLayoutWidth || height != _lastKnownLayoutHeight))
			{
				_lastKnownLayoutWidth = width;
				_lastKnownLayoutHeight = height;
				Device.BeginInvokeOnMainThread(TouchChildrenWorkaroundForAndroidOrientationChange);
			}
			else
			{
				_lastKnownLayoutWidth = width;
				_lastKnownLayoutHeight = height;
			}
			base.LayoutChildren(x, y, width, height);
		}

		private void TouchChildrenWorkaroundForAndroidOrientationChange()
		{
			if (base.Children.Count > 0)
			{
				View bindable = base.Children[0];
				Grid.SetRowSpan(bindable, 2);
				Grid.SetRowSpan(bindable, 1);
			}
		}
	}
}
