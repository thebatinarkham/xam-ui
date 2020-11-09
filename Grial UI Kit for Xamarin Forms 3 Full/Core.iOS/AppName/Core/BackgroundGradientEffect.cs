using CoreAnimation;
using CoreGraphics;
using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AppName.Core
{
    public class BackgroundGradientEffect : PlatformEffect
    {
        private CAGradientLayer _gradientLayer;

        private Gradient _gradient;

        private GradientChangeListener _listener;

        protected override void OnAttached()
        {
            _gradient = Effects.GetBackgroundGradient(base.Element);
            if (_gradient != null)
            {
                _listener = new GradientChangeListener(_gradient);
                _listener.Updated += OnGradientUpdated;
                UpdateLayerGradient();
                UpdateLayerLayout();
            }
        }

        protected override void OnDetached()
        {
            ClearLayer();
            if (_listener != null)
            {
                _listener.Updated -= OnGradientUpdated;
                _listener = null;
            }
            _gradient = null;
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            if (_gradientLayer != null && (args.PropertyName == "Width" || args.PropertyName == "Height" || args.PropertyName == "CornerRadius" || args.PropertyName == "BorderRadius" || args.PropertyName == Gradient.BackgroundGradientForcedHeightProperty.PropertyName))
            {
                UpdateLayerLayout();
            }
        }

        private void OnGradientUpdated(object sender, EventArgs e)
        {
            UpdateLayerGradient();
        }

        private void UpdateLayerLayout()
        {
            if (_gradientLayer == null)
            {
                return;
            }
            UIView targetElement = GetTargetElement();
            double backgroundGradientForcedHeight = Gradient.GetBackgroundGradientForcedHeight(base.Element);
            _gradientLayer.Frame = new CGRect(0.0, 0.0, ((VisualElement)base.Element).Width, (backgroundGradientForcedHeight > 0.0) ? backgroundGradientForcedHeight : ((VisualElement)base.Element).Height);
            _gradientLayer.CornerRadius = targetElement.Layer.CornerRadius;
            BoxView boxView = base.Element as BoxView;
            if (boxView != null)
            {
                CornerRadius cornerRadius = boxView.CornerRadius;
                if (cornerRadius != default(CornerRadius))
                {
                    if (cornerRadius.BottomLeft == cornerRadius.TopLeft && cornerRadius.TopRight == cornerRadius.BottomRight && cornerRadius.BottomLeft == cornerRadius.BottomRight)
                    {
                        _gradientLayer.CornerRadius = (nfloat)cornerRadius.BottomLeft;
                        _gradientLayer.Mask = null;
                    }
                    else
                    {
                        UIBezierPath uIBezierPath = new UIBezierPath();
                        float num = 0f;
                        float num2 = 0f;
                        float num3 = (float)boxView.Width;
                        float num4 = (float)boxView.Height;
                        float num5 = (float)cornerRadius.TopRight;
                        float num6 = (float)cornerRadius.TopLeft;
                        float num7 = (float)cornerRadius.BottomLeft;
                        float num8 = (float)cornerRadius.BottomRight;
                        uIBezierPath.AddArc(new CGPoint(num + num3 - num5, num2 + num5), num5, 4.712389f, MathF.PI * 2f, clockWise: true);
                        uIBezierPath.AddArc(new CGPoint(num + num3 - num8, num2 + num4 - num8), num8, 0, MathF.PI / 2f, clockWise: true);
                        uIBezierPath.AddArc(new CGPoint(num + num7, num2 + num4 - num7), num7, MathF.PI / 2f, MathF.PI, clockWise: true);
                        uIBezierPath.AddArc(new CGPoint(num + num6, num2 + num6), num6, MathF.PI, 4.712389f, clockWise: true);
                        _gradientLayer.Mask = new CAShapeLayer
                        {
                            Path = uIBezierPath.CGPath
                        };
                    }
                }
                else
                {
                    _gradientLayer.Mask = null;
                }
            }
            GradientFactory.UpdateRadialRadius(_gradient, _gradientLayer);
        }

        private UIView GetTargetElement()
        {
            if (base.Element is CardView)
            {
                UIView[] subviews = base.Container.Subviews;
                if (subviews != null && subviews.Length == 1)
                {
                    return base.Container.Subviews[0];
                }
            }
            if (base.Control == null || typeof(Label).IsAssignableFrom(base.Element.GetType()))
            {
                return base.Container;
            }
            return base.Control;
        }

        private void EnsureLayerExistence()
        {
            if (_gradient.Colors.Count == 0)
            {
                ClearLayer();
            }
            else if (_gradientLayer == null)
            {
                _gradientLayer = new CAGradientLayer();
                GetTargetElement().Layer.InsertSublayer(_gradientLayer, 0);
            }
        }

        private void ClearLayer()
        {
            if (_gradientLayer != null)
            {
                _gradientLayer.RemoveFromSuperLayer();
                _gradientLayer.Dispose();
                _gradientLayer = null;
            }
        }

        private void UpdateLayerGradient()
        {
            EnsureLayerExistence();
            if (_gradientLayer != null)
            {
                GradientFactory.UpdateLayer(_gradient, _gradientLayer);
            }
        }
    }
}
