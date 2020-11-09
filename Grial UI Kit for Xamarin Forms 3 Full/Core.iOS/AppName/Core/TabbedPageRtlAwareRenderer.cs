using System;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace AppName.Core
{
    public class TabbedPageRtlAwareRenderer : TabbedRenderer
    {
        public TabbedPageRtlAwareRenderer()
        {
            UpdateLayoutDirection();
            LayoutDirectionService.Instance.LayoutDirectionChanged += OnLayoutDirectionChanged;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            LayoutDirectionService.Instance.LayoutDirectionChanged -= OnLayoutDirectionChanged;
        }

        private void UpdateLayoutDirection()
        {
            if (!LayoutDirectionService.Instance.IsFakingDirection())
            {
                TabBar.SemanticContentAttribute = UISemanticContentAttribute.Unspecified;
            }
            else
            {
                TabBar.SemanticContentAttribute = (UISemanticContentAttribute)((LayoutDirectionService.Instance.LayoutDirection == LayoutDirection.Ltr) ? 3 : 4);
            }
        }

        private void OnLayoutDirectionChanged(object sender, EventArgs e)
        {
            UpdateLayoutDirection();
        }
    }
}
