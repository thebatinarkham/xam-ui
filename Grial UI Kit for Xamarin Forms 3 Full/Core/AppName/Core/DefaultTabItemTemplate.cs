using Xamarin.Forms;

namespace AppName.Core
{
	internal class DefaultTabItemTemplate : StackLayout
	{
		private readonly Label _label;

		private readonly ContentPresenter _presenter;

		public DefaultTabItemTemplate()
		{
			base.VerticalOptions = LayoutOptions.Center;
			base.HorizontalOptions = LayoutOptions.Center;
			_label = new Label();
			_presenter = new ContentPresenter();
			base.Children.Add(_label);
			base.Children.Add(_presenter);
		}

		protected override void OnParentSet()
		{
			base.OnParentSet();
			TabItem tabItem = base.Parent as TabItem;
			if (tabItem != null)
			{
				if (tabItem.Text != null)
				{
					_label.Text = tabItem.Text;
				}
				_presenter.Content = tabItem.Header;
			}
		}
	}
}
