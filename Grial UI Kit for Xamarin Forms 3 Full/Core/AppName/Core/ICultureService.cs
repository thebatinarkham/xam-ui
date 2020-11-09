using System.Globalization;

namespace AppName.Core
{
	public interface ICultureService
	{
		CultureInfo CurrentCulture
		{
			get;
		}

		event CultureChangedEventHandler CurrentCultureChanged;

		void SimulateCultureChange(CultureInfo ci);
	}
}
