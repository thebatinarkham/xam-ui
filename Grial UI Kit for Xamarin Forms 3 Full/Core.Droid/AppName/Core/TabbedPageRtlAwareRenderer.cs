using Android.Content;
using Android.Support.Design.Widget;
using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

namespace AppName.Core
{
    public class TabbedPageRtlAwareRenderer : TabbedPageRenderer
    {
        private FieldInfo _tabLayoutField;

        private TabLayout _tabLayout;

        public TabbedPageRtlAwareRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);
            Load();
            UpdateLayoutDirection();
            LayoutDirectionService.Instance.LayoutDirectionChanged -= OnLayoutDirectionChanged;
            LayoutDirectionService.Instance.LayoutDirectionChanged += OnLayoutDirectionChanged;
        }

        protected override void Dispose(bool disposing)
        {
            LayoutDirectionService.Instance.LayoutDirectionChanged -= OnLayoutDirectionChanged;
            base.Dispose(disposing);
        }

        private void OnLayoutDirectionChanged(object sender, EventArgs e)
        {
            Load();
            UpdateLayoutDirection();
        }

        private void Load()
        {
            if (_tabLayoutField == null)
            {
                _tabLayoutField = typeof(TabbedPageRenderer).GetField("_tabLayout", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            }
            if (_tabLayoutField != null)
            {
                _tabLayout = (_tabLayoutField.GetValue(this) as TabLayout);
            }
        }

        private void UpdateLayoutDirection()
        {
            if (_tabLayout != null)
            {
                LayoutDirection layoutDirection = LayoutDirectionService.Instance.LayoutDirection;
                _tabLayout.LayoutDirection = ((layoutDirection != 0) ? Android.Views.LayoutDirection.Rtl : Android.Views.LayoutDirection.Ltr);
            }
        }
    }
}
