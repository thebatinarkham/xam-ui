using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace AppName.Core
{
    internal class FixedSizeLayout : Layout<View>, ILayoutDirectionAware
	{
		internal const double UnspecifiedSizeValue = -1.0;

		internal const double DefaultItemSizeWhenUnspecified = 20.0;

		private static readonly Size UnknownSize = new Size(-1.0, -1.0);

		public static readonly BindableProperty SpacingProperty = BindableProperty.Create("Spacing", typeof(double), typeof(FixedSizeLayout), 0.0, BindingMode.OneWay, null, OnSpacingChanged);

		public static readonly BindableProperty OrientationProperty = BindableProperty.Create("Orientation", typeof(RepeaterOrientation), typeof(FixedSizeLayout), RepeaterOrientation.Horizontal, BindingMode.OneWay, null, OnOrientationChanged);

		public static readonly BindableProperty ItemSizeProperty = BindableProperty.Create("ItemSize", typeof(double), typeof(FixedSizeLayout), -1.0, BindingMode.OneWay, null, OnItemSizeChanged);

		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IList), typeof(FixedSizeLayout), null, BindingMode.OneWay, null, OnItemsSourceChanged);

		private IList<Rectangle> _layouts;

		private double _totalSize;

		private double _layoutsLastWidth;

		private double _layoutsLastHeight;

		private int _layoutsLastItemCount;

		private object[] _visibleItems;

		private View[] _visibleViews;

		private bool _viewPortSet;

		private double _viewPortCenter;

		private double _viewPortRadius;

		private int _visibleFromIndex = -1;

		private int _visibleToIndex = -1;

		private bool _usingDeaultItemSize;

		private bool _canWaitToCalculateItemSize;

		private bool _ignoreLayout;

		private Size _anyElementMeasure = UnknownSize;

		private LayoutDirection _layoutDirection;

		private Action<object> _updateSelection;

		private bool _isSelectionEnabled;

		private object _selectedItem;

		public IList ItemsSource
		{
			get
			{
				return (IList)GetValue(ItemsSourceProperty);
			}
			set
			{
				SetValue(ItemsSourceProperty, value);
			}
		}

		public double Spacing
		{
			get
			{
				return (double)GetValue(SpacingProperty);
			}
			set
			{
				SetValue(SpacingProperty, value);
			}
		}

		public RepeaterOrientation Orientation
		{
			get
			{
				return (RepeaterOrientation)GetValue(OrientationProperty);
			}
			set
			{
				SetValue(OrientationProperty, value);
			}
		}

		public double ItemSize
		{
			get
			{
				return (double)GetValue(ItemSizeProperty);
			}
			set
			{
				SetValue(ItemSizeProperty, value);
			}
		}

		public DataTemplate ItemTemplate
		{
			get;
			set;
		}

		public DataTemplate SelectedItemTemplate
		{
			get;
			set;
		}

		internal bool IsSelectionEnabled
		{
			get
			{
				return _isSelectionEnabled;
			}
			set
			{
				if (_isSelectionEnabled != value)
				{
					_isSelectionEnabled = value;
					if (!value)
					{
						SelectedItem = null;
					}
				}
			}
		}

		internal object SelectedItem
		{
			get
			{
				return _selectedItem;
			}
			set
			{
				if (_selectedItem != value)
				{
					object selectedItem = _selectedItem;
					_selectedItem = value;
					_updateSelection(value);
					OnSelectedItemChanged(selectedItem, value);
				}
			}
		}

		internal int VisibleFromIndex => _visibleFromIndex;

		public FixedSizeLayout(Action<object> updateSelection)
		{
			_updateSelection = updateSelection;
			_canWaitToCalculateItemSize = true;
			ResetLayoutCache();
		}

		public void SetLayoutDirection(LayoutDirection layoutDirection)
		{
			if (_layoutDirection != layoutDirection)
			{
				_layoutDirection = layoutDirection;
				ResetItemSource();
			}
		}

		internal void SetViewPort(double center, double radius)
		{
			_viewPortSet = true;
			_viewPortCenter = center;
			_viewPortRadius = radius;
			UpdateVisibleItems();
		}

		internal void ResetViewPort()
		{
			_viewPortSet = false;
		}

		private void UpdateVisibleItems()
		{
			if (!_viewPortSet)
			{
				return;
			}
			bool flag = false;
			IList items = GetItems();
			int indexFrom = -1;
			int indexTo = -1;
			if (!CalculateVisible(_viewPortCenter, _viewPortRadius, out indexFrom, out indexTo))
			{
				indexFrom = (indexTo = -1);
			}
			if (indexFrom == -1)
			{
				_visibleItems = null;
				_visibleViews = null;
				base.Children.Clear();
				return;
			}
			if (_visibleViews == null || indexFrom != _visibleFromIndex || indexTo != _visibleToIndex)
			{
				int num = indexTo - indexFrom + 1;
				object[] array = new object[num];
				View[] array2 = new View[num];
				int num2 = _visibleFromIndex;
				if (_visibleItems != null)
				{
					int num3 = 0;
					while (num3 < _visibleItems.Length)
					{
						if (num2 >= indexFrom && num2 <= indexTo)
						{
							int num4 = num2 - indexFrom;
							array[num4] = _visibleItems[num3];
							array2[num4] = _visibleViews[num3];
						}
						else
						{
							base.Children.Remove(_visibleViews[num3]);
						}
						num3++;
						num2++;
					}
				}
				num2 = indexFrom;
				int num5 = 0;
				while (num5 < array.Length)
				{
					if (array[num5] == null)
					{
						View item = array2[num5] = CreateViewFor(array[num5] = items[num2]);
						base.Children.Insert(num5, item);
						flag = true;
					}
					num5++;
					num2++;
				}
				_visibleFromIndex = indexFrom;
				_visibleToIndex = indexTo;
				_visibleItems = array;
				_visibleViews = array2;
			}
			if (flag)
			{
				InvalidateLayout();
			}
		}

		private View CreateViewFor(object item)
		{
			DataTemplate dataTemplate = (IsSelectionEnabled && item == SelectedItem && SelectedItemTemplate != null) ? SelectedItemTemplate : ItemTemplate;
			object obj;
			if (dataTemplate != null)
			{
				DataTemplateSelector dataTemplateSelector = dataTemplate as DataTemplateSelector;
				obj = ((dataTemplateSelector == null) ? dataTemplate.CreateContent() : dataTemplateSelector.SelectTemplate(item, null).CreateContent());
			}
			else
			{
				DataTemplate dataTemplate2 = item as DataTemplate;
				obj = ((dataTemplate2 == null) ? new Label
				{
					Text = "MISSING DATA TEMPLATE",
					TextColor = Color.Red
				} : dataTemplate2.CreateContent());
			}
			ViewCell viewCell = obj as ViewCell;
			View view = (viewCell == null) ? ((View)obj) : viewCell.View;
			view.BindingContext = item;
			view.GestureRecognizers.Add(new TapGestureRecognizer
			{
				CommandParameter = item,
				Command = new Command(OnItemTapped, CanTapItem)
			});
			return (View)obj;
		}

		private bool CalculateVisible(double center, double radius, out int indexFrom, out int indexTo)
		{
			indexFrom = (indexTo = -1);
			IList items = GetItems();
			if (items == null || items.Count == 0)
			{
				return false;
			}
			double totalSize;
			IList<Rectangle> orCalculateLayout = GetOrCalculateLayout(base.Width, base.Height, out totalSize);
			if (center < 0.0)
			{
				return false;
			}
			bool usingDefault;
			double itemSize = GetItemSize(out usingDefault);
			if (_canWaitToCalculateItemSize && usingDefault)
			{
				_canWaitToCalculateItemSize = false;
				_usingDeaultItemSize = true;
				indexFrom = 0;
				indexTo = Math.Min(0, orCalculateLayout.Count - 1);
				return true;
			}
			int val = (int)Math.Round(center / itemSize);
			val = Math.Min(Math.Max(0, val), items.Count - 1);
			Rectangle rectangle = orCalculateLayout[val];
			double num;
			double num2;
			if (Orientation == RepeaterOrientation.Horizontal)
			{
				num = rectangle.Left;
				num2 = rectangle.Right;
			}
			else
			{
				num = rectangle.Top;
				num2 = rectangle.Bottom;
			}
			if (center < num || center > num2)
			{
				if (center < num)
				{
					while (val >= 0)
					{
						val--;
						rectangle = orCalculateLayout[val];
						num = ((Orientation == RepeaterOrientation.Horizontal) ? rectangle.Left : rectangle.Top);
						if (center > num)
						{
							break;
						}
					}
				}
				else
				{
					while (val < orCalculateLayout.Count - 1)
					{
						val++;
						rectangle = orCalculateLayout[val];
						num2 = ((Orientation == RepeaterOrientation.Horizontal) ? rectangle.Right : rectangle.Bottom);
						if (center < num2)
						{
							break;
						}
					}
				}
			}
			indexFrom = 0;
			for (int num3 = val; num3 > 0; num3--)
			{
				if (center - ((Orientation == RepeaterOrientation.Horizontal) ? orCalculateLayout[num3].Left : orCalculateLayout[num3].Top) >= radius)
				{
					indexFrom = ((num3 > 0) ? (num3 - 1) : num3);
					break;
				}
			}
			indexTo = orCalculateLayout.Count - 1;
			for (int i = val; i < orCalculateLayout.Count; i++)
			{
				if (((Orientation == RepeaterOrientation.Horizontal) ? orCalculateLayout[i].Right : orCalculateLayout[i].Bottom) - center >= radius)
				{
					indexTo = ((i < orCalculateLayout.Count - 1) ? (i + 1) : i);
					break;
				}
			}
			return true;
		}

		protected override void LayoutChildren(double x, double y, double width, double height)
		{
			if (_ignoreLayout || _visibleFromIndex == -1 || _visibleViews == null)
			{
				return;
			}
			if (_usingDeaultItemSize)
			{
				_usingDeaultItemSize = false;
				GetItemSize(out bool usingDefault);
				if (!usingDefault)
				{
					UpdateVisibleItems();
					return;
				}
			}
			IList<Rectangle> orCalculateLayout = GetOrCalculateLayout(width, height);
			int num = _visibleFromIndex;
			int num2 = 0;
			while (num2 < _visibleViews.Length)
			{
				View child = _visibleViews[num2];
				Rectangle region = orCalculateLayout[num];
				Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion(child, region);
				num2++;
				num++;
			}
		}

		protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
		{
			GetOrCalculateLayout(widthConstraint, heightConstraint, out double totalSize);
			Size request = (Orientation == RepeaterOrientation.Horizontal) ? new Size(totalSize, (heightConstraint == double.PositiveInfinity) ? MeasureAnyElement().Height : heightConstraint) : new Size((widthConstraint == double.PositiveInfinity) ? MeasureAnyElement().Width : widthConstraint, totalSize);
			return new SizeRequest(request);
		}

		private Size MeasureAnyElement()
		{
			if (_anyElementMeasure == UnknownSize)
			{
				View[] visibleViews = _visibleViews;
				if (visibleViews == null || visibleViews.Length == 0)
				{
					return new Size(0.0, 0.0);
				}
				_anyElementMeasure = visibleViews[0].Measure(double.PositiveInfinity, double.PositiveInfinity).Request;
			}
			return _anyElementMeasure;
		}

		protected override void InvalidateLayout()
		{
			ResetLayoutCache();
			base.InvalidateLayout();
		}

		protected override void InvalidateMeasure()
		{
			ResetLayoutCache();
			base.InvalidateMeasure();
		}

		protected void ResetLayoutCache()
		{
			_layouts = null;
			_layoutsLastWidth = double.NegativeInfinity;
			_layoutsLastHeight = double.NegativeInfinity;
			_layoutsLastItemCount = -1;
		}

		private IList<Rectangle> GetOrCalculateLayout(double width, double height)
		{
			return GetOrCalculateLayout(width, height, out _totalSize);
		}

		private IList<Rectangle> GetOrCalculateLayout(double width, double height, out double totalSize)
		{
			int num = (ItemsSource != null) ? ItemsSource.Count : 0;
			if (_layouts == null || width != _layoutsLastWidth || height != _layoutsLastHeight || num != _layoutsLastItemCount)
			{
				_layouts = ComputeNaiveLayout(width, height, out _totalSize);
				_layoutsLastWidth = width;
				_layoutsLastHeight = height;
				_layoutsLastItemCount = num;
			}
			totalSize = _totalSize;
			return _layouts;
		}

		private List<Rectangle> ComputeNaiveLayout(double width, double height, out double totalSizeInRepeatDimension)
		{
			List<Rectangle> list = new List<Rectangle>();
			IList items = GetItems();
			totalSizeInRepeatDimension = 0.0;
			if (items != null)
			{
				double spacing = Spacing;
				double num = 0.0;
				double num2 = 0.0;
				bool flag = Orientation == RepeaterOrientation.Horizontal;
				double num3;
				double num4;
				if (flag)
				{
					num3 = GetItemSize();
					num4 = height;
				}
				else
				{
					num3 = width;
					num4 = GetItemSize();
				}
				for (int i = 0; i < items.Count; i++)
				{
					if (i > 0)
					{
						if (flag)
						{
							num += spacing;
						}
						else
						{
							num2 += spacing;
						}
					}
					list.Add(new Rectangle(num, num2, num3, num4));
					if (flag)
					{
						num += num3;
					}
					else
					{
						num2 += num4;
					}
				}
				totalSizeInRepeatDimension = ((Orientation == RepeaterOrientation.Horizontal) ? num : num2);
			}
			return list;
		}

		private static void OnSpacingChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			((FixedSizeLayout)bindable).InvalidateMeasure();
		}

		private static void OnOrientationChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			((FixedSizeLayout)bindable).InvalidateMeasure();
		}

		private static void OnItemSizeChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			((FixedSizeLayout)bindable).InvalidateMeasure();
		}

		private void VisualCollectionReset()
		{
			_visibleFromIndex = (_visibleToIndex = -1);
			_visibleItems = null;
			_visibleViews = null;
			base.Children.Clear();
		}

		private void ResetItemSource()
		{
			VisualCollectionReset();
			UpdateVisibleItems();
			InvalidateMeasure();
		}

		private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
		{
			FixedSizeLayout fixedSizeLayout = (FixedSizeLayout)bindable;
			fixedSizeLayout._anyElementMeasure = UnknownSize;
			fixedSizeLayout._canWaitToCalculateItemSize = true;
			fixedSizeLayout.ResetItemSource();
			INotifyCollectionChanged notifyCollectionChanged = oldValue as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				notifyCollectionChanged.CollectionChanged -= fixedSizeLayout.NotifyCollectionOnCollectionChanged;
			}
			INotifyCollectionChanged notifyCollectionChanged2 = newValue as INotifyCollectionChanged;
			if (notifyCollectionChanged2 != null)
			{
				notifyCollectionChanged2.CollectionChanged += fixedSizeLayout.NotifyCollectionOnCollectionChanged;
			}
		}

		private void NotifyCollectionOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (_visibleViews == null || _visibleItems == null)
			{
				ResetItemSource();
			}
			else if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				ResetItemSource();
			}
			else if (e.Action == NotifyCollectionChangedAction.Add)
			{
				if (e.NewStartingIndex == -1)
				{
					ResetItemSource();
				}
				else
				{
					AdjustVisibleItemsToSourceChange(e.NewStartingIndex, e.NewItems.Count, isAdd: true);
				}
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				if (e.OldStartingIndex == -1)
				{
					ResetItemSource();
				}
				else
				{
					AdjustVisibleItemsToSourceChange(e.OldStartingIndex, e.OldItems.Count, isAdd: false);
				}
			}
			else if (e.Action == NotifyCollectionChangedAction.Replace)
			{
				if (e.NewStartingIndex == -1 || e.OldStartingIndex == -1)
				{
					ResetItemSource();
					return;
				}
				int start = e.NewStartingIndex;
				int end = start + e.NewItems.Count - 1;
				MapItemsIndex(ItemsSource.Count, ref start, ref end);
				if ((start < _visibleFromIndex || start > _visibleToIndex) && (end < _visibleFromIndex || end > _visibleToIndex))
				{
					return;
				}
				int num = 0;
				int num2 = start;
				while (num2 <= end)
				{
					if (num2 >= _visibleFromIndex && num2 <= _visibleToIndex)
					{
						int num3 = num2 - _visibleFromIndex;
						_ = _visibleViews[num3];
						base.Children.RemoveAt(num3);
						object obj = e.NewItems[num];
						View view = CreateViewFor(obj);
						base.Children.Insert(num3, view);
						_visibleViews[num3] = view;
						_visibleItems[num3] = obj;
					}
					num2++;
					num++;
				}
				InvalidateLayout();
			}
			else if (e.Action == NotifyCollectionChangedAction.Move)
			{
				ResetItemSource();
			}
		}

		private void AdjustVisibleItemsToSourceChange(int firstIndex, int affectedItemsCount, bool isAdd)
		{
			int end = firstIndex + affectedItemsCount - 1;
			bool flag = !isAdd;
			double itemSize = GetItemSize();
			MapItemsIndex(ItemsSource.Count + (flag ? affectedItemsCount : 0), ref firstIndex, ref end);
			if (flag && end < _visibleFromIndex)
			{
				_viewPortCenter -= (double)affectedItemsCount * (itemSize + Spacing);
				_visibleFromIndex -= affectedItemsCount;
				_visibleToIndex -= affectedItemsCount;
				InvalidateMeasure();
			}
			else if (isAdd && firstIndex < _visibleFromIndex)
			{
				_viewPortCenter += (double)affectedItemsCount * (itemSize + Spacing);
				_visibleFromIndex += affectedItemsCount;
				_visibleToIndex += affectedItemsCount;
				InvalidateMeasure();
			}
			else if (firstIndex <= _visibleToIndex)
			{
				try
				{
					_ignoreLayout = true;
					if (firstIndex > _visibleFromIndex)
					{
						if (isAdd || (flag && firstIndex + affectedItemsCount > _visibleToIndex))
						{
							int num = firstIndex - _visibleFromIndex;
							int index = firstIndex - _visibleFromIndex;
							int num2 = _visibleToIndex - firstIndex + 1;
							View[] array = new View[num];
							object[] array2 = new object[num];
							Array.Copy(_visibleViews, array, num);
							Array.Copy(_visibleItems, array2, num);
							_visibleToIndex = firstIndex - 1;
							_visibleItems = array2;
							_visibleViews = array;
							for (int i = 0; i < num2; i++)
							{
								base.Children.RemoveAt(index);
							}
						}
						else
						{
							int num3 = _visibleItems.Length - affectedItemsCount;
							int num4 = firstIndex - _visibleFromIndex;
							int num5 = num4 + affectedItemsCount;
							View[] array3 = new View[num3];
							object[] array4 = new object[num3];
							Array.Copy(_visibleViews, array3, num4);
							Array.Copy(_visibleViews, num5, array3, num4, _visibleViews.Length - num5);
							Array.Copy(_visibleItems, array4, num4);
							Array.Copy(_visibleItems, num5, array4, num4, _visibleViews.Length - num5);
							_visibleToIndex -= affectedItemsCount;
							_visibleItems = array4;
							_visibleViews = array3;
							for (int j = 0; j < affectedItemsCount; j++)
							{
								base.Children.RemoveAt(num4);
							}
						}
					}
					else if (end < _visibleToIndex)
					{
						int num6 = _visibleToIndex - end;
						int num7 = _visibleItems.Length - num6;
						View[] array5 = new View[num6];
						object[] array6 = new object[num6];
						if (isAdd)
						{
							Array.Copy(_visibleViews, array5, num6);
							Array.Copy(_visibleItems, array6, num6);
							_visibleFromIndex += affectedItemsCount;
							for (int k = 0; k < num7; k++)
							{
								base.Children.RemoveAt(base.Children.Count - 1);
							}
						}
						else
						{
							Array.Copy(_visibleViews, num7, array5, 0, num6);
							Array.Copy(_visibleItems, num7, array6, 0, num6);
							_visibleFromIndex = firstIndex;
							_visibleToIndex -= affectedItemsCount;
							for (int l = 0; l < num7; l++)
							{
								base.Children.RemoveAt(0);
							}
						}
						_visibleItems = array6;
						_visibleViews = array5;
					}
					else
					{
						VisualCollectionReset();
					}
				}
				finally
				{
					_ignoreLayout = false;
				}
				UpdateVisibleItems();
				InvalidateMeasure();
			}
			else
			{
				UpdateVisibleItems();
				InvalidateMeasure();
			}
		}

		private void MapItemsIndex(int itemsCount, ref int start, ref int end)
		{
			if (_layoutDirection != 0 && Orientation != RepeaterOrientation.Vertical)
			{
				int num = itemsCount - end - 1;
				end = itemsCount - start - 1;
				start = num;
			}
		}

		private IList GetItems()
		{
			if (ItemsSource == null)
			{
				return null;
			}
			if (_layoutDirection != 0 && Orientation != RepeaterOrientation.Vertical)
			{
				return new ReadonlyReverseListAdapter(ItemsSource);
			}
			return ItemsSource;
		}

		private double GetItemSize(out bool usingDefault)
		{
			usingDefault = false;
			if (ItemSize != -1.0)
			{
				return ItemSize;
			}
			Size size = MeasureAnyElement();
			if (Orientation == RepeaterOrientation.Horizontal)
			{
				if (size.Width <= 0.0)
				{
					usingDefault = true;
					return 20.0;
				}
				return size.Width;
			}
			if (size.Height <= 0.0)
			{
				usingDefault = true;
				return 20.0;
			}
			return size.Height;
		}

		private double GetItemSize()
		{
			bool usingDefault;
			return GetItemSize(out usingDefault);
		}

		private void OnSelectedItemChanged(object oldSelection, object newSelection)
		{
			if (_visibleItems == null)
			{
				return;
			}
			for (int i = 0; i < _visibleItems.Length; i++)
			{
				object obj = _visibleItems[i];
				if (obj == oldSelection || obj == newSelection)
				{
					base.Children.RemoveAt(i);
					_visibleViews[i] = CreateViewFor(obj);
					base.Children.Insert(i, _visibleViews[i]);
				}
			}
		}

		private void OnItemTapped(object item)
		{
			SelectedItem = item;
		}

		private bool CanTapItem(object item)
		{
			return IsSelectionEnabled;
		}
	}
}
