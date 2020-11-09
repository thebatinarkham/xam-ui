using System;
using Xamarin.Forms;

namespace AppName.Core
{
	public abstract class AnimatedIntervalBaseBehavior : AnimatedBaseBehavior
	{
		public static readonly BindableProperty EasingProperty = BindableProperty.Create("Easing", typeof(EasingType), typeof(AnimatedIntervalBaseBehavior), EasingType.Linear, BindingMode.OneWay, null, AnimatedBaseBehavior.OnChanged);

		public static readonly BindableProperty ProgressStartProperty = BindableProperty.Create("ProgressStart", typeof(double), typeof(AnimatedIntervalBaseBehavior), 0.0, BindingMode.OneWay, null, AnimatedBaseBehavior.OnChanged);

		public static readonly BindableProperty ProgressEndProperty = BindableProperty.Create("ProgressEnd", typeof(double), typeof(AnimatedIntervalBaseBehavior), 1.0, BindingMode.OneWay, null, AnimatedBaseBehavior.OnChanged);

		public static readonly BindableProperty IsSymmetricProperty = BindableProperty.Create("IsSymmetric", typeof(bool), typeof(AnimatedDouble), false, BindingMode.OneWay, null, AnimatedBaseBehavior.OnChanged);

		public EasingType Easing
		{
			get
			{
				return (EasingType)GetValue(EasingProperty);
			}
			set
			{
				SetValue(EasingProperty, value);
			}
		}

		public double ProgressStart
		{
			get
			{
				return (double)GetValue(ProgressStartProperty);
			}
			set
			{
				SetValue(ProgressStartProperty, value);
			}
		}

		public double ProgressEnd
		{
			get
			{
				return (double)GetValue(ProgressEndProperty);
			}
			set
			{
				SetValue(ProgressEndProperty, value);
			}
		}

		public bool IsSymmetric
		{
			get
			{
				return (bool)GetValue(IsSymmetricProperty);
			}
			set
			{
				SetValue(IsSymmetricProperty, value);
			}
		}

		protected T Interpolate<T>(T start, T end, Func<double, T> percentualConvert)
		{
			double num = base.Progress ?? ProgressStart;
			double num2 = (ProgressStart == 0.0 && IsSymmetric) ? Math.Abs(num) : num;
			if (num2 <= ProgressStart)
			{
				return start;
			}
			if (num2 >= ProgressEnd)
			{
				return end;
			}
			double p = (num2 - ProgressStart) / (ProgressEnd - ProgressStart);
			p = ApplyEasing(p);
			return percentualConvert(p);
		}

		private double ApplyEasing(double p)
		{
			switch (Easing)
			{
			case EasingType.BounceIn:
				return Xamarin.Forms.Easing.BounceIn.Ease(p);
			case EasingType.BounceOut:
				return Xamarin.Forms.Easing.BounceOut.Ease(p);
			case EasingType.CubicIn:
				return Xamarin.Forms.Easing.CubicIn.Ease(p);
			case EasingType.CubicInOut:
				return Xamarin.Forms.Easing.CubicInOut.Ease(p);
			case EasingType.CubicOut:
				return Xamarin.Forms.Easing.CubicOut.Ease(p);
			case EasingType.SinIn:
				return Xamarin.Forms.Easing.SinIn.Ease(p);
			case EasingType.SinInOut:
				return Xamarin.Forms.Easing.SinInOut.Ease(p);
			case EasingType.SinOut:
				return Xamarin.Forms.Easing.SinOut.Ease(p);
			case EasingType.SpringIn:
				return Xamarin.Forms.Easing.SpringIn.Ease(p);
			default:
				return p;
			}
		}
	}
}
