using Xamarin.Forms;

namespace AppName.Core
{
    public class ScrollViewScrollBehavior : Behavior<ScrollView>
	{
		private static readonly BindablePropertyKey ViewportHeightPropertyKey = BindableProperty.CreateReadOnly("ViewportHeight", typeof(double), typeof(ScrollViewScrollBehavior), 0.0);

		public static readonly BindableProperty ViewportHeightProperty = ViewportHeightPropertyKey.BindableProperty;

		private static readonly BindablePropertyKey ViewportWidthPropertyKey = BindableProperty.CreateReadOnly("ViewportWidth", typeof(double), typeof(ScrollViewScrollBehavior), 0.0);

		public static readonly BindableProperty ViewportWidthProperty = ViewportWidthPropertyKey.BindableProperty;

		private static readonly BindablePropertyKey RelativeScrollXPropertyKey = BindableProperty.CreateReadOnly("RelativeScrollX", typeof(double), typeof(ScrollViewScrollBehavior), 0.0);

		public static readonly BindableProperty RelativeScrollXProperty = RelativeScrollXPropertyKey.BindableProperty;

		private static readonly BindablePropertyKey RelativeScrollYPropertyKey = BindableProperty.CreateReadOnly("RelativeScrollY", typeof(double), typeof(ScrollViewScrollBehavior), 0.0);

		public static readonly BindableProperty RelativeScrollYProperty = RelativeScrollYPropertyKey.BindableProperty;

		private static readonly BindablePropertyKey AbsoluteScrollXPropertyKey = BindableProperty.CreateReadOnly("AbsoluteScrollX", typeof(double), typeof(ScrollViewScrollBehavior), 0.0);

		public static readonly BindableProperty AbsoluteScrollXProperty = AbsoluteScrollXPropertyKey.BindableProperty;

		private static readonly BindablePropertyKey AbsoluteScrollYPropertyKey = BindableProperty.CreateReadOnly("AbsoluteScrollY", typeof(double), typeof(ScrollViewScrollBehavior), 0.0);

		public static readonly BindableProperty AbsoluteScrollYProperty = AbsoluteScrollYPropertyKey.BindableProperty;

		public double ViewportHeight
		{
			get
			{
				return (double)GetValue(ViewportHeightProperty);
			}
			private set
			{
				SetValue(ViewportHeightPropertyKey, value);
			}
		}

		public double ViewportWidth
		{
			get
			{
				return (double)GetValue(ViewportWidthProperty);
			}
			private set
			{
				SetValue(ViewportWidthPropertyKey, value);
			}
		}

		public double RelativeScrollX
		{
			get
			{
				return (double)GetValue(RelativeScrollXProperty);
			}
			private set
			{
				SetValue(RelativeScrollXPropertyKey, value);
			}
		}

		public double RelativeScrollY
		{
			get
			{
				return (double)GetValue(RelativeScrollYProperty);
			}
			private set
			{
				SetValue(RelativeScrollYPropertyKey, value);
			}
		}

		public double AbsoluteScrollX
		{
			get
			{
				return (double)GetValue(AbsoluteScrollXProperty);
			}
			private set
			{
				SetValue(AbsoluteScrollXPropertyKey, value);
			}
		}

		public double AbsoluteScrollY
		{
			get
			{
				return (double)GetValue(AbsoluteScrollYProperty);
			}
			private set
			{
				SetValue(AbsoluteScrollYPropertyKey, value);
			}
		}

		public ScrollViewScrollBehavior()
		{
		}

		protected override void OnAttachedTo(ScrollView bindable)
		{
			base.OnAttachedTo(bindable);
			bindable.Scrolled += OnScrolled;
		}

		protected override void OnDetachingFrom(ScrollView bindable)
		{
			base.OnDetachingFrom(bindable);
			bindable.Scrolled -= OnScrolled;
		}

		private void OnScrolled(object sender, ScrolledEventArgs e)
		{
			ScrollView scrollView = (ScrollView)sender;
			ViewportHeight = scrollView.ContentSize.Height - scrollView.Height;
			RelativeScrollY = ((ViewportHeight <= 0.0) ? 0.0 : (e.ScrollY / ViewportHeight));
			AbsoluteScrollY = e.ScrollY;
			ViewportWidth = scrollView.ContentSize.Width - scrollView.Width;
			RelativeScrollX = ((ViewportWidth <= 0.0) ? 0.0 : (e.ScrollX / ViewportWidth));
			AbsoluteScrollX = e.ScrollX;
		}
	}
}
