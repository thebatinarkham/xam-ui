using System;
using System.ComponentModel;
using Xamarin.Forms.Platform.Android;

namespace AppName.Core
{
    public abstract class BaseEffect : PlatformEffect
    {
        protected bool Attached
        {
            get;
            private set;
        }

        protected BaseEffect()
        {
        }

        protected sealed override void OnAttached()
        {
            if (CanBeApplied())
            {
                Attached = true;
                try
                {
                    OnAttachedInternal();
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }

        protected sealed override void OnDetached()
        {
            if (Attached)
            {
                Attached = false;
                try
                {
                    OnDetachedInternal();
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }

        protected sealed override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            if (Attached)
            {
                try
                {
                    OnElementPropertyChangedInternal(args.PropertyName);
                }
                catch (ObjectDisposedException)
                {
                }
            }
        }

        protected virtual void OnElementPropertyChangedInternal(string propertyName)
        {
        }

        protected virtual bool CanBeApplied()
        {
            return true;
        }

        protected virtual void OnAttachedInternal()
        {
        }

        protected virtual void OnDetachedInternal()
        {
        }
    }
}
