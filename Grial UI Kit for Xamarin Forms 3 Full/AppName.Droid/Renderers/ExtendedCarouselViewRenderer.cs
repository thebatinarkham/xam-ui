using System;
using System.ComponentModel;
using Android.Content;
using Android.Support.V4.View;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using CarouselViewRenderer = CarouselView.FormsPlugin.Android.CarouselViewRenderer_Fix;

namespace AppName.Droid
{
    /// <summary>
    /// This Android renderer extends the original to provide a similar scrolling behavior than the iOS renderer
    /// which is keep sending scroll changes even after the swipe gesture finishes until the ViewPager reaches
    /// its final position.
    /// Base class code here: 
    ///   https://github.com/alexrainman/CarouselView/blob/master/CarouselView/CarouselView.FormsPlugin.Android/CarouselViewImplementation.cs
    /// </summary>
    public class ExtendedCarouselViewRenderer : CarouselViewRenderer
    {
        private bool _isInteractiveDragInverse;
        private bool _positionWillChange;
        private int _lastKnownScrollState;
        private ViewPager _pager;

        public ExtendedCarouselViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            if (_pager != null)
            {
                // Layout fix for orientation change
                for (var i = 0; i < _pager.ChildCount; i++)
                {
                    var renderer = _pager.GetChildAt(i);
                    if (renderer is IVisualElementRenderer r)
                    {
                        r.Element.Layout(new Rectangle(0, 0, Element.Width, Element.Height));
                    }
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ExtendedCarouselViewControl.ScrollProgressProperty.PropertyName) ||
                e.PropertyName == nameof(ExtendedCarouselViewControl.CurrentItemProperty.PropertyName) ||
                e.PropertyName == nameof(ExtendedCarouselViewControl.NextItemProperty.PropertyName))
            {
                return;
            }

            Unsuscribe();

            base.OnElementPropertyChanged(sender, e);

            Subscribe();
        }

        private void OnPageScrolled(object sender, ViewPager.PageScrolledEventArgs e)
        {
            if (_lastKnownScrollState == ViewPager.ScrollStateDragging)
            {
                _isInteractiveDragInverse = e.Position < Element.Position;
            }
            else
            {
                var carousel = (ExtendedCarouselViewControl)Element;

                if (_positionWillChange && e.PositionOffset == 0)
                {
                    if (_isInteractiveDragInverse)
                    {
                        carousel.OnAndroidScrolledFix(0, true);
                    }
                    else
                    {
                        carousel.OnAndroidScrolledFix(100, false);
                    }
                }
                else
                {
                    _positionWillChange = e.Position != Element.Position;

                    var result = _isInteractiveDragInverse ?
                        Math.Floor((1 - e.PositionOffset) * 100) :
                        Math.Floor(e.PositionOffset * 100);

                    carousel.OnAndroidScrolledFix(result, _isInteractiveDragInverse);
                }
            }
        }

        private void OnPageScrollStateChanged(object sender, ViewPager.PageScrollStateChangedEventArgs e)
        {
            _lastKnownScrollState = e.State;
        }

        private void Subscribe()
        {
            _pager = Control?.FindViewById<ViewPager>(Resource.Id.pager);

            if (_pager != null)
            {
                _pager.PageScrollStateChanged += OnPageScrollStateChanged;
                _pager.PageScrolled += OnPageScrolled;
            }
        }

        private void Unsuscribe()
        {
            if (_pager != null)
            {
                _pager.PageScrollStateChanged -= OnPageScrollStateChanged;
                _pager.PageScrolled -= OnPageScrolled;
                _pager = null;
            }
        }
    }
}
