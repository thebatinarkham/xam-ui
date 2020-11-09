using Xamarin.Forms;

namespace AppName.Core
{
	public class ListViewTemplateSelector : TemplateSelector
	{
		protected override DataTemplate BuildDefault()
		{
			return new DataTemplate(() => new ViewCell
			{
				View = new Label
				{
					Text = "No matching template."
				}
			});
		}
	}
}
