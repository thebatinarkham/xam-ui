using System.Reflection;
using Xamarin.Forms;

namespace AppName.Core
{
	public class AnimatedFontSize : AnimatedDouble
	{
		private BindableProperty _targetProperty;

		private bool _propertyLookupFailed;

		protected override void OnAttachedTo(VisualElement bindable)
		{
			base.OnAttachedTo(bindable);
			_targetProperty = null;
			_propertyLookupFailed = false;
		}

		protected override void SetPropertyValue(double value)
		{
			if (_targetProperty == null && !_propertyLookupFailed)
			{
				if (base.Target is Label)
				{
					_targetProperty = Label.FontSizeProperty;
				}
				else if (base.Target is Button)
				{
					_targetProperty = Button.FontSizeProperty;
				}
				else if (base.Target is Entry)
				{
					_targetProperty = Entry.FontSizeProperty;
				}
				else if (base.Target is Editor)
				{
					_targetProperty = Editor.FontSizeProperty;
				}
				else if (base.Target is SearchBar)
				{
					_targetProperty = SearchBar.FontSizeProperty;
				}
				else if (base.Target is Picker)
				{
					_targetProperty = Picker.FontSizeProperty;
				}
				else if (base.Target is DatePicker)
				{
					_targetProperty = DatePicker.FontSizeProperty;
				}
				else
				{
					try
					{
						if (ReflectionHelper.TryGetPropertyOrField(base.Target, "FontSizeProperty", out PropertyInfo property, out FieldInfo _) && property != null)
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
