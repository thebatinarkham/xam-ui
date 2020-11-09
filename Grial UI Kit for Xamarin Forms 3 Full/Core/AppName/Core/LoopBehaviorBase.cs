using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppName.Core
{
    public abstract class LoopBehaviorBase : Behavior<VisualElement>
	{
		protected class State
		{
			public bool IsRunning
			{
				get;
				set;
			}
		}

		private State _state;

		private VisualElement _target;

		public static BindableProperty IsRunningProperty = BindableProperty.Create("IsRunning", typeof(bool), typeof(LoopBehaviorBase), false, BindingMode.OneWay, null, OnIsRunningChanged);

		public static BindableProperty LoopDurationProperty = BindableProperty.Create("LoopDuration", typeof(int), typeof(LoopBehaviorBase), -1, BindingMode.OneWay, null, OnIsRunningChanged);

		public bool IsRunning
		{
			get
			{
				return (bool)GetValue(IsRunningProperty);
			}
			set
			{
				SetValue(IsRunningProperty, value);
			}
		}

		public int LoopDuration
		{
			get
			{
				return (int)GetValue(LoopDurationProperty);
			}
			set
			{
				SetValue(LoopDurationProperty, value);
			}
		}

		protected LoopBehaviorBase()
		{
		}

		private static void OnIsRunningChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((LoopBehaviorBase)bindable).Update();
		}

		protected override void OnAttachedTo(VisualElement bindable)
		{
			base.OnAttachedTo(bindable);
			if (bindable != null)
			{
				_target = bindable;
				_target.PropertyChanged += OnTargetPropertyChanged;
				InitializeTarget(_target);
				Update();
			}
		}

		protected override void OnDetachingFrom(VisualElement bindable)
		{
			if (_state != null)
			{
				_state.IsRunning = false;
				_state = null;
			}
			if (_target != null)
			{
				_target.PropertyChanged += OnTargetPropertyChanged;
				_target = null;
			}
			base.OnDetachingFrom(bindable);
		}

		protected virtual void InitializeTarget(VisualElement target)
		{
		}

		private void OnTargetPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Renderer" && !InVisualTree())
			{
				IsRunning = false;
			}
		}

		private void Update()
		{
			if (_target != null)
			{
				if (_state != null)
				{
					_state.IsRunning = false;
					_state = null;
				}
				if (IsRunning)
				{
					_state = new State
					{
						IsRunning = true
					};
					Animate(_target, _state);
				}
			}
		}

		protected abstract Task Animate(VisualElement target, State state);

		private bool InVisualTree()
		{
			Element parent = _target.Parent;
			while (parent != null)
			{
				parent = parent.Parent;
				if (parent is Application)
				{
					return true;
				}
			}
			return false;
		}
	}
}
