using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppName.Core
{
    public class OnOrientationInt : IMarkupExtension<Binding>, IMarkupExtension, IOnOrientationValues<int>
	{
		private int _portrait;

		private int _portraitPhone;

		private int _portraitTablet;

		private int _portraitDesktop;

		private int _landscape;

		private int _landscapePhone;

		private int _landscapeTablet;

		private int _landscapeDesktop;

		private int _default;

		private bool _hasPortrait;

		private bool _hasPortraitPhone;

		private bool _hasPortraitTablet;

		private bool _hasPortraitDesktop;

		private bool _hasLandscape;

		private bool _hasLandscapePhone;

		private bool _hasLandscapeTablet;

		private bool _hasLandscapeDesktop;

		private bool _hasDefault;

		public int Portrait
		{
			get
			{
				return _portrait;
			}
			set
			{
				_portrait = value;
				_hasPortrait = true;
			}
		}

		public int Landscape
		{
			get
			{
				return _landscape;
			}
			set
			{
				_landscape = value;
				_hasLandscape = true;
			}
		}

		public int PortraitPhone
		{
			get
			{
				return _portraitPhone;
			}
			set
			{
				_portraitPhone = value;
				_hasPortraitPhone = true;
			}
		}

		public int PortraitTablet
		{
			get
			{
				return _portraitTablet;
			}
			set
			{
				_portraitTablet = value;
				_hasPortraitTablet = true;
			}
		}

		public int PortraitDesktop
		{
			get
			{
				return _portraitDesktop;
			}
			set
			{
				_portraitDesktop = value;
				_hasPortraitDesktop = true;
			}
		}

		public int LandscapePhone
		{
			get
			{
				return _landscapePhone;
			}
			set
			{
				_landscapePhone = value;
				_hasLandscapePhone = true;
			}
		}

		public int LandscapeTablet
		{
			get
			{
				return _landscapeTablet;
			}
			set
			{
				_landscapeTablet = value;
				_hasLandscapeTablet = true;
			}
		}

		public int LandscapeDesktop
		{
			get
			{
				return _landscapeDesktop;
			}
			set
			{
				_landscapeDesktop = value;
				_hasLandscapeDesktop = true;
			}
		}

		public int Default
		{
			get
			{
				return _default;
			}
			set
			{
				_default = value;
				_hasDefault = true;
			}
		}

		public bool HasPortrait => _hasPortrait;

		public bool HasLandscape => _hasLandscape;

		public bool HasPortraitPhone => _hasPortraitPhone;

		public bool HasPortraitTablet => _hasPortraitTablet;

		public bool HasPortraitDesktop => _hasPortraitDesktop;

		public bool HasLandscapePhone => _hasLandscapePhone;

		public bool HasLandscapeTablet => _hasLandscapeTablet;

		public bool HasLandscapeDesktop => _hasLandscapeDesktop;

		public bool HasDefault => _hasDefault;

		public OnOrientationInt()
		{
		}

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			return CreateBinding();
		}

		Binding IMarkupExtension<Binding>.ProvideValue(IServiceProvider serviceProvider)
		{
			return CreateBinding();
		}

		private Binding CreateBinding()
		{
			return new Binding
			{
				Source = OnOrientationHelper.GetBindingSource(this),
				Path = "Magnitude"
			};
		}
	}
}
