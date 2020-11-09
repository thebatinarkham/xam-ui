using Xamarin.Forms;

namespace AppName.Core
{
	[ContentProperty("DataTemplate")]
	public class IntMemberTemplateSelectorItem
	{
		public DataTemplate DataTemplate
		{
			get;
			set;
		}

		public int Value
		{
			get;
			set;
		}
	}
}
