using CoreGraphics;
using Foundation;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AppName.Core
{
    public class PageRenderer : Xamarin.Forms.Platform.iOS.PageRenderer
    {
        private NSObject _keyboardShowObserver;

        private NSObject _keyboardHideObserver;

        private double _activeViewBottom;

        private double _originalY;

        private bool _pageWasShiftedUp;

        private bool _isKeyboardShown;

        public PageRenderer()
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ContentPage contentPage = base.Element as ContentPage;
            if (contentPage != null && !(contentPage.Content is ScrollView))
            {
                RegisterForKeyboardNotifications();
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            UnregisterForKeyboardNotifications();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            UnregisterForKeyboardNotifications();
        }

        private void RegisterForKeyboardNotifications()
        {
            if (_keyboardShowObserver == null)
            {
                _keyboardShowObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardShow);
            }
            if (_keyboardHideObserver == null)
            {
                _keyboardHideObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardHide);
            }
        }

        private void UnregisterForKeyboardNotifications()
        {
            _isKeyboardShown = false;
            if (_keyboardShowObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardShowObserver);
                _keyboardShowObserver.Dispose();
                _keyboardShowObserver = null;
            }
            if (_keyboardHideObserver != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardHideObserver);
                _keyboardHideObserver.Dispose();
                _keyboardHideObserver = null;
            }
        }

        protected virtual void OnKeyboardShow(NSNotification notification)
        {
            if (!IsViewLoaded || _isKeyboardShown)
            {
                return;
            }

            CGRect keyboardFrame = UIKeyboard.FrameEndFromNotification(notification);
            if (keyboardFrame.Height == 0)
            {
                return;
            }

            _isKeyboardShown = true;
            UIView uIView = FindFirstResponder(View);
            if (uIView == null)
            {
                return;
            }

            double requiredBottomOffset = 0.0;
            IVisualElementRenderer visualElementRenderer = uIView.Superview as IVisualElementRenderer;
            if (visualElementRenderer != null)
            {
                View view = visualElementRenderer.Element as View;
                if (view != null)
                {
                    requiredBottomOffset = view.Margin.Bottom;
                }
            }

            bool flag = IsKeyboardOverlapping(uIView, keyboardFrame, requiredBottomOffset);
            if (flag && flag)
            {
                _activeViewBottom = GetViewRelativeBottom(uIView, requiredBottomOffset);
                ShiftPageUp(keyboardFrame, _activeViewBottom);
            }
        }

        private void OnKeyboardHide(NSNotification notification)
        {
            if (IsViewLoaded)
            {
                _isKeyboardShown = false;
                CGRect keyboardFrame = UIKeyboard.FrameEndFromNotification(notification);
                if (_pageWasShiftedUp)
                {
                    ShiftPageDown(keyboardFrame, _activeViewBottom);
                }
            }
        }

        private void ShiftPageUp(CGRect keyboardFrame, double activeViewBottom)
        {
            Rectangle bounds = base.Element.Bounds;
            _originalY = bounds.Y;
            double y = bounds.Y + (double)keyboardFrame.Y - activeViewBottom;
            base.Element.LayoutTo(new Rectangle(bounds.X, y, bounds.Width, bounds.Height));
            _pageWasShiftedUp = true;
        }

        private void ShiftPageDown(CGRect keyboardFrame, double activeViewBottom)
        {
            Rectangle bounds = base.Element.Bounds;
            base.Element.LayoutTo(new Rectangle(bounds.X, _originalY, bounds.Width, bounds.Height));
            _pageWasShiftedUp = false;
        }

        private static UIView FindFirstResponder(UIView view)
        {
            if (view.IsFirstResponder)
            {
                return view;
            }

            UIView[] subviews = view.Subviews;
            for (int i = 0; i < subviews.Length; i++)
            {
                UIView uIView = FindFirstResponder(subviews[i]);
                if (uIView != null)
                {
                    return uIView;
                }
            }

            return null;
        }

        private static bool IsKeyboardOverlapping(UIView activeView, CGRect keyboardFrame, double requiredBottomOffset)
        {
            double viewRelativeBottom = GetViewRelativeBottom(activeView, requiredBottomOffset);
            nfloat y = keyboardFrame.Y;
            return viewRelativeBottom >= (double)y;
        }

        private static double GetViewRelativeBottom(UIView view, double requiredBottomOffset)
        {
            return (double)(-UIScreen.MainScreen.CoordinateSpace.ConvertPointToCoordinateSpace(view.Frame.Location, view).Y + view.Frame.Height) + requiredBottomOffset;
        }
    }
}
