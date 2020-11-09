using System;
using System.ComponentModel;
using Xamarin.Forms.Platform.iOS;

namespace AppName.Core
{
    public class CornerRadiusEffect : PlatformEffect
    {
        private nfloat _originalRadius;

        private nfloat _originalControlRadius;

        protected override void OnAttached()
        {
            if (base.Container != null)
            {
                _originalRadius = base.Container.Layer.CornerRadius;
                if (base.Control != null)
                {
                    _originalControlRadius = base.Control.Layer.CornerRadius;
                    base.Control.ClipsToBounds = true;
                }
                else
                {
                    base.Container.ClipsToBounds = true;
                }
                UpdateCorner();
            }
        }

        protected override void OnDetached()
        {
            if (base.Container == null)
            {
                return;
            }
            if (base.Container.Layer != null)
            {
                base.Container.Layer.CornerRadius = _originalRadius;
            }
            if (base.Control != null)
            {
                if (base.Control.Layer != null)
                {
                    base.Control.Layer.CornerRadius = _originalControlRadius;
                }
                base.Control.ClipsToBounds = false;
            }
            else
            {
                base.Container.ClipsToBounds = false;
            }
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            if (args.PropertyName == Effects.CornerRadiusProperty.PropertyName)
            {
                UpdateCorner();
            }
        }

        private void UpdateCorner()
        {
            nfloat cornerRadius = (nfloat)Effects.GetCornerRadius(base.Element);
            base.Container.Layer.CornerRadius = cornerRadius;
            if (base.Control != null)
            {
                base.Control.Layer.CornerRadius = cornerRadius;
            }
        }
    }
}
