using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.OS;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AppName.Core
{
    public class BackgroundGradientEffect : BaseEffect
    {
        private readonly Collection<Drawable> _currentDrawables = new Collection<Drawable>();

        private GradientChangeListener _listener;

        private Gradient _gradient;

        private Drawable _gradientBackground;

        private LayerDrawable _gradientBackgroundLayerDrawable;

        private Drawable _originalBackground;

        protected override void OnAttachedInternal()
        {
            _gradient = Effects.GetBackgroundGradient(base.Element);
            if (_gradient != null)
            {
                _listener = new GradientChangeListener(_gradient);
                _listener.Updated += OnGradientUpdated;
                UpdateGradient();
            }
        }

        protected override void OnDetachedInternal()
        {
            RestoreBackground();
            if (_listener != null)
            {
                _listener.Updated -= OnGradientUpdated;
                _listener = null;
            }
            CleanDrawables();
            _gradientBackgroundLayerDrawable = null;
            _gradient = null;
        }

        protected override void OnElementPropertyChangedInternal(string propertyName)
        {
            if (_gradientBackground != null && !(propertyName == "Renderer"))
            {
                if (propertyName == Gradient.BackgroundGradientForcedHeightProperty.PropertyName)
                {
                    UpdateGradient();
                }
                else if (propertyName == "CornerRadius")
                {
                    UpdateGradient();
                }
                else if (GetTargetElement().Background != _gradientBackground)
                {
                    UpdateGradient(forceRegenerateGradient: false);
                }
            }
        }

        private void OnGradientUpdated(object sender, EventArgs e)
        {
            UpdateGradient();
        }

        private Android.Views.View GetTargetElement()
        {
            return base.Control ?? base.Container;
        }

        private void RestoreBackground()
        {
            if (_originalBackground != null)
            {
                try
                {
                    GetTargetElement().Background = _originalBackground;
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }

        private void UpdateGradient(bool forceRegenerateGradient = true)
        {
            Android.Views.View targetElement = GetTargetElement();
            try
            {
                if (targetElement.Background != _gradientBackground)
                {
                    _originalBackground = targetElement.Background;
                }
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            if (_gradient.Colors.Count == 0)
            {
                CleanDrawables();
                RestoreBackground();
                return;
            }
            if (!forceRegenerateGradient && !(base.Element is Button) && Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                if (_originalBackground != null && _gradientBackgroundLayerDrawable != null)
                {
                    _gradientBackgroundLayerDrawable.SetDrawable(0, _originalBackground);
                }
                targetElement.Background = _gradientBackground;
                return;
            }
            CleanDrawables();
            double backgroundGradientForcedHeight = Gradient.GetBackgroundGradientForcedHeight(base.Element);
            PaintDrawable paintDrawable = (backgroundGradientForcedHeight >= 0.0) ? GradientFactory.GetDrawable(_gradient, targetElement.Context, backgroundGradientForcedHeight) : GradientFactory.GetDrawable(_gradient, targetElement.Context);
            if (paintDrawable == null)
            {
                RestoreBackground();
                return;
            }
            _currentDrawables.Add(paintDrawable);
            BoxView boxView = base.Element as BoxView;
            if (boxView != null && boxView.CornerRadius != default(CornerRadius))
            {
                float num = targetElement.Context.ToPixels(boxView.CornerRadius.TopLeft);
                float num2 = targetElement.Context.ToPixels(boxView.CornerRadius.TopRight);
                float num3 = targetElement.Context.ToPixels(boxView.CornerRadius.BottomLeft);
                float num4 = targetElement.Context.ToPixels(boxView.CornerRadius.BottomRight);
                paintDrawable.SetCornerRadii(new float[8]
                {
                    num,
                    num,
                    num2,
                    num2,
                    num4,
                    num4,
                    num3,
                    num3
                });
            }
            Drawable drawable = null;
            bool flag = true;
            Button button = base.Element as Button;
            if (button != null)
            {
                int num5 = (int)targetElement.Context.ToPixels(button.BorderWidth);
                float num6 = targetElement.Context.ToPixels(button.CornerRadius);
                if (num6 > 0f)
                {
                    paintDrawable.Shape = new RoundRectShape(new float[8]
                    {
                        num6,
                        num6,
                        num6,
                        num6,
                        num6,
                        num6,
                        num6,
                        num6
                    }, null, null);
                }
                float num7 = Math.Max(0f, num6 - (float)(num5 / 2));
                if (num5 > 0 || num7 > 0f)
                {
                    GradientDrawable gradientDrawable = new GradientDrawable();
                    gradientDrawable.SetStroke(num5, button.BorderColor.ToAndroid());
                    gradientDrawable.SetCornerRadius(num7);
                    _currentDrawables.Add(gradientDrawable);
                    GradientDrawable gradientDrawable2 = new GradientDrawable();
                    gradientDrawable2.SetColor(button.BackgroundColor.ToAndroid());
                    gradientDrawable2.SetCornerRadius(num6);
                    _currentDrawables.Add(gradientDrawable2);
                    drawable = new LayerDrawable(new Drawable[3]
                    {
                        gradientDrawable2,
                        paintDrawable,
                        gradientDrawable
                    });
                    _currentDrawables.Add(drawable);
                }
                if (_originalBackground is RippleDrawable)
                {
                    Android.Graphics.Color color = GetRippleColor(button, targetElement).AddLuminosity(-0.12).ToAndroid();
                    drawable = new RippleDrawable(ColorStateList.ValueOf(color), drawable ?? paintDrawable, null);
                    _currentDrawables.Add(drawable);
                }
                if (button.BackgroundColor == Xamarin.Forms.Color.Default)
                {
                    flag = false;
                }
            }
            if (drawable != null)
            {
                if (flag)
                {
                    _gradientBackground = new LayerDrawable(new Drawable[2]
                    {
                        _originalBackground,
                        drawable
                    });
                    _currentDrawables.Add(_gradientBackground);
                }
                else
                {
                    _gradientBackground = drawable;
                }
            }
            else if (_originalBackground == null)
            {
                _gradientBackground = paintDrawable;
            }
            else
            {
                _gradientBackgroundLayerDrawable = new LayerDrawable(new Drawable[2]
                {
                    _originalBackground,
                    paintDrawable
                });
                _currentDrawables.Add(_gradientBackgroundLayerDrawable);
                _gradientBackground = _gradientBackgroundLayerDrawable;
            }
            targetElement.Background = _gradientBackground;
        }

        private Xamarin.Forms.Color GetRippleColor(Button button, Android.Views.View targetView)
        {
            Xamarin.Forms.Color result = (button.BackgroundColor != Xamarin.Forms.Color.Default) ? button.BackgroundColor : Forms.GetColorButtonNormal(targetView.Context);
            for (int i = 0; i < _gradient.Colors.Count; i++)
            {
                Xamarin.Forms.Color finalColor = _gradient.Colors[i].FinalColor;
                if (!(finalColor == Xamarin.Forms.Color.Transparent))
                {
                    result = finalColor;
                    if (result.A < 0.5)
                    {
                        result = Xamarin.Forms.Color.FromRgba(result.R, result.G, result.B, 0.5);
                    }
                    break;
                }
            }
            return result;
        }

        private void CleanDrawables()
        {
            for (int i = 0; i < _currentDrawables.Count; i++)
            {
                _currentDrawables[i].Dispose();
            }
            _currentDrawables.Clear();
            _gradientBackground = null;
            _gradientBackgroundLayerDrawable = null;
        }
    }
}
