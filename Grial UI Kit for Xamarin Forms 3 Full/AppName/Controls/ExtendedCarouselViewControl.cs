using System;
using System.Runtime.CompilerServices;
using CarouselView.FormsPlugin.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;

using ScrolledEventArgs = CarouselView.FormsPlugin.Abstractions.ScrolledEventArgs;

namespace AppName
{
    public class ExtendedCarouselViewControl : CarouselViewControl
    {
        private static readonly BindablePropertyKey ScrollProgressPropertyKey =
            BindableProperty.CreateReadOnly(nameof(ScrollProgress), typeof(double), typeof(ExtendedCarouselViewControl), 0.0);

        public static readonly BindableProperty ScrollProgressProperty = ScrollProgressPropertyKey.BindableProperty;

        public double ScrollProgress
        {
            get { return (double)GetValue(ScrollProgressProperty); }
            private set { SetValue(ScrollProgressPropertyKey, value); }
        }

        private static readonly BindablePropertyKey CurrentItemPropertyKey =
            BindableProperty.CreateReadOnly(nameof(CurrentItem), typeof(object), typeof(ExtendedCarouselViewControl), null);

        public static readonly BindableProperty CurrentItemProperty = CurrentItemPropertyKey.BindableProperty;

        public object CurrentItem
        {
            get { return GetValue(CurrentItemProperty); }
            private set { SetValue(CurrentItemPropertyKey, value); }
        }

        private static readonly BindablePropertyKey NextItemPropertyKey =
            BindableProperty.CreateReadOnly(nameof(NextItem), typeof(object), typeof(ExtendedCarouselViewControl), null);

        public static readonly BindableProperty NextItemProperty = NextItemPropertyKey.BindableProperty;

        public object NextItem
        {
            get { return GetValue(NextItemProperty); }
            private set { SetValue(NextItemPropertyKey, value); }
        }

        public static readonly BindableProperty ResetPositionOnItemsChangeProperty =
            BindableProperty.Create(nameof(ResetPositionOnItemsChange), typeof(bool), typeof(ExtendedCarouselViewControl), false);

        public bool ResetPositionOnItemsChange
        {
            get { return (bool)GetValue(ResetPositionOnItemsChangeProperty); }
            set { SetValue(ResetPositionOnItemsChangeProperty, value); }
        }

        private bool _isInverseScrolling;
        private bool _isTransitioning;
        private DisplayOrientation _lastKnownOrientation;

        public ExtendedCarouselViewControl()
        {
            Scrolled += OnScrolled;

            _lastKnownOrientation = DeviceDisplay.MainDisplayInfo.Orientation;
        }

        public void OnAndroidScrolledFix(double value, bool increase)
        {
            HandleScroll(value, increase);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            var orientation = _lastKnownOrientation;
            _lastKnownOrientation = DeviceDisplay.MainDisplayInfo.Orientation;

            if (orientation != _lastKnownOrientation)
            {
                Device.BeginInvokeOnMainThread(() => ScrollProgress = 0);
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(ItemsSource))
            {
                UpdateItems();
                if (ResetPositionOnItemsChange)
                {
                    Position = 0;
                }
            }
            else if (propertyName == nameof(Position))
            {
                var progress = Math.Abs(ScrollProgress);

                if (progress > 0 && progress < 1)
                {
                    _isTransitioning = true;
                }
                else
                {
                    PositionDidChange();
                }
            }
        }

        private void OnScrolled(object sender, ScrolledEventArgs e)
        {
            _isTransitioning = false;
            if (e.NewValue < 100 && e.NewValue > Math.Abs(ScrollProgress) * 100)
            {
                _isInverseScrolling = e.Direction == ScrollDirection.Up || e.Direction == ScrollDirection.Left;
            }

            HandleScroll(e.NewValue, _isInverseScrolling);
        }

        private void HandleScroll(double value, bool inverse)
        {
            var result = value / 100;

            ScrollProgress = inverse ? -result : result;

            if (!_isTransitioning)
            {
                UpdateItems(!inverse);
            }
            else if (result == 1)
            {
                PositionDidChange();
            }
        }

        private void PositionDidChange()
        {
            _isTransitioning = false;
            UpdateItems();

            Device.BeginInvokeOnMainThread(() => ScrollProgress = 0);
        }

        private void UpdateItems(bool? isForwardDirection = null)
        {
            object current = null, next = null;

            if (ItemsSource != null)
            {
                int i = 0;
                foreach (var item in ItemsSource)
                {
                    if (Position == i)
                    {
                        current = item;

                        if (isForwardDirection != true)
                        {
                            break;
                        }
                    }
                    else if (isForwardDirection == false && Position == i + 1)
                    {
                        next = item;
                    }
                    else if (isForwardDirection == true && Position == i - 1)
                    {
                        next = item;
                        break;
                    }

                    i++;
                }

                CurrentItem = current;
                NextItem = next;
            }
        }
    }
}
