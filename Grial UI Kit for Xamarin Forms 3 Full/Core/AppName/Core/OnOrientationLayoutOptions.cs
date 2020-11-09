using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppName.Core
{
    [ContentProperty("Default")]
	public class OnOrientationLayoutOptions : IMarkupExtension<Binding>, IMarkupExtension, IOnOrientationValues<LayoutOptions>
	{
		private LayoutOptions _portrait;

		private LayoutOptions _portraitPhone;

		private LayoutOptions _portraitTablet;

		private LayoutOptions _portraitDesktop;

		private LayoutOptions _landscape;

		private LayoutOptions _landscapePhone;

		private LayoutOptions _landscapeTablet;

		private LayoutOptions _landscapeDesktop;

		private LayoutOptions _default;

		private bool _hasPortrait;

		private bool _hasPortraitPhone;

		private bool _hasPortraitTablet;

		private bool _hasPortraitDesktop;

		private bool _hasLandscape;

		private bool _hasLandscapePhone;

		private bool _hasLandscapeTablet;

		private bool _hasLandscapeDesktop;

		private bool _hasDefault;

		public LayoutOptions Portrait
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

		public LayoutOptions Landscape
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

		public LayoutOptions PortraitPhone
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

		public LayoutOptions PortraitTablet
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

		public LayoutOptions PortraitDesktop
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

		public LayoutOptions LandscapePhone
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

		public LayoutOptions LandscapeTablet
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

		public LayoutOptions LandscapeDesktop
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

		public LayoutOptions Default
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

		public OnOrientationLayoutOptions()
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
				Source = OnOrientationHelper.GetBindingSource(this, LayoutOptions.Fill),
				Path = "Magnitude"
			};
		}
	}
}
