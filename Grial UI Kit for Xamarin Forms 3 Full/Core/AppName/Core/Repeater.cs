using System;
using System.Collections;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppName.Core
{
    public class Repeater : ContentView, ILayoutDirectionAware
	{
		public EventHandler<RepeaterSelectedItemChangedEventArgs> ItemSelected;

		public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create("SelectedItem", typeof(object), typeof(Repeater), null, BindingMode.TwoWay, null, OnSelectedItemChanged);

		public static readonly BindableProperty ItemSelectedCommandProperty = BindableProperty.Create("ItemSelectedCommand", typeof(ICommand), typeof(Repeater));

		public static readonly BindableProperty SelectionModeProperty = BindableProperty.Create("SelectionMode", typeof(RepeaterSelectionMode), typeof(Repeater), RepeaterSelectionMode.None, BindingMode.OneWay, null, OnSelectionModeChanged);

		public static readonly BindableProperty InitialSelectionProperty = BindableProperty.Create("InitialSelection", typeof(RepeaterInitialSelection), typeof(Repeater), RepeaterInitialSelection.Empty);

		public static readonly BindableProperty ScrollPaddingProperty = BindableProperty.Create("ScrollPadding", typeof(Thickness), typeof(Repeater), default(Thickness), BindingMode.OneWay, null, OnScrollPaddingChanged);

		public static readonly BindableProperty SpacingProperty = BindableProperty.Create("Spacing", typeof(double), typeof(Repeater), 0.0, BindingMode.OneWay, null, OnSpacingChanged);

		public static readonly BindableProperty ItemSizeProperty = BindableProperty.Create("ItemSize", typeof(double), typeof(Repeater), -1.0, BindingMode.OneWay, null, OnItemSizeChanged);

		public static readonly BindableProperty OrientationProperty = BindableProperty.Create("Orientation", typeof(RepeaterOrientation), typeof(FixedSizeLayout), RepeaterOrientation.Horizontal, BindingMode.OneWay, null, OnOrientationChanged);

		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IList), typeof(Repeater), null, BindingMode.OneWay, null, OnItemSourceChanged);

		public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(Repeater), null, BindingMode.OneWay, null, OnItemTemplateChanged);

		public static readonly BindableProperty SelectedItemTemplateProperty = BindableProperty.Create("SelectedItemTemplate", typeof(DataTemplate), typeof(Repeater), null, BindingMode.OneWay, null, OnSelectedItemTemplateChanged);

		public static readonly BindableProperty ScrollPositionProperty = BindableProperty.Create("ScrollPosition", typeof(double), typeof(Repeater), 0.0, BindingMode.OneWay, null, OnScrollPositionChanged);

		public static readonly BindableProperty ScrollBarVisibilityProperty = BindableProperty.Create("ScrollBarVisibility", typeof(ScrollBarVisibility), typeof(Repeater), ScrollBarVisibility.Default, BindingMode.OneWay, null, delegate(BindableObject s, object o, object n)
		{
			((Repeater)s).UpdateScrollBarVisibility();
		});

		private readonly ScrollView _scrollView;

		private readonly FixedSizeLayout _layout;

		private Grid _scrollViewerPaddingGrid;

		private double _lastReportedSize = -1.0;

		private bool _userInitiatedScrollInProgress;

		private LayoutDirection _layoutDirection;

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

		public RepeaterSelectionMode SelectionMode
		{
			get
			{
				return (RepeaterSelectionMode)GetValue(SelectionModeProperty);
			}
			set
			{
				SetValue(SelectionModeProperty, value);
			}
		}

		public RepeaterInitialSelection InitialSelection
		{
			get
			{
				return (RepeaterInitialSelection)GetValue(InitialSelectionProperty);
			}
			set
			{
				SetValue(InitialSelectionProperty, value);
			}
		}

		public ScrollBarVisibility ScrollBarVisibility
		{
			get
			{
				return (ScrollBarVisibility)GetValue(ScrollBarVisibilityProperty);
			}
			set
			{
				SetValue(ScrollBarVisibilityProperty, value);
			}
		}

		public Thickness ScrollPadding
		{
			get
			{
				return (Thickness)GetValue(ScrollPaddingProperty);
			}
			set
			{
				SetValue(ScrollPaddingProperty, value);
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

		public DataTemplate SelectedItemTemplate
		{
			get
			{
				return (DataTemplate)GetValue(SelectedItemTemplateProperty);
			}
			set
			{
				SetValue(SelectedItemTemplateProperty, value);
			}
		}

		public double ScrollPosition
		{
			get
			{
				return (double)GetValue(ScrollPositionProperty);
			}
			set
			{
				SetValue(ScrollPositionProperty, value);
			}
		}

		private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((Repeater)bindable)._layout.SelectedItem = newValue;
		}

		private static void OnSelectionModeChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((Repeater)bindable)._layout.IsSelectionEnabled = ((RepeaterSelectionMode)newValue == RepeaterSelectionMode.Single);
		}

		public Repeater()
		{
			_scrollView = new ScrollView
			{
				Orientation = ((Orientation == RepeaterOrientation.Horizontal) ? ScrollOrientation.Horizontal : ScrollOrientation.Vertical)
			};
			UpdateScrollBarVisibility();
			_layout = new FixedSizeLayout(UpdateSelection);
			Rtl.SetMirrorBehavior(_layout, MirrorBehavior.SkipSelf);
			_layout.SetLayoutDirection(_layoutDirection);
			_scrollView.Content = _layout;
			_scrollView.Scrolled += OnScrollHandler;
			base.Content = _scrollView;
		}

		protected override async void LayoutChildren(double x, double y, double width, double height)
		{
			_scrollView.Layout(new Rectangle(x, y, width, height));
			double num = width;
			double num2 = height;
			if (Orientation == RepeaterOrientation.Horizontal)
			{
				num = double.PositiveInfinity;
				num2 -= ScrollPadding.VerticalThickness;
			}
			else
			{
				num2 = double.PositiveInfinity;
				num -= ScrollPadding.HorizontalThickness;
			}
			SizeRequest sizeRequest = _layout.Measure(num, num2);
			_layout.Layout(new Rectangle(x + ScrollPadding.Left, y + ScrollPadding.Top, sizeRequest.Request.Width, sizeRequest.Request.Height));
			double num3 = (Orientation == RepeaterOrientation.Horizontal) ? width : height;
			if (Math.Abs(_lastReportedSize - num3) > 0.01)
			{
				_lastReportedSize = num3;
				UpdateViewport(width, height, ScrollPosition);
			}
			await UpdateScrollPosition();
		}

		protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
		{
			SizeRequest sizeRequest = _layout.Measure(widthConstraint, heightConstraint);
			double num = Math.Min(sizeRequest.Request.Width, widthConstraint);
			double num2 = Math.Min(sizeRequest.Request.Height, heightConstraint);
			if (Orientation == RepeaterOrientation.Horizontal)
			{
				if (num > 0.0)
				{
					num2 += ScrollPadding.VerticalThickness;
					if (num2 == 0.0)
					{
						num2 = 0.01;
					}
				}
			}
			else if (num2 > 0.0)
			{
				num += ScrollPadding.HorizontalThickness;
				if (num == 0.0)
				{
					num = 0.01;
				}
			}
			return new SizeRequest(new Size(num, num2));
		}

		public void SetLayoutDirection(LayoutDirection layoutDirection)
		{
			_layoutDirection = layoutDirection;
			_layout?.SetLayoutDirection(layoutDirection);
		}

		private void OnScrollHandler(object sender, ScrolledEventArgs e)
		{
			_userInitiatedScrollInProgress = true;
			double scrollPosition = (Orientation == RepeaterOrientation.Horizontal) ? e.ScrollX : e.ScrollY;
			UpdateViewport(base.Width, base.Height, scrollPosition);
			ScrollPosition = scrollPosition;
			_userInitiatedScrollInProgress = false;
		}

		private void UpdateViewport(double width, double height, double scrollPosition)
		{
			double num;
			double center;
			if (Orientation == RepeaterOrientation.Horizontal)
			{
				num = width / 2.0;
				center = num + scrollPosition - ScrollPadding.Left;
			}
			else
			{
				num = height / 2.0;
				center = num + scrollPosition - ScrollPadding.Top;
			}
			_layout.SetViewPort(center, num);
		}

		private async Task UpdateScrollPosition()
		{
			if (_userInitiatedScrollInProgress)
			{
				return;
			}
			if (Orientation == RepeaterOrientation.Horizontal)
			{
				if (!(Math.Abs(ScrollPosition - _scrollView.ScrollX) < 0.01))
				{
					await _scrollView.ScrollToAsync(ScrollPosition, 0.0, animated: false);
				}
			}
			else if (!(Math.Abs(ScrollPosition - _scrollView.ScrollY) < 0.01))
			{
				await _scrollView.ScrollToAsync(0.0, ScrollPosition, animated: false);
			}
		}

		private static void OnScrollPaddingChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			((Repeater)bindable).UpdateItemsScrollPadding((Thickness)newvalue);
		}

		private static void OnSpacingChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			((Repeater)bindable)._layout.Spacing = (double)newvalue;
		}

		private static void OnItemSizeChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			((Repeater)bindable)._layout.ItemSize = (double)newvalue;
		}

		private static void OnOrientationChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			Repeater repeater = (Repeater)bindable;
			repeater._layout.Orientation = (RepeaterOrientation)newvalue;
			repeater._scrollView.Orientation = ((repeater.Orientation == RepeaterOrientation.Horizontal) ? ScrollOrientation.Horizontal : ScrollOrientation.Vertical);
		}

		private static async void OnScrollPositionChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			await((Repeater)bindable).UpdateScrollPosition();
		}

		private static void OnItemTemplateChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			((Repeater)bindable)._layout.ItemTemplate = (DataTemplate)newvalue;
		}

		private static void OnSelectedItemTemplateChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			((Repeater)bindable)._layout.SelectedItemTemplate = (DataTemplate)newvalue;
		}

		private static void OnItemSourceChanged(BindableObject bindable, object oldValue, object newValue)
		{
			Repeater repeater = (Repeater)bindable;
			repeater.ScrollPosition = 0.0;
			IList list = (IList)newValue;
			repeater._layout.ItemsSource = list;
			repeater.InvalidateMeasure();
			INotifyCollectionChanged notifyCollectionChanged = oldValue as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				notifyCollectionChanged.CollectionChanged -= repeater.NotifyCollectionOnCollectionChanged;
			}
			INotifyCollectionChanged notifyCollectionChanged2 = newValue as INotifyCollectionChanged;
			if (notifyCollectionChanged2 != null)
			{
				notifyCollectionChanged2.CollectionChanged += repeater.NotifyCollectionOnCollectionChanged;
			}
			if (repeater.InitialSelection == RepeaterInitialSelection.First)
			{
				repeater._layout.SelectedItem = ((list != null && list.Count > 0) ? list[0] : null);
			}
			else
			{
				repeater._layout.SelectedItem = null;
			}
		}

		private void NotifyCollectionOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				ScrollPosition = 0.0;
				InvalidateMeasure();
				InvalidateLayout();
			}
			else if (e.Action == NotifyCollectionChangedAction.Add)
			{
				int newStartingIndex = e.NewStartingIndex;
				int count = e.NewItems.Count;
				if (newStartingIndex < _layout.VisibleFromIndex)
				{
					ScrollPosition += (double)count * (ItemSize + Spacing);
				}
				InvalidateMeasure();
				InvalidateLayout();
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				int oldStartingIndex = e.OldStartingIndex;
				int count2 = e.OldItems.Count;
				if (oldStartingIndex < _layout.VisibleFromIndex)
				{
					ScrollPosition -= (double)count2 * (ItemSize + Spacing);
				}
				InvalidateMeasure();
				InvalidateLayout();
			}
			else if (e.Action != NotifyCollectionChangedAction.Replace)
			{
				_ = e.Action;
				_ = 3;
			}
		}

		private void UpdateItemsScrollPadding(Thickness padding)
		{
			if (padding.HorizontalThickness > 0.0 && Device.RuntimePlatform == "iOS")
			{
				if (_scrollViewerPaddingGrid == null)
				{
					_scrollViewerPaddingGrid = new Grid();
					_scrollView.Content = _scrollViewerPaddingGrid;
					_scrollViewerPaddingGrid.Children.Add(_layout);
				}
				_scrollViewerPaddingGrid.Padding = padding;
			}
			else
			{
				if (_scrollViewerPaddingGrid != null)
				{
					_scrollViewerPaddingGrid.Children.Clear();
					_scrollViewerPaddingGrid = null;
					_scrollView.Content = _layout;
				}
				_scrollView.Padding = padding;
			}
		}

		private void UpdateScrollBarVisibility()
		{
			if (Orientation == RepeaterOrientation.Horizontal)
			{
				_scrollView.HorizontalScrollBarVisibility = ScrollBarVisibility;
			}
			else
			{
				_scrollView.VerticalScrollBarVisibility = ScrollBarVisibility;
			}
		}

		private void UpdateSelection(object obj)
		{
			SelectedItem = obj;
			ItemSelected?.Invoke(this, new RepeaterSelectedItemChangedEventArgs(obj));
			ItemSelectedCommand?.Execute(SelectedItem);
		}
	}
}
