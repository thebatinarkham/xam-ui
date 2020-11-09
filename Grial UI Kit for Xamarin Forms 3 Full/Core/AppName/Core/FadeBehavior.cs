using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppName.Core
{
	internal class FadeBehavior : Behavior<VisualElement>
	{
		public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create("IsSelected", typeof(bool), typeof(FadeBehavior), false, BindingMode.Default, null, IsSelectedChanged);

		public VisualElement AssociatedObject
		{
			get;
			private set;
		}

		public bool IsSelected
		{
			get
			{
				return (bool)GetValue(IsSelectedProperty);
			}
			set
			{
				SetValue(IsSelectedProperty, value);
			}
		}

		public uint FadeInAnimationLength
		{
			get;
			set;
		}

		public uint FadeOutAnimationLength
		{
			get;
			set;
		}

		public FadeBehavior()
		{
			FadeInAnimationLength = 250u;
			FadeOutAnimationLength = 350u;
		}

		protected override void OnAttachedTo(VisualElement bindable)
		{
			base.OnAttachedTo(bindable);
			AssociatedObject = bindable;
			bindable.Opacity = 0.0;
			bindable.IsVisible = false;
		}

		private static void IsSelectedChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			FadeBehavior fadeBehavior = bindable as FadeBehavior;
			if (fadeBehavior != null && fadeBehavior.AssociatedObject != null)
			{
				fadeBehavior.Animate();
			}
		}

		private void Animate()
		{
			if (IsSelected)
			{
				AssociatedObject.IsVisible = true;
			}
			AssociatedObject.FadeTo(IsSelected ? 1 : 0, IsSelected ? FadeInAnimationLength : FadeOutAnimationLength, Easing.Linear).ContinueWith(delegate
			{
				if (!IsSelected)
				{
					AssociatedObject.IsVisible = false;
				}
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}
	}
}
