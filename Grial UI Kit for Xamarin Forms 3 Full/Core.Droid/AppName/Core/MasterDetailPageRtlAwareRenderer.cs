using Android.Content;
using Android.Views;
using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

namespace AppName.Core
{
    public class MasterDetailPageRtlAwareRenderer : MasterDetailPageRenderer
    {
        private bool _hasDirectionType = true;

        private Type _flowDirectionType;

        private PropertyInfo _flowDirectionProperty;

        private VisualElement _elementForFlowDirection;

        private FieldInfo _masterField;

        private PropertyInfo _elementProperty;

        private ViewGroup _masterLayout;

        private IMasterDetailPageController _element;

        private bool _isFakingLayoutDirection;

        private double _masterWidth;

        public MasterDetailPageRtlAwareRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(VisualElement oldElement, VisualElement newElement)
        {
            _elementForFlowDirection = newElement;
            base.OnElementChanged(oldElement, newElement);
            Load();
            UpdateLayoutDirection();
            LayoutDirectionService.Instance.LayoutDirectionChanged -= OnLayoutDirectionChanged;
            LayoutDirectionService.Instance.LayoutDirectionChanged += OnLayoutDirectionChanged;
        }

        public override void CloseDrawer(int gravity)
        {
            if (_isFakingLayoutDirection && gravity == 8388611)
            {
                gravity = 8388613;
            }
            base.CloseDrawer(gravity);
        }

        public override void OpenDrawer(int gravity)
        {
            if (_isFakingLayoutDirection && gravity == 8388611)
            {
                gravity = 8388613;
            }
            base.OpenDrawer(gravity);
        }

        protected override void Dispose(bool disposing)
        {
            LayoutDirectionService.Instance.LayoutDirectionChanged -= OnLayoutDirectionChanged;
            base.Dispose(disposing);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            if (_element != null && _element.MasterBounds.Width != _masterWidth)
            {
                _masterWidth = _element.MasterBounds.Width;
                UpdateLayoutDirection();
            }
        }

        private void OnLayoutDirectionChanged(object sender, EventArgs e)
        {
            Load();
            UpdateLayoutDirection();
        }

        private void Load()
        {
            if (_masterField == null)
            {
                _masterField = typeof(MasterDetailPageRenderer).GetField("_masterLayout", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            }
            if (_elementProperty == null)
            {
                _elementProperty = typeof(MasterDetailPageRenderer).GetProperty("MasterDetailPageController", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            }
            if (_hasDirectionType && _flowDirectionProperty == null)
            {
                _flowDirectionProperty = typeof(VisualElement).GetProperty("FlowDirection", BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                _flowDirectionType = typeof(VisualElement).Assembly.GetType("Xamarin.Forms.FlowDirection");
                if (_flowDirectionType == null || _flowDirectionProperty == null)
                {
                    _hasDirectionType = false;
                }
            }
            if (_masterField != null)
            {
                _masterLayout = (_masterField.GetValue(this) as ViewGroup);
            }
            if (_elementProperty != null)
            {
                _element = (_elementProperty.GetValue(this) as IMasterDetailPageController);
            }
            if (_elementForFlowDirection != null && _hasDirectionType)
            {
                if (LayoutDirectionService.Instance.DeviceNativeDirection == AppName.Core.LayoutDirection.Rtl)
                {
                    _flowDirectionProperty.SetValue(_elementForFlowDirection, Enum.Parse(_flowDirectionType, "RightToLeft"));
                }
                else
                {
                    _flowDirectionProperty.SetValue(_elementForFlowDirection, Enum.Parse(_flowDirectionType, "LeftToRight"));
                }
            }
        }

        private void UpdateLayoutDirection()
        {
            if (_masterLayout != null)
            {
                _isFakingLayoutDirection = LayoutDirectionService.Instance.IsFakingDirection();
                LayoutParams layoutParams = new LayoutParams(-2, -2)
                {
                    Gravity = (_isFakingLayoutDirection ? 8388613 : 8388611)
                };
                double num = _masterWidth * (double)Math.Max(1f, Resources.DisplayMetrics.Density);
                if (num > 0.0)
                {
                    layoutParams.Width = (int)num;
                }
                _masterLayout.LayoutParameters = layoutParams;
            }
        }
    }
}
