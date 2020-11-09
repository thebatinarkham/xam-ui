using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppName.Core
{
    [ContentProperty("Default")]
	public class OnOrientationBool : IMarkupExtension<Binding>, IMarkupExtension, IOnOrientationValues<bool>
	{
		private bool _portrait;

		private bool _portraitPhone;

		private bool _portraitTablet;

		private bool _portraitDesktop;

		private bool _landscape;

		private bool _landscapePhone;

		private bool _landscapeTablet;

		private bool _landscapeDesktop;

		private bool _default;

		private bool _hasPortrait;

		private bool _hasPortraitPhone;

		private bool _hasPortraitTablet;

		private bool _hasPortraitDesktop;

		private bool _hasLandscape;

		private bool _hasLandscapePhone;

		private bool _hasLandscapeTablet;

		private bool _hasLandscapeDesktop;

		private bool _hasDefault;

		public bool Portrait
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

		public bool Landscape
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

		public bool PortraitPhone
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

		public bool PortraitTablet
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

		public bool PortraitDesktop
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

		public bool LandscapePhone
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

		public bool LandscapeTablet
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

		public bool LandscapeDesktop
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

		public bool Default
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

		public OnOrientationBool()
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
