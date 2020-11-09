using System;
using System.Threading.Tasks;
using Xamanimation;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public class Reset : AnimationBase
    {
        public static readonly BindableProperty OpacityProperty =
            BindableProperty.Create(
                nameof(Opacity),
                typeof(double?),
                typeof(Reset),
                null);

        public double? Opacity
        {
            get { return (double?)GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }

        public static readonly BindableProperty RotationProperty =
            BindableProperty.Create(
                nameof(Rotation),
                typeof(double?),
                typeof(Reset),
                null);

        public double? Rotation
        {
            get { return (double?)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
        }

        public static readonly BindableProperty ScaleProperty =
           BindableProperty.Create(
               nameof(Scale),
               typeof(double?),
               typeof(Reset),
               null);

        public double? Scale
        {
            get { return (double?)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        public static readonly BindableProperty TranslateXProperty =
           BindableProperty.Create(
               nameof(TranslateXProperty),
               typeof(double?),
               typeof(Reset),
               null);

        public double? TranslateX
        {
            get { return (double?)GetValue(TranslateXProperty); }
            set { SetValue(TranslateXProperty, value); }
        }

        public static readonly BindableProperty TranslateYProperty =
           BindableProperty.Create(
               nameof(TranslateYProperty),
               typeof(double?),
               typeof(Reset),
               null);

        public double? TranslateY
        {
            get { return (double?)GetValue(TranslateYProperty); }
            set { SetValue(TranslateYProperty, value); }
        }

        protected override Task BeginAnimation()
        {
            if (Target != null)
            {
                if (Opacity is double opacity)
                {
                    Target.Opacity = opacity;
                }

                if (Rotation is double rotation)
                {
                    Target.Rotation = rotation;
                }

                if (Scale is double scale)
                {
                    Target.Scale = scale;
                }

                if (TranslateX is double translateX)
                {
                    Target.TranslationX = translateX;
                }

                if (TranslateY is double translateY)
                {
                    Target.TranslationY = translateY;
                }
            }

            return Task.CompletedTask;
        }
    }
}
