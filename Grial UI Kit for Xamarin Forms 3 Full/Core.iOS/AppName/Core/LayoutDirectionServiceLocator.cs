namespace AppName.Core
{
	public class LayoutDirectionServiceLocator : ILayoutDirectionServiceLocator
	{
		public ILayoutDirectionService Service => LayoutDirectionService.Instance;
	}
}
