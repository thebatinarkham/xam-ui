using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace AppName.Core
{
	[ContentProperty("Items")]
	public class IntMemberTemplateSelector : DataTemplateSelector
	{
		public string MemberName
		{
			get;
			set;
		}

		public Collection<IntMemberTemplateSelectorItem> Items
		{
			get;
			set;
		}

		public IntMemberTemplateSelector()
		{
			Items = new Collection<IntMemberTemplateSelectorItem>();
		}

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			if (item != null && ReflectionHelper.TryGetPropertyOrFieldValue(item, MemberName, out object value) && value is int)
			{
				int value2 = (int)value;
				return Find(value2);
			}
			return Find(0);
		}

		private DataTemplate Find(int value)
		{
			foreach (IntMemberTemplateSelectorItem item in Items)
			{
				if (item.Value == value)
				{
					return item.DataTemplate;
				}
			}
			return new DataTemplate(() => new Label
			{
				Text = "No matching template."
			});
		}
	}
}
