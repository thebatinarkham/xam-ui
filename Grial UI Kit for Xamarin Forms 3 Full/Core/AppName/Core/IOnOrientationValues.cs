namespace AppName.Core
{
	internal interface IOnOrientationValues<T>
	{
		T Portrait
		{
			get;
		}

		T Landscape
		{
			get;
		}

		T PortraitPhone
		{
			get;
		}

		T PortraitTablet
		{
			get;
		}

		T PortraitDesktop
		{
			get;
		}

		T LandscapePhone
		{
			get;
		}

		T LandscapeTablet
		{
			get;
		}

		T LandscapeDesktop
		{
			get;
		}

		T Default
		{
			get;
		}

		bool HasPortrait
		{
			get;
		}

		bool HasLandscape
		{
			get;
		}

		bool HasPortraitPhone
		{
			get;
		}

		bool HasPortraitTablet
		{
			get;
		}

		bool HasPortraitDesktop
		{
			get;
		}

		bool HasLandscapePhone
		{
			get;
		}

		bool HasLandscapeTablet
		{
			get;
		}

		bool HasLandscapeDesktop
		{
			get;
		}

		bool HasDefault
		{
			get;
		}
	}
}
