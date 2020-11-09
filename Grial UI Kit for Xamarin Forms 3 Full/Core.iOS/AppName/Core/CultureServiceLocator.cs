namespace AppName.Core
{
	public class CultureServiceLocator : ICultureServiceLocator
	{
		public ICultureService Service => CultureService.Instance;
	}
}
