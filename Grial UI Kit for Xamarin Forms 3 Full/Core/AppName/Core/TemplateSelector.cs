using System;
using System.Collections.ObjectModel;
using System.Reflection;
using Xamarin.Forms;

namespace AppName.Core
{
	[ContentProperty("Items")]
	public class TemplateSelector : DataTemplateSelector
	{
		private DataTemplate _defaultDataTemplate;

		public DataTemplate DefaultDataTemplate
		{
			get
			{
				return _defaultDataTemplate ?? BuildDefault();
			}
			set
			{
				_defaultDataTemplate = value;
			}
		}

		public Collection<TemplateSelectorItem> Items
		{
			get;
			set;
		}

		public TemplateSelector()
		{
			Items = new Collection<TemplateSelectorItem>();
		}

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			if (item == null)
			{
				return DefaultDataTemplate;
			}
			Type type = item.GetType();
			bool flag = type.FullName == "UXDivers.Gorilla.Platform.JsonModel";
			foreach (TemplateSelectorItem item2 in Items)
			{
				if ((!(item2.TargetType == null) || !string.IsNullOrEmpty(item2.HasMember)) && (flag || !(item2.TargetType != null) || !(item2.TargetType != type)) && (string.IsNullOrEmpty(item2.HasMember) || ReflectionHelper.TryGetPropertyOrField(item, item2.HasMember, out PropertyInfo _, out FieldInfo _)))
				{
					return item2.DataTemplate;
				}
			}
			return DefaultDataTemplate;
		}

		protected virtual DataTemplate BuildDefault()
		{
			return new DataTemplate(() => new Label
			{
				Text = "No matching template."
			});
		}
	}
}
