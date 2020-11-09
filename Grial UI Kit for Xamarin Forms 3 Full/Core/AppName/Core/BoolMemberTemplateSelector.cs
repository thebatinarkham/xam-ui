using Xamarin.Forms;

namespace AppName.Core
{
    public class BoolMemberTemplateSelector : DataTemplateSelector
	{
		public string MemberName
		{
			get;
			set;
		}

		public DataTemplate TrueDataTemplate
		{
			get;
			set;
		}

		public DataTemplate FalseDataTemplate
		{
			get;
			set;
		}

		public BoolMemberTemplateSelector()
		{
		}

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			if (item != null && ReflectionHelper.TryGetPropertyOrFieldValue(item, MemberName, out object value))
			{
				bool flag = default(bool);
				int num;
				if (value is bool)
				{
					flag = (bool)value;
					num = 1;
				}
				else
				{
					num = 0;
				}
				if ((num & (flag ? 1 : 0)) != 0)
				{
					return TrueDataTemplate;
				}
			}
			return FalseDataTemplate;
		}
	}
}
