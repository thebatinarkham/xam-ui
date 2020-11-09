using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace AppName.Core
{
    public class WrapPanel : Layout<View>, IDisposable, ILayoutDirectionAware
	{
		public static readonly BindableProperty PositionProperty = BindableProperty.CreateAttached("Position", typeof(WrapPanelPosition), typeof(WrapPanel), WrapPanelPosition.Start);

		private static readonly BindableProperty IsMarginMirroredProperty = BindableProperty.CreateAttached("IsMarginMirrored", typeof(bool), typeof(WrapPanel), false);

		private readonly Dictionary<View, SizeRequest> _layoutCache = new Dictionary<View, SizeRequest>();

		private readonly HashSet<View> _generatedChildren;

		private readonly List<View> _sortedChildren;

		private readonly List<double> _rowsHeight;

		private LayoutDirection _layoutDirection;

		public static readonly BindableProperty ItemSeparatorTemplateProperty = BindableProperty.Create("ItemSeparatorTemplate", typeof(DataTemplate), typeof(WrapPanel));

		public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create("ItemSeparatorTemplate", typeof(DataTemplate), typeof(WrapPanel));

		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IList), typeof(WrapPanel));

		public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create("RowSpacing", typeof(double), typeof(WrapPanel), 6.0, BindingMode.OneWay, null, delegate(BindableObject bindable, object oldvalue, object newvalue)
		{
			((WrapPanel)bindable).ForceLayout();
		});

		public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create("ColumnSpacing", typeof(double), typeof(WrapPanel), 0.0, BindingMode.OneWay, null, delegate(BindableObject bindable, object oldvalue, object newvalue)
		{
			((WrapPanel)bindable).ForceLayout();
		});

		public static readonly BindableProperty IgnoreMarginsWhenMirroringProperty = BindableProperty.Create("ItemsSource", typeof(bool), typeof(WrapPanel), false);

		public DataTemplate ItemSeparatorTemplate
		{
			get
			{
				return (DataTemplate)GetValue(ItemSeparatorTemplateProperty);
			}
			set
			{
				SetValue(ItemSeparatorTemplateProperty, value);
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

		public double RowSpacing
		{
			get
			{
				return (double)GetValue(RowSpacingProperty);
			}
			set
			{
				SetValue(RowSpacingProperty, value);
			}
		}

		public double ColumnSpacing
		{
			get
			{
				return (double)GetValue(ColumnSpacingProperty);
			}
			set
			{
				SetValue(ColumnSpacingProperty, value);
			}
		}

		public bool IgnoreMarginsWhenMirroring
		{
			get
			{
				return (bool)GetValue(IgnoreMarginsWhenMirroringProperty);
			}
			set
			{
				SetValue(IgnoreMarginsWhenMirroringProperty, value);
			}
		}

		private double CalculatedSpacing
		{
			get
			{
				if (ItemSeparatorTemplate != null)
				{
					return 0.0;
				}
				return ColumnSpacing;
			}
		}

		public static WrapPanelPosition GetPosition(BindableObject bo)
		{
			return (WrapPanelPosition)bo.GetValue(PositionProperty);
		}

		public static void SetPosition(BindableObject bo, WrapPanelPosition value)
		{
			bo.SetValue(PositionProperty, value);
		}

		private static bool GetIsMarginMirrored(BindableObject bo)
		{
			return (bool)bo.GetValue(IsMarginMirroredProperty);
		}

		private static void SetIsMarginMirrored(BindableObject bo, bool value)
		{
			bo.SetValue(IsMarginMirroredProperty, value);
		}

		public WrapPanel()
		{
			_sortedChildren = new List<View>();
			_layoutCache = new Dictionary<View, SizeRequest>();
			_generatedChildren = new HashSet<View>();
			_rowsHeight = new List<double>();
			base.PropertyChanged += WrapPanelPropertyChanged;
			base.PropertyChanging += WrapPanelPropertyChanging;
		}

		public void SetLayoutDirection(LayoutDirection layoutDirection)
		{
			if (_layoutDirection != layoutDirection)
			{
				_layoutDirection = layoutDirection;
				ForceLayout();
			}
		}

		protected override void OnChildAdded(Element child)
		{
			base.OnChildAdded(child);
			View view = child as View;
			if (view != null && !_sortedChildren.Contains(view))
			{
				int index = Math.Min(base.Children.IndexOf(view), _sortedChildren.Count);
				_sortedChildren.Insert(index, view);
			}
		}

		protected override void OnChildRemoved(Element child)
		{
			base.OnChildRemoved(child);
			View view = child as View;
			if (view != null)
			{
				_sortedChildren.Remove(view);
			}
		}

		protected override void OnChildMeasureInvalidated()
		{
			base.OnChildMeasureInvalidated();
			_layoutCache.Clear();
		}

		protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
		{
			if (base.WidthRequest > 0.0)
			{
				widthConstraint = Math.Min(widthConstraint, base.WidthRequest);
			}
			if (base.HeightRequest > 0.0)
			{
				heightConstraint = Math.Min(heightConstraint, base.HeightRequest);
			}
			double widthConstraint2 = double.IsPositiveInfinity(widthConstraint) ? double.PositiveInfinity : Math.Max(0.0, widthConstraint);
			double heightConstraint2 = double.IsPositiveInfinity(heightConstraint) ? double.PositiveInfinity : Math.Max(0.0, heightConstraint);
			return MeasureRows(widthConstraint2, heightConstraint2);
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);
			if (width > 0.0 && height > 0.0)
			{
				InvalidateMeasure();
			}
		}

		protected override void LayoutChildren(double x, double y, double width, double height)
		{
			if (_rowsHeight.Count != 0)
			{
				double num = y;
				double num2 = (_layoutDirection == LayoutDirection.Ltr) ? x : width;
				bool flag = true;
				int num3 = 0;
				double num4 = _rowsHeight[num3];
				foreach (View item in _sortedChildren.Where((View c) => c.IsVisible))
				{
					SizeRequest sizeRequest = MeasureChild(item, width, height);
					if (!sizeRequest.Request.IsZero)
					{
						double width2 = sizeRequest.Request.Width;
						_ = sizeRequest.Request.Height;
						if (!flag && ((_layoutDirection == LayoutDirection.Ltr) ? (num2 + width2 > width) : (num2 - width2 < x)))
						{
							flag = true;
							num2 = ((_layoutDirection == LayoutDirection.Ltr) ? x : width);
							num += num4 + RowSpacing;
							num3++;
							if (num3 < _rowsHeight.Count)
							{
								num4 = _rowsHeight[num3];
							}
						}
						else
						{
							flag = false;
						}
						Rectangle region = new Rectangle((_layoutDirection == LayoutDirection.Ltr) ? num2 : (num2 - width2), num, width2, num4);
						UpdateMargin(item);
						Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion(item, region);
						num2 = ((_layoutDirection != 0) ? (num2 - (region.Width + CalculatedSpacing)) : (num2 + (region.Width + CalculatedSpacing)));
					}
				}
			}
		}

		private void UpdateMargin(View child)
		{
			if (!IgnoreMarginsWhenMirroring)
			{
				bool isMarginMirrored = GetIsMarginMirrored(child);
				if ((isMarginMirrored && _layoutDirection == LayoutDirection.Ltr) || (!isMarginMirrored && _layoutDirection == LayoutDirection.Rtl))
				{
					child.Margin = new Thickness(child.Margin.Right, child.Margin.Top, child.Margin.Left, child.Margin.Bottom);
					SetIsMarginMirrored(child, !isMarginMirrored);
				}
			}
		}

		private void WrapPanelPropertyChanging(object sender, Xamarin.Forms.PropertyChangingEventArgs e)
		{
			if (e.PropertyName == ItemsSourceProperty.PropertyName)
			{
				INotifyCollectionChanged notifyCollectionChanged = ItemsSource as INotifyCollectionChanged;
				if (notifyCollectionChanged != null)
				{
					notifyCollectionChanged.CollectionChanged -= ItemsCollectionChanged;
				}
			}
		}

		private void WrapPanelPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == ItemsSourceProperty.PropertyName)
			{
				INotifyCollectionChanged notifyCollectionChanged = ItemsSource as INotifyCollectionChanged;
				if (notifyCollectionChanged != null)
				{
					notifyCollectionChanged.CollectionChanged += ItemsCollectionChanged;
				}
				RecreateChildren();
			}
		}

		private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			RecreateChildren();
		}

		private void RecreateChildren()
		{
			_layoutCache.Clear();
			_sortedChildren.Clear();
			if (_generatedChildren.Count > 0)
			{
				int num = 0;
				while (num < base.Children.Count)
				{
					View view = base.Children[num];
					if (_generatedChildren.Contains(view))
					{
						view.BindingContext = null;
						base.Children.RemoveAt(num);
					}
					else
					{
						num++;
					}
				}
				_generatedChildren.Clear();
			}
			int num2 = 0;
			for (int i = 0; i < base.Children.Count; i++)
			{
				View view2 = base.Children[i];
				if (GetPosition(view2) == WrapPanelPosition.Start)
				{
					_sortedChildren.Insert(num2, view2);
					num2++;
				}
				else
				{
					_sortedChildren.Add(view2);
				}
			}
			for (int j = 0; j < ItemsSource.Count; j++)
			{
				View view3 = null;
				DataTemplateSelector dataTemplateSelector = ItemTemplate as DataTemplateSelector;
				view3 = ((dataTemplateSelector != null) ? ((View)dataTemplateSelector.SelectTemplate(ItemsSource[j], null).CreateContent()) : ((ItemTemplate == null) ? new Label
				{
					Text = "MISSING DATA TEMPLATE",
					TextColor = Color.Red
				} : ((View)ItemTemplate.CreateContent())));
				view3.BindingContext = ItemsSource[j];
				_generatedChildren.Add(view3);
				_sortedChildren.Insert(num2++, view3);
				base.Children.Add(view3);
				if (ItemSeparatorTemplate != null && j < ItemsSource.Count - 1)
				{
					view3 = (View)ItemSeparatorTemplate.CreateContent();
					_generatedChildren.Add(view3);
					_sortedChildren.Insert(num2++, view3);
					base.Children.Add(view3);
				}
			}
		}

		private SizeRequest MeasureRows(double widthConstraint, double heightConstraint)
		{
			_rowsHeight.Clear();
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double height = 0.0;
			int nextRowStartingIndex = 0;
			SizeRequest rowSize;
			while (MeasureNextRow(nextRowStartingIndex, widthConstraint, heightConstraint, out rowSize, out nextRowStartingIndex))
			{
				_rowsHeight.Add(rowSize.Request.Height);
				num3 = Math.Max(rowSize.Request.Width, num3);
				num = Math.Max(rowSize.Minimum.Width, num);
				num2 = Math.Max(rowSize.Minimum.Height, num2);
			}
			if (_rowsHeight.Count > 0)
			{
				height = _rowsHeight.Sum() + RowSpacing * (double)(_rowsHeight.Count - 1);
			}
			return new SizeRequest(new Size(num3, height), new Size(num, num2));
		}

		private bool MeasureNextRow(int startIndex, double widthConstraint, double heightConstraint, out SizeRequest rowSize, out int nextRowStartingIndex)
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			nextRowStartingIndex = _sortedChildren.Count;
			if (startIndex >= 0 && startIndex < _sortedChildren.Count)
			{
				bool flag = true;
				for (int i = startIndex; i < _sortedChildren.Count; i++)
				{
					View view = _sortedChildren[i];
					if (!view.IsVisible)
					{
						continue;
					}
					SizeRequest sizeRequest = MeasureChild(view, widthConstraint, heightConstraint);
					if (!sizeRequest.Request.IsZero)
					{
						double num5 = num2 + sizeRequest.Request.Width;
						if (flag)
						{
							num5 += CalculatedSpacing;
						}
						if (num5 > widthConstraint && !flag)
						{
							nextRowStartingIndex = i;
							break;
						}
						flag = false;
						num = Math.Max(num, sizeRequest.Request.Height);
						num2 = num5;
						num3 = Math.Max(num3, sizeRequest.Minimum.Height);
						num4 = Math.Max(num4, sizeRequest.Minimum.Width);
					}
				}
			}
			rowSize = new SizeRequest(new Size(num2, num), new Size(num4, num3));
			return num2 > 0.0;
		}

		private SizeRequest MeasureChild(View child, double width, double height)
		{
			if (!_layoutCache.TryGetValue(child, out SizeRequest value))
			{
				value = child.Measure(width, height, MeasureFlags.IncludeMargins);
				_layoutCache[child] = value;
			}
			return value;
		}

		public void Dispose()
		{
			base.PropertyChanged -= WrapPanelPropertyChanged;
			base.PropertyChanging -= WrapPanelPropertyChanging;
			INotifyCollectionChanged notifyCollectionChanged = ItemsSource as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				notifyCollectionChanged.CollectionChanged -= ItemsCollectionChanged;
			}
		}
	}
}
