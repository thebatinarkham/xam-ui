using Xamarin.Forms;

namespace AppName.Core
{
    public abstract class AnimatedBaseBehavior : Behavior<VisualElement>
	{
		public static readonly BindableProperty IsEnabledProperty = BindableProperty.Create("IsEnabled", typeof(bool), typeof(AnimatedBaseBehavior), true, BindingMode.OneWay, null, OnChanged);

		public static readonly BindableProperty ProgressProperty = BindableProperty.Create("Progress", typeof(double?), typeof(AnimatedBaseBehavior), null, BindingMode.OneWay, null, OnChanged);

		public bool IsEnabled
		{
			get
			{
				return (bool)GetValue(IsEnabledProperty);
			}
			set
			{
				SetValue(IsEnabledProperty, value);
			}
		}

		public double? Progress
		{
			get
			{
				return (double?)GetValue(ProgressProperty);
			}
			set
			{
				SetValue(ProgressProperty, value);
			}
		}

		public VisualElement Target
		{
			get;
			private set;
		}

		protected AnimatedBaseBehavior()
		{
		}

		protected static void OnChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((AnimatedBaseBehavior)bindable).Update();
		}

		protected void Update()
		{
			if (IsEnabled && Target != null && Progress.HasValue)
			{
				OnUpdate();
			}
		}

		protected abstract void OnUpdate();

		protected override void OnAttachedTo(VisualElement bindable)
		{
			Target = bindable;
			base.OnAttachedTo(bindable);
			Update();
		}

		protected override void OnDetachingFrom(VisualElement bindable)
		{
			base.OnDetachingFrom(bindable);
			Target = null;
		}
	}
}
