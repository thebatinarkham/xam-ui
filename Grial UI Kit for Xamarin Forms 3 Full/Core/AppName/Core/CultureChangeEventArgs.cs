using System;
using System.Globalization;

namespace AppName.Core
{
	public class CultureChangeEventArgs : EventArgs
	{
		public CultureInfo NewCulture
		{
			get;
		}

		public CultureInfo OldCulture
		{
			get;
		}

		public CultureChangeEventArgs(CultureInfo oldCulture, CultureInfo newCulture)
		{
			OldCulture = oldCulture;
			NewCulture = newCulture;
		}
	}
}
