using System.Reflection;
using Xamarin.Forms;

namespace AppName.Core
{
	public class AnimatedTextColor : AnimatedColor
	{
		private BindableProperty _targetProperty;

		private bool _propertyLookupFailed;

		protected override void OnAttachedTo(VisualElement bindable)
		{
			base.OnAttachedTo(bindable);
			_targetProperty = null;
			_propertyLookupFailed = false;
		}

		protected override void SetPropertyValue(Color value)
		{
			if (_targetProperty == null && !_propertyLookupFailed)
			{
				if (base.Target is Label)
				{
					_targetProperty = Label.TextColorProperty;
				}
				else if (base.Target is Button)
				{
					_targetProperty = Button.TextColorProperty;
				}
				else if (base.Target is Entry)
				{
					_targetProperty = Entry.TextColorProperty;
				}
				else if (base.Target is Editor)
				{
					_targetProperty = Editor.TextColorProperty;
				}
				else if (base.Target is SearchBar)
				{
					_targetProperty = SearchBar.TextColorProperty;
				}
				else if (base.Target is Picker)
				{
					_targetProperty = Picker.TextColorProperty;
				}
				else if (base.Target is DatePicker)
				{
					_targetProperty = DatePicker.TextColorProperty;
				}
				else
				{
					try
					{
						if (ReflectionHelper.TryGetPropertyOrField(base.Target, "TextColorProperty", out PropertyInfo property, out FieldInfo _) && property != null)
						{
							_targetProperty = (property.GetValue(base.Target.GetType()) as BindableProperty);
						}
					}
					catch
					{
					}
					_propertyLookupFailed = (_targetProperty == null);
				}
			}
			if (_targetProperty != null)
			{
				base.Target.SetValue(_targetProperty, value);
			}
		}
	}
}
