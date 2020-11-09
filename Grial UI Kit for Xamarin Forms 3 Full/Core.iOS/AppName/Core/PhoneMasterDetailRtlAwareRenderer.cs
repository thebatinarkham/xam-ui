using CoreGraphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AppName.Core
{
    public class PhoneMasterDetailRtlAwareRenderer : UIViewController, IVisualElementRenderer, IDisposable, IRegisterable, IEffectControlProvider
    {
        private class ChildViewController : UIViewController
        {
            public override void ViewDidLayoutSubviews()
            {
                UIViewController[] childViewControllers = ChildViewControllers;
                for (int i = 0; i < childViewControllers.Length; i++)
                {
                    childViewControllers[i].View.Frame = View.Bounds;
                }
            }
        }

        private bool _wasRtlOnLastChildrenLayout;

        private UIView _clickOffView;

        private UIViewController _detailController;

        private bool _disposed;

        private EventTracker _events;

        private UIViewController _masterController;

        private UIPanGestureRecognizer _panGesture;

        private bool _presented;

        private UIGestureRecognizer _tapGesture;

        private VisualElementTracker _tracker;

        private IPageController PageController => Element as IPageController;

        private bool IsRtl => LayoutDirectionService.Instance.LayoutDirection == LayoutDirection.Rtl;

        private IMasterDetailPageController MasterDetailPageController => Element as IMasterDetailPageController;

        private bool Presented
        {
            get
            {
                return _presented;
            }
            set
            {
                if (_presented != value)
                {
                    _presented = value;
                    LayoutChildren(animated: true);
                    if (value)
                    {
                        AddClickOffView();
                    }
                    else
                    {
                        RemoveClickOffView();
                    }
                    ((IElementController)Element).SetValueFromRenderer(MasterDetailPage.IsPresentedProperty, (object)value);
                }
            }
        }

        public VisualElement Element
        {
            get;
            private set;
        }

        public UIView NativeView => View;

        public UIViewController ViewController => this;

        public event EventHandler<VisualElementChangedEventArgs> ElementChanged;

        public PhoneMasterDetailRtlAwareRenderer()
        {
            if (!UIDevice.CurrentDevice.CheckSystemVersion(7, 0))
            {
                WantsFullScreenLayout = true;
            }
            LayoutDirectionService.Instance.LayoutDirectionChanged -= OnLayoutDirectionChanged;
        }

        public SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return NativeView.GetSizeRequest(widthConstraint, heightConstraint);
        }

        public void SetElement(VisualElement element)
        {
            VisualElement element2 = Element;
            Element = element;
            Element.SizeChanged += PageOnSizeChanged;
            _masterController = new ChildViewController();
            _detailController = new ChildViewController();
            _clickOffView = new UIView();
            _clickOffView.BackgroundColor = new Color(0.0, 0.0, 0.0, 0.0).ToUIColor();
            Presented = ((MasterDetailPage)Element).IsPresented;
            OnElementChanged(new VisualElementChangedEventArgs(element2, element));
            RegisterEffectControlProvider(this, element2, element);
        }

        public void SetElementSize(Size size)
        {
            Element.Layout(new Rectangle(Element.X, Element.Y, size.Width, size.Height));
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            PageController.SendAppearing();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            PageController.SendDisappearing();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            LayoutChildren(animated: false);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _tracker = new VisualElementTracker(this);
            _events = new EventTracker(this);
            _events.LoadEvents(View);
            ((MasterDetailPage)Element).PropertyChanged += HandlePropertyChanged;
            _tapGesture = new UITapGestureRecognizer((Action)delegate
            {
                if (Presented)
                {
                    Presented = false;
                }
            });
            _clickOffView.AddGestureRecognizer(_tapGesture);
            PackContainers();
            UpdateMasterDetailContainers();
            UpdateBackground();
            UpdatePanGesture();
        }

        public override void WillRotate(UIInterfaceOrientation toInterfaceOrientation, double duration)
        {
            if (!MasterDetailPageController.ShouldShowSplitMode && _presented)
            {
                Presented = false;
            }
            base.WillRotate(toInterfaceOrientation, duration);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                Element.SizeChanged -= PageOnSizeChanged;
                Element.PropertyChanged -= HandlePropertyChanged;
                if (_tracker != null)
                {
                    _tracker.Dispose();
                    _tracker = null;
                }
                if (_events != null)
                {
                    _events.Dispose();
                    _events = null;
                }
                if (_tapGesture != null)
                {
                    if (_clickOffView != null && _clickOffView.GestureRecognizers.Contains(_panGesture))
                    {
                        _clickOffView.GestureRecognizers = RemoveItem(_clickOffView.GestureRecognizers, _tapGesture);
                        _clickOffView.Dispose();
                    }
                    _tapGesture.Dispose();
                }
                if (_panGesture != null)
                {
                    if (View != null && View.GestureRecognizers.Contains(_panGesture))
                    {
                        RemoveItem(View.GestureRecognizers, _panGesture);
                    }
                    _panGesture.Dispose();
                }
                EmptyContainers();
                PageController.SendDisappearing();
                _disposed = true;
            }
            base.Dispose(disposing);
        }

        protected virtual void OnElementChanged(VisualElementChangedEventArgs e)
        {
            this.ElementChanged?.Invoke(this, e);
        }

        private void AddClickOffView()
        {
            View.Add(_clickOffView);
            _clickOffView.Frame = _detailController.View.Frame;
        }

        private void EmptyContainers()
        {
            foreach (UIView item in _detailController.View.Subviews.Concat(_masterController.View.Subviews))
            {
                item.RemoveFromSuperview();
            }
            foreach (UIViewController item2 in _detailController.ChildViewControllers.Concat(_masterController.ChildViewControllers))
            {
                item2.RemoveFromParentViewController();
            }
        }

        private void HandleMasterPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Page.IconImageSourceProperty.PropertyName || e.PropertyName == Page.TitleProperty.PropertyName)
            {
                MessagingCenter.Send((IVisualElementRenderer)this, "Xamarin.UpdateToolbarButtons");
            }
        }

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Master" || e.PropertyName == "Detail")
            {
                UpdateMasterDetailContainers();
            }
            else if (e.PropertyName == MasterDetailPage.IsPresentedProperty.PropertyName)
            {
                Presented = ((MasterDetailPage)Element).IsPresented;
            }
            else if (e.PropertyName == MasterDetailPage.IsGestureEnabledProperty.PropertyName)
            {
                UpdatePanGesture();
            }
            else if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
            {
                UpdateBackground();
            }
            else if (e.PropertyName == Page.BackgroundImageSourceProperty.PropertyName)
            {
                UpdateBackground();
            }
        }

        private void LayoutChildren(bool animated)
        {
            CGRect cGRect = Element.Bounds.ToRectangleF();
            CGRect frame = cGRect;
            frame.Width = (int)(Math.Min(frame.Width, frame.Height) * 0.8);
            if (IsRtl)
            {
                frame.X = cGRect.Width - frame.Width;
            }
            _masterController.View.Frame = frame;
            CGRect frame2 = cGRect;
            if (Presented)
            {
                if (IsRtl)
                {
                    frame2.X -= frame.Width;
                }
                else
                {
                    frame2.X += frame.Width;
                }
            }
            if (animated)
            {
                UIView.BeginAnimations("Flyout");
                _detailController.View.Frame = frame2;
                UIView.SetAnimationCurve(UIViewAnimationCurve.EaseOut);
                UIView.SetAnimationDuration(250.0);
                UIView.CommitAnimations();
            }
            else
            {
                _detailController.View.Frame = frame2;
            }
            MasterDetailPageController.MasterBounds = new Rectangle(0.0, 0.0, frame.Width, frame.Height);
            MasterDetailPageController.DetailBounds = new Rectangle(0.0, 0.0, cGRect.Width, cGRect.Height);
            if (Presented)
            {
                _clickOffView.Frame = _detailController.View.Frame;
            }
            if (_wasRtlOnLastChildrenLayout != IsRtl)
            {
                MasterDetailPage masterDetailPage = Element as MasterDetailPage;
                if (masterDetailPage != null)
                {
                    ContentPage contentPage = masterDetailPage.Master as ContentPage;
                    if (contentPage != null)
                    {
                        BindableObject bindableObject = FindElementWithSafeAreaEffectUpTo2LevelsDeep(contentPage);
                        if (bindableObject != null)
                        {
                            ApplyIOSSafeAreaAsPaddingEffect.SetForceSafeAreaRecalculationCount(bindableObject, ApplyIOSSafeAreaAsPaddingEffect.GetForceSafeAreaRecalculationCount(bindableObject) + 1);
                        }
                    }
                }
            }
            _wasRtlOnLastChildrenLayout = IsRtl;
        }

        private BindableObject FindElementWithSafeAreaEffectUpTo2LevelsDeep(ContentPage page)
        {
            if (Effects.GetApplyIOSSafeAreaAsPadding(page) != 0)
            {
                return page;
            }
            Layout layout = page.Content as Layout;
            if (layout != null)
            {
                if (Effects.GetApplyIOSSafeAreaAsPadding(page) != 0)
                {
                    return layout;
                }
                Layout<View> layout2 = layout as Layout<View>;
                if (layout2 != null)
                {
                    IList<View> children = layout2.Children;
                    if (children != null && children.Count > 0)
                    {
                        for (int i = 0; i < layout2.Children.Count; i++)
                        {
                            View view = layout2.Children[i];
                            if (Effects.GetApplyIOSSafeAreaAsPadding(view) != 0)
                            {
                                return view;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private void PackContainers()
        {
            _detailController.View.BackgroundColor = new UIColor(1, 1, 1, 1);
            View.AddSubview(_masterController.View);
            View.AddSubview(_detailController.View);
            AddChildViewController(_masterController);
            AddChildViewController(_detailController);
        }

        private void PageOnSizeChanged(object sender, EventArgs eventArgs)
        {
            LayoutChildren(animated: false);
        }

        private void RemoveClickOffView()
        {
            _clickOffView.RemoveFromSuperview();
        }

        private void UpdateBackground()
        {
            if (!string.IsNullOrEmpty(((Page)Element).BackgroundImage))
            {
                View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromBundle(((Page)Element).BackgroundImage));
            }
            else if (Element.BackgroundColor == Color.Default)
            {
                View.BackgroundColor = UIColor.White;
            }
            else
            {
                View.BackgroundColor = Element.BackgroundColor.ToUIColor();
            }
        }

        private void UpdateMasterDetailContainers()
        {
            ((MasterDetailPage)Element).Master.PropertyChanged -= HandleMasterPropertyChanged;
            EmptyContainers();
            if (Xamarin.Forms.Platform.iOS.Platform.GetRenderer(((MasterDetailPage)Element).Master) == null)
            {
                Xamarin.Forms.Platform.iOS.Platform.SetRenderer(((MasterDetailPage)Element).Master, Xamarin.Forms.Platform.iOS.Platform.CreateRenderer(((MasterDetailPage)Element).Master));
            }
            if (Xamarin.Forms.Platform.iOS.Platform.GetRenderer(((MasterDetailPage)Element).Detail) == null)
            {
                Xamarin.Forms.Platform.iOS.Platform.SetRenderer(((MasterDetailPage)Element).Detail, Xamarin.Forms.Platform.iOS.Platform.CreateRenderer(((MasterDetailPage)Element).Detail));
            }
            IVisualElementRenderer renderer = Xamarin.Forms.Platform.iOS.Platform.GetRenderer(((MasterDetailPage)Element).Master);
            IVisualElementRenderer renderer2 = Xamarin.Forms.Platform.iOS.Platform.GetRenderer(((MasterDetailPage)Element).Detail);
            ((MasterDetailPage)Element).Master.PropertyChanged += HandleMasterPropertyChanged;
            _masterController.View.AddSubview(renderer.NativeView);
            _masterController.AddChildViewController(renderer.ViewController);
            _detailController.View.AddSubview(renderer2.NativeView);
            _detailController.AddChildViewController(renderer2.ViewController);
        }

        private void UpdatePanGesture()
        {
            if (!((MasterDetailPage)Element).IsGestureEnabled)
            {
                if (_panGesture != null)
                {
                    View.RemoveGestureRecognizer(_panGesture);
                }
                return;
            }
            if (_panGesture != null)
            {
                View.AddGestureRecognizer(_panGesture);
                return;
            }
            UITouchEventArgs shouldReceiveTouch = (UIGestureRecognizer g, UITouch t) => !(t.View is UISlider);
            CGPoint center = default(CGPoint);
            _panGesture = new UIPanGestureRecognizer(delegate (UIPanGestureRecognizer g)
            {
                UIGestureRecognizerState state = g.State;
                UIGestureRecognizerState num = state - 1;
                if ((ulong)num <= 2uL)
                {
                    switch (num)
                    {
                        case UIGestureRecognizerState.Possible:
                            center = g.LocationInView(g.View);
                            break;
                        case UIGestureRecognizerState.Began:
                            {
                                nfloat v = g.LocationInView(g.View).X - center.X;
                                UIView view = _detailController.View;
                                CGRect frame3 = view.Frame;
                                if (IsRtl)
                                {
                                    if (Presented)
                                    {
                                        frame3.X = (nfloat)Math.Min(0.0, Math.Max(0.0, v) - (double)_masterController.View.Frame.Width);
                                    }
                                    else
                                    {
                                        frame3.X = (nfloat)Math.Max(-_masterController.View.Frame.Width, Math.Min(0.0, v));
                                    }
                                }
                                else if (Presented)
                                {
                                    frame3.X = (nfloat)Math.Max(0.0, (double)_masterController.View.Frame.Width + Math.Min(0.0, v));
                                }
                                else
                                {
                                    frame3.X = (nfloat)Math.Min(_masterController.View.Frame.Width, Math.Max(0.0, v));
                                }
                                view.Frame = frame3;
                                break;
                            }
                        case UIGestureRecognizerState.Changed:
                            {
                                CGRect frame = _detailController.View.Frame;
                                CGRect frame2 = _masterController.View.Frame;
                                double num2 = Math.Abs(frame.X);
                                if (Presented)
                                {
                                    if (num2 < (double)frame2.Width * 0.75)
                                    {
                                        Presented = false;
                                    }
                                    else
                                    {
                                        LayoutChildren(animated: true);
                                    }
                                }
                                else if (num2 > (double)frame2.Width * 0.25)
                                {
                                    Presented = true;
                                }
                                else
                                {
                                    LayoutChildren(animated: true);
                                }
                                break;
                            }
                    }
                }
            });
            _panGesture.ShouldReceiveTouch = shouldReceiveTouch;
            _panGesture.MaximumNumberOfTouches = (byte)2;
            View.AddGestureRecognizer(_panGesture);
        }

        void IEffectControlProvider.RegisterEffect(Effect effect)
        {
        }

        private static void RegisterEffectControlProvider(IEffectControlProvider self, IElementController oldElement, IElementController newElement)
        {
            if (oldElement != null && oldElement.EffectControlProvider == self)
            {
                oldElement.EffectControlProvider = null;
            }
            if (newElement != null)
            {
                newElement.EffectControlProvider = self;
            }
        }

        public static UIGestureRecognizer[] RemoveItem(UIGestureRecognizer[] array, UIGestureRecognizer item)
        {
            int num = -1;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == item)
                {
                    num = i;
                    break;
                }
            }
            if (num < 0)
            {
                return array;
            }
            UIGestureRecognizer[] array2 = new UIGestureRecognizer[array.Length - 1];
            if (num > 0)
            {
                Array.Copy(array, array2, num);
            }
            if (num < array.Length - 1)
            {
                Array.Copy(array, num + 1, array2, num, array.Length - num - 1);
            }
            return array2;
        }

        private void OnLayoutDirectionChanged(object sender, EventArgs e)
        {
            LayoutChildren(animated: false);
        }
    }
}
