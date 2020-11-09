using System;

namespace AppName.Core
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public class GrialVersion : Attribute
	{
		public string Version
		{
			get;
		}

		public GrialVersion(string version)
		{
			Version = version;
		}
	}
}
