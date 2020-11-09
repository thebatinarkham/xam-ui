using System.Windows.Input;
using UIKit;
using AppName.Core;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AppName.Core
{
    internal class LongPressEffect : PlatformEffect
    {
        private class LongPressRecognizer : UILongPressGestureRecognizer
        {
            private Element element;

            public override UIGestureRecognizerState State
            {
                get
                {
                    return base.State;
                }
                set
                {
                    base.State = value;
                    if (value != UIGestureRecognizerState.Began)
                    {
                        return;
                    }
                    ICommand longPressCommand = Events.GetLongPressCommand(element);
                    if (longPressCommand != null)
                    {
                        object longPressCommandParameter = Events.GetLongPressCommandParameter(element);
                        if (longPressCommand.CanExecute(longPressCommandParameter))
                        {
                            longPressCommand.Execute(longPressCommandParameter);
                        }
                    }
                }
            }

            public LongPressRecognizer(Element element)
            {
                this.element = element;
            }
        }

        private UIView view;

        private LongPressRecognizer touchRecognizer;

        protected override void OnAttached()
        {
            view = ((base.Control == null) ? base.Container : base.Control);
            if (view != null)
            {
                touchRecognizer = new LongPressRecognizer(base.Element);
                view.AddGestureRecognizer(touchRecognizer);
            }
        }

        protected override void OnDetached()
        {
            if (touchRecognizer != null)
            {
                view.RemoveGestureRecognizer(touchRecognizer);
            }
        }
    }
}
