namespace AppName.Core
{
	internal static class AssemblyGlobal
	{
		public const string Company = "Binaria";

		public const string ProductLine = "AppName";

		public const string Year = "2018";

		public const string AssemblyVersion = "3.0.0.*";

        public const string Copyright = Company + " - " + Year;

#if DEBUG
        public const string Configuration = "Debug";
#elif RELEASE
        public const string Configuration = "Release";
#else
        public const string Configuration = "Unkown";
#endif
	}
}
