using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppName.Core
{
    [ContentProperty("Tabs")]
    public class TabControl : ContentView, ILayoutDirectionAware
    {
        private class CustomCommand : Command
        {
            public CustomCommand(Action<object> execute)
                : base(execute)
            {
            }
        }

        public static readonly BindableProperty TabSelectedCommandProperty = BindableProperty.Create("TabSelectedCommand", typeof(ICommand), typeof(TabControl));

        public static readonly BindableProperty TabTapCommandProperty = BindableProperty.Create("TabTapCommand", typeof(ICommand), typeof(TabControl));

        private readonly IDictionary<object, View> _views = new Dictionary<object, View>();

        private readonly Grid _mainContainerGrid = new Grid();

        private readonly Grid _contentGrid = new Grid();

        private readonly ScrollView _tabsScrollView = new ScrollView();

        private readonly Grid _tabStrip = new Grid
        {
            ColumnSpacing = 0.0
        };

        private readonly Grid _tabStripScrollViewWrapper = new Grid();

        private readonly ObservableCollection<TabItem> _manualTabs = new ObservableCollection<TabItem>();

        private bool _manualTabDrawingEnabled;

        private bool _triggerTabChange = true;

        private OrientationChangePatch _orientationChangePatch = new OrientationChangePatch(null);

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(TabControl), null, BindingMode.OneWay, null, delegate (BindableObject s, object o, object n)
        {
            ((TabControl)s).DrawIfReady();
        });

        public static readonly BindableProperty TabItemDataTemplateProperty = BindableProperty.Create("TabItemDataTemplate", typeof(DataTemplate), typeof(TabControl));

        public static readonly BindableProperty TabContentDataTemplateProperty = BindableProperty.Create("TabContentDataTemplate", typeof(DataTemplate), typeof(TabControl));

        public static readonly BindableProperty TabItemControlTemplateProperty = BindableProperty.Create("TabItemControlTemplate", typeof(ControlTemplate), typeof(TabControl), new ControlTemplate(typeof(DefaultTabItemTemplate)), BindingMode.OneWay, null, delegate (BindableObject s, object o, object n)
        {
            ((TabControl)s).DrawIfReady();
        });

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create("SelectedItem", typeof(object), typeof(TabControl), null, BindingMode.TwoWay, null, OnSelectedItemChanged);

        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create("SelectedIndex", typeof(int), typeof(TabControl), 0, BindingMode.OneWay, null, OnSelectedIndexChanged);

        public static readonly BindableProperty TabStripPlacementProperty = BindableProperty.Create("TabStripPlacement", typeof(TabStripPlacement), typeof(TabControl), TabStripPlacement.Top, BindingMode.OneWay, null, delegate (BindableObject s, object o, object n)
        {
            ((TabControl)s).SetTabStripPlacement((TabStripPlacement)n);
        });

        public static readonly BindableProperty TabStripStyleProperty = BindableProperty.Create("TabStripStyle", typeof(Style), typeof(TabControl), null, BindingMode.OneWay, null, delegate (BindableObject s, object o, object n)
        {
            ((VisualElement)((TabControl)s)._tabStrip).Style = ((Style)n);
        });

        public static readonly BindableProperty TabItemStyleProperty = BindableProperty.Create("TabItemStyle", typeof(Style), typeof(TabControl), null, BindingMode.OneWay, null, delegate (BindableObject s, object o, object n)
        {
            ((TabControl)s).UpdateTabItemsStyle((Style)o, (Style)n);
        });

        public static readonly BindableProperty AnimateTransitionProperty = BindableProperty.Create("AnimateTransition", typeof(bool), typeof(TabControl), true);

        public static readonly BindableProperty TabContentBackgroundColorProperty = BindableProperty.Create("TabContentBackgroundColor", typeof(Color), typeof(TabControl), Color.Default);

        public static readonly BindableProperty TabStripBackgroundColorProperty = BindableProperty.Create("TabStripBackgroundColor", typeof(Color), typeof(TabControl), Color.Default);

        public static readonly BindableProperty TabStripHeightProperty = BindableProperty.Create("TabStripHeight", typeof(double), typeof(TabControl), -1.0);

        public static readonly BindableProperty TabContentHeightProperty = BindableProperty.Create("TabContentHeight", typeof(double), typeof(TabControl), -1.0);

        public static readonly BindableProperty TabsWidthProperty = BindableProperty.Create("TabsWidth", typeof(GridLength), typeof(TabControl), GridLength.Star);

        private LayoutDirection _layoutDirection;

        public ICommand TabSelectedCommand
        {
            get
            {
                return (ICommand)GetValue(TabSelectedCommandProperty);
            }
            set
            {
                SetValue(TabSelectedCommandProperty, value);
            }
        }

        public ICommand TabTapCommand
        {
            get
            {
                return (ICommand)GetValue(TabTapCommandProperty);
            }
            set
            {
                SetValue(TabTapCommandProperty, value);
            }
        }

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

        public DataTemplate TabItemDataTemplate
        {
            get
            {
                return (DataTemplate)GetValue(TabItemDataTemplateProperty);
            }
            set
            {
                SetValue(TabItemDataTemplateProperty, value);
            }
        }

        public DataTemplate TabContentDataTemplate
        {
            get
            {
                return (DataTemplate)GetValue(TabContentDataTemplateProperty);
            }
            set
            {
                SetValue(TabContentDataTemplateProperty, value);
            }
        }

        public ControlTemplate TabItemControlTemplate
        {
            get
            {
                return (ControlTemplate)GetValue(TabItemControlTemplateProperty);
            }
            set
            {
                SetValue(TabItemControlTemplateProperty, value);
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

        public int SelectedIndex
        {
            get
            {
                return (int)GetValue(SelectedIndexProperty);
            }
            set
            {
                SetValue(SelectedIndexProperty, value);
            }
        }

        public TabStripPlacement TabStripPlacement
        {
            get
            {
                return (TabStripPlacement)GetValue(TabStripPlacementProperty);
            }
            set
            {
                SetValue(TabStripPlacementProperty, value);
            }
        }

        public IList<TabItem> Tabs => _manualTabs;

        public Style TabStripStyle
        {
            get
            {
                return (Style)GetValue(TabStripStyleProperty);
            }
            set
            {
                SetValue(TabStripStyleProperty, value);
            }
        }

        public Style TabItemStyle
        {
            get
            {
                return (Style)GetValue(TabItemStyleProperty);
            }
            set
            {
                SetValue(TabItemStyleProperty, value);
            }
        }

        public bool AnimateTransition
        {
            get
            {
                return (bool)GetValue(AnimateTransitionProperty);
            }
            set
            {
                SetValue(AnimateTransitionProperty, value);
            }
        }

        public Color TabContentBackgroundColor
        {
            get
            {
                return (Color)GetValue(TabContentBackgroundColorProperty);
            }
            set
            {
                SetValue(TabContentBackgroundColorProperty, value);
            }
        }

        public Color TabStripBackgroundColor
        {
            get
            {
                return (Color)GetValue(TabStripBackgroundColorProperty);
            }
            set
            {
                SetValue(TabStripBackgroundColorProperty, value);
            }
        }

        public double TabStripHeight
        {
            get
            {
                return (double)GetValue(TabStripHeightProperty);
            }
            set
            {
                SetValue(TabStripHeightProperty, value);
            }
        }

        public double TabContentHeight
        {
            get
            {
                return (double)GetValue(TabContentHeightProperty);
            }
            set
            {
                SetValue(TabContentHeightProperty, value);
            }
        }

        public GridLength TabsWidth
        {
            get
            {
                return (GridLength)GetValue(TabsWidthProperty);
            }
            set
            {
                SetValue(TabsWidthProperty, value);
            }
        }

        public event EventHandler<TabSelectedEventArgs> TabSelected;

        public event EventHandler<SelectedTabChangingEventArgs> SelectedTabChanging;

        public event EventHandler<TabTappedEventArgs> TabTapped;

        public TabControl()
        {
            Initialize();
            _manualTabs.CollectionChanged += OnManualTabsCollectionChanged;
            Rtl.SetMirrorBehavior(_tabStrip, MirrorBehavior.SkipSelf);
        }

        private void Initialize()
        {
            _mainContainerGrid.RowDefinitions.Add(new RowDefinition
            {
                Height = GridLength.Star
            });
            _mainContainerGrid.RowDefinitions.Add(new RowDefinition
            {
                Height = GridLength.Auto
            });
            _mainContainerGrid.RowSpacing = 0.0;
            _tabStrip.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, expands: false);
            _tabStrip.ColumnSpacing = 0.0;
            _tabsScrollView.Orientation = ScrollOrientation.Horizontal;
            _tabsScrollView.Content = _tabStrip;
            _tabStripScrollViewWrapper.Children.Add(_tabsScrollView);
            SetTabStripPlacement(TabStripPlacement);
            _mainContainerGrid.Children.Add(_contentGrid);
            _mainContainerGrid.Children.Add(_tabStripScrollViewWrapper);
            _tabStrip.SetBinding(VisualElement.BackgroundColorProperty, new Binding("TabStripBackgroundColor", BindingMode.Default, null, null, null, this));
            _tabStrip.SetBinding(VisualElement.HeightRequestProperty, new Binding("TabStripHeight", BindingMode.Default, null, null, null, this));
            _contentGrid.SetBinding(VisualElement.BackgroundColorProperty, new Binding("TabContentBackgroundColor", BindingMode.Default, null, null, null, this));
            _contentGrid.SetBinding(VisualElement.HeightRequestProperty, new Binding("TabContentHeight", BindingMode.Default, null, null, null, this));
            base.Content = _mainContainerGrid;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == VisualElement.HeightProperty.PropertyName)
            {
                _orientationChangePatch?.OnDimensionChanged(base.Height, delegate
                {
                    if (Device.RuntimePlatform == "Android")
                    {
                        Grid.SetColumn(_tabsScrollView, 1);
                    }
                    _mainContainerGrid.HorizontalOptions = LayoutOptions.Start;
                }, delegate
                {
                    Grid.SetColumn(_tabsScrollView, 0);
                    _mainContainerGrid.HorizontalOptions = LayoutOptions.Fill;
                });
            }
        }

        private void UpdateTabItemsStyle(Style oldStyle, Style newStyle)
        {
            for (int i = 0; i < _manualTabs.Count; i++)
            {
                TabItem tabItem = _manualTabs[i];
                if (((VisualElement)tabItem).Style == oldStyle || ((VisualElement)tabItem).Style == null)
                {
                    ((VisualElement)tabItem).Style = (newStyle);
                }
            }
        }

        private static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TabControl tabControl = (TabControl)bindable;
            int num = (int)newValue;
            if (tabControl._triggerTabChange && num < tabControl._tabStrip.Children.Count)
            {
                View view = tabControl._tabStrip.Children[num];
                tabControl.SelectedItem = ((view as TabItem) ?? view.BindingContext);
            }
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TabControl obj = (TabControl)bindable;
            object selectedItem = obj.SelectedItem;
            obj.ForceSelection(oldValue, selectedItem);
        }

        private void ForceSelection(object oldItem, object selectedItem)
        {
            _triggerTabChange = false;
            View view = _views[selectedItem];
            View item = (selectedItem as TabItem) ?? _tabStrip.Children.First((View x) => x.BindingContext == selectedItem);
            SelectedIndex = _tabStrip.Children.IndexOf(item);
            View value = null;
            if (oldItem != null)
            {
                _views.TryGetValue(oldItem, out value);
            }
            if (AnimateTransition)
            {
                if (value != null)
                {
                    FadeBehavior fadeBehavior = (FadeBehavior)value.Behaviors.FirstOrDefault((Behavior b) => b is FadeBehavior);
                    if (fadeBehavior != null)
                    {
                        fadeBehavior.IsSelected = false;
                    }
                }
                FadeBehavior fadeBehavior2 = (FadeBehavior)view.Behaviors.FirstOrDefault((Behavior b) => b is FadeBehavior);
                if (fadeBehavior2 != null)
                {
                    fadeBehavior2.IsSelected = true;
                }
            }
            else
            {
                if (value != null)
                {
                    value.IsVisible = false;
                    value.Opacity = 0.0;
                }
                view.IsVisible = true;
                view.Opacity = 1.0;
            }
            TabItem tabItem = selectedItem as TabItem;
            if (tabItem != null)
            {
                tabItem.IsSelected = true;
            }
            TabItem tabItem2 = oldItem as TabItem;
            if (tabItem2 != null)
            {
                tabItem2.IsSelected = false;
            }
            _triggerTabChange = true;
            this.TabSelected?.Invoke(this, new TabSelectedEventArgs(SelectedItem));
            TabSelectedCommand?.Execute(SelectedItem);
        }

        private void SetTabStripPlacement(TabStripPlacement newPlacement)
        {
            if (newPlacement == TabStripPlacement.Top)
            {
                Grid.SetRow(_tabStripScrollViewWrapper, 0);
                _mainContainerGrid.RowDefinitions[0].Height = GridLength.Auto;
                Grid.SetRow(_contentGrid, 1);
                _mainContainerGrid.RowDefinitions[1].Height = GridLength.Star;
            }
            else
            {
                Grid.SetRow(_contentGrid, 0);
                _mainContainerGrid.RowDefinitions[0].Height = GridLength.Star;
                Grid.SetRow(_tabStripScrollViewWrapper, 1);
                _mainContainerGrid.RowDefinitions[1].Height = GridLength.Auto;
            }
        }

        public void SetLayoutDirection(LayoutDirection layoutDirection)
        {
            if (_layoutDirection != layoutDirection)
            {
                _layoutDirection = layoutDirection;
                UpdateDirection();
            }
        }

        private void UpdateDirection()
        {
            for (int i = 0; i < _tabStrip.Children.Count; i++)
            {
                int value = (_layoutDirection == LayoutDirection.Ltr) ? i : (_tabStrip.Children.Count - i - 1);
                Grid.SetColumn(_tabStrip.Children[i], value);
            }
            UpdateStripLayout();
        }

        private void AddTabViewFromTemplate(object item)
        {
            DataTemplateSelector dataTemplateSelector = TabItemDataTemplate as DataTemplateSelector;
            View view = (dataTemplateSelector == null) ? ((View)TabItemDataTemplate.CreateContent()) : ((View)dataTemplateSelector.SelectTemplate(item, this).CreateContent());
            SetSelectionTapRecognizer(view, item);
            view.BindingContext = item;
            AddToTabStrip(view);
        }

        private void AddTabContentFromTemplate(object item)
        {
            DataTemplateSelector dataTemplateSelector = TabContentDataTemplate as DataTemplateSelector;
            View view = (dataTemplateSelector == null) ? ((View)TabContentDataTemplate.CreateContent()) : ((View)dataTemplateSelector.SelectTemplate(item, this).CreateContent());
            view.BindingContext = item;
            AddTransitionBehavior(view);
            _contentGrid.Children.Add(view);
            _views.Add(item, view);
        }

        private void DrawManualTab(TabItem tabItem, int index = -1)
        {
            if (tabItem.Content == null)
            {
                throw new ArgumentException("All tabs need to have the Content property set.");
            }
            ReleaseTabItem(tabItem);
            if (tabItem.ControlTemplate == null || tabItem.ControlTemplate == TabItemControlTemplateProperty.DefaultValue)
            {
                tabItem.ControlTemplate = TabItemControlTemplate;
            }
            AddTransitionBehavior(tabItem.Content);
            _contentGrid.Children.Add(tabItem.Content);
            _views.Add(tabItem, tabItem.Content);
            SetSelectionTapRecognizer(tabItem, tabItem);
            if (((VisualElement)tabItem).Style == null)
            {
                ((VisualElement)tabItem).Style = (TabItemStyle);
            }
            AddToTabStrip(tabItem, index);
        }

        private void ReleaseTabItem(TabItem tabItem)
        {
            RemoveFromTabStrip(tabItem);
            _contentGrid.Children.Remove(tabItem.Content);
            _views.Remove(tabItem);
            Behavior behavior = tabItem.Content.Behaviors.FirstOrDefault((Behavior x) => x is FadeBehavior);
            if (behavior != null)
            {
                tabItem.Content.Behaviors.Remove(behavior);
            }
            IGestureRecognizer gestureRecognizer = tabItem.GestureRecognizers.FirstOrDefault((IGestureRecognizer x) => (x as TapGestureRecognizer)?.Command is CustomCommand);
            if (gestureRecognizer != null)
            {
                tabItem.GestureRecognizers.Remove(gestureRecognizer);
            }
        }

        private void OnManualTabsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (TabItem oldItem in e.OldItems)
                {
                    ReleaseTabItem(oldItem);
                }
            }
            if (_manualTabDrawingEnabled)
            {
                if (e.NewItems != null)
                {
                    foreach (TabItem newItem in e.NewItems)
                    {
                        DrawManualTab(newItem, Tabs.IndexOf(newItem));
                    }
                }
                if (SelectedIndex == _tabStrip.Children.Count - 1)
                {
                    UpdateSelectedTabAccordingToCurrentlySelectedIndexWhenManualTab();
                }
            }
        }

        private void UpdateSelectedTabAccordingToCurrentlySelectedIndexWhenManualTab()
        {
            TabItem tabItem = _tabStrip.Children[SelectedIndex] as TabItem;
            if (SelectedItem == tabItem)
            {
                ForceSelection(null, tabItem);
            }
            else
            {
                SelectedItem = tabItem;
            }
        }

        private void SetSelectionTapRecognizer(View tab, object param)
        {
            tab.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new CustomCommand(delegate (object tabSelected)
                {
                    (tabSelected as TabItem)?.RaiseTapped();
                    this.TabTapped?.Invoke(this, new TabTappedEventArgs(tabSelected));
                    TabTapCommand?.Execute(tabSelected);
                    if (SelectedItem == null || !SelectedItem.Equals(tabSelected))
                    {
                        SelectedTabChangingEventArgs selectedTabChangingEventArgs = new SelectedTabChangingEventArgs(tabSelected);
                        this.SelectedTabChanging?.Invoke(this, selectedTabChangingEventArgs);
                        if (!selectedTabChangingEventArgs.CancelSelection)
                        {
                            SelectedItem = tabSelected;
                        }
                    }
                }),
                CommandParameter = param
            });
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            if (_tabStrip.Children.Count == 0)
            {
                DrawIfReady();
            }
        }

        private void DrawIfReady()
        {
            bool flag = ItemsSource != null && TabItemDataTemplate != null && TabContentDataTemplate != null;
            bool flag2 = Tabs != null && Tabs.Any();
            if (!(flag | flag2))
            {
                return;
            }
            bool flag3 = !flag;
            if (flag3)
            {
                foreach (TabItem tab in Tabs)
                {
                    ReleaseTabItem(tab);
                }
            }
            ClearTabStrip();
            _contentGrid.Children.Clear();
            _views.Clear();
            if (!flag3)
            {
                object obj = null;
                int num = 0;
                foreach (object item in ItemsSource)
                {
                    if (num == SelectedIndex)
                    {
                        obj = item;
                    }
                    num++;
                    AddTabViewFromTemplate(item);
                    AddTabContentFromTemplate(item);
                }
                if (SelectedItem == obj)
                {
                    ForceSelection(null, obj);
                }
                else
                {
                    SelectedItem = obj;
                }
            }
            else
            {
                _manualTabDrawingEnabled = true;
                foreach (TabItem tab2 in Tabs)
                {
                    DrawManualTab(tab2);
                }
                if (SelectedIndex < _tabStrip.Children.Count)
                {
                    UpdateSelectedTabAccordingToCurrentlySelectedIndexWhenManualTab();
                }
                else
                {
                    SelectedIndex = 0;
                }
            }
        }

        private void ClearTabStrip()
        {
            foreach (View child in _tabStrip.Children)
            {
                child.PropertyChanged -= OnTabPropertyChanged;
            }
            _tabStrip.Children.Clear();
            _tabStrip.ColumnDefinitions.Clear();
        }

        private void RemoveFromTabStrip(TabItem tabItem)
        {
            tabItem.PropertyChanged -= OnTabPropertyChanged;
            int num = _tabStrip.Children.IndexOf(tabItem);
            if (num >= 0)
            {
                _tabStrip.Children.Remove(tabItem);
                _tabStrip.ColumnDefinitions.RemoveAt(num);
                for (int i = num; i < _tabStrip.Children.Count; i++)
                {
                    Grid.SetColumn(_tabStrip.Children[i], i);
                }
            }
            UpdateStripLayout();
        }

        private void AddToTabStrip(View tabView, int index = -1)
        {
            tabView.PropertyChanged -= OnTabPropertyChanged;
            tabView.PropertyChanged += OnTabPropertyChanged;
            ColumnDefinition item = new ColumnDefinition
            {
                Width = TabsWidth
            };
            _tabStrip.ColumnDefinitions.Add(item);
            if (_layoutDirection == LayoutDirection.Ltr)
            {
                if (index < 0)
                {
                    _tabStrip.Children.Add(tabView);
                    int num = _tabStrip.Children.Count - 1;
                    tabView.SetValue(Grid.ColumnProperty, num);
                }
                else
                {
                    _tabStrip.Children.Insert(index, tabView);
                    for (int i = index; i < _tabStrip.Children.Count; i++)
                    {
                        Grid.SetColumn(_tabStrip.Children[i], i);
                    }
                }
            }
            else if (index < 0)
            {
                for (int j = 0; j < _tabStrip.Children.Count; j++)
                {
                    View bindable = _tabStrip.Children[j];
                    Grid.SetColumn(bindable, Grid.GetColumn(bindable) + 1);
                }
                _tabStrip.Children.Add(tabView);
            }
            else
            {
                index = _tabStrip.Children.Count - index;
                _tabStrip.Children.Insert(index, tabView);
                for (int k = index; k < _tabStrip.Children.Count; k++)
                {
                    Grid.SetColumn(_tabStrip.Children[k], k);
                }
            }
            UpdateStripLayout();
        }

        private void OnTabPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsVisible")
            {
                UpdateStripLayout();
            }
        }

        private void UpdateStripLayout()
        {
            for (int i = 0; i < _tabStrip.Children.Count; i++)
            {
                int index = (_layoutDirection == LayoutDirection.Ltr) ? i : (_tabStrip.Children.Count - i - 1);
                _tabStrip.ColumnDefinitions[index].Width = (_tabStrip.Children[i].IsVisible ? TabsWidth : ((GridLength)0.0));
            }
        }

        private void AddTransitionBehavior(View view)
        {
            FadeBehavior item = new FadeBehavior();
            view.Behaviors.Add(item);
        }
    }
}
