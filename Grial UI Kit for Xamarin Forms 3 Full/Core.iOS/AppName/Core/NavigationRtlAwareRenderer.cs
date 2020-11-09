using System;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace AppName.Core
{
    public class NavigationRtlAwareRenderer : GrialNavigationPageRenderer
    {
        public NavigationRtlAwareRenderer()
        {
            LayoutDirectionService.Instance.LayoutDirectionChanged += OnLayoutDirectionChanged;
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            UpdateLayoutDirection();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            LayoutDirectionService.Instance.LayoutDirectionChanged -= OnLayoutDirectionChanged;
        }

        private void UpdateLayoutDirection()
        {
            UISemanticContentAttribute semanticContentAttribute = (UISemanticContentAttribute)(LayoutDirectionService.Instance.IsFakingDirection() ? ((LayoutDirectionService.Instance.LayoutDirection == LayoutDirection.Ltr) ? 3 : 4) : 0);
            NavigationBar.SemanticContentAttribute = semanticContentAttribute;
            foreach (object item in NavigationBar)
            {
                UIView uIView = item as UIView;
                if (uIView != null)
                {
                    uIView.SetNeedsLayout();
                    foreach (object item2 in uIView)
                    {
                        (item2 as UIView)?.SetNeedsLayout();
                    }
                }
            }
            NavigationBar.SetNeedsLayout();
            if (View != null)
            {
                View.SemanticContentAttribute = semanticContentAttribute;
            }
        }

        private void OnLayoutDirectionChanged(object sender, EventArgs e)
        {
            UpdateLayoutDirection();
        }
    }
}
