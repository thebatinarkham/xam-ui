using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppName.Core
{
    [ContentProperty("Default")]
	public class OnOrientationThickness : IMarkupExtension<Binding>, IMarkupExtension, IOnOrientationValues<Thickness>
	{
		private Thickness _portrait;

		private Thickness _portraitPhone;

		private Thickness _portraitTablet;

		private Thickness _portraitDesktop;

		private Thickness _landscape;

		private Thickness _landscapePhone;

		private Thickness _landscapeTablet;

		private Thickness _landscapeDesktop;

		private Thickness _default;

		private bool _hasPortrait;

		private bool _hasPortraitPhone;

		private bool _hasPortraitTablet;

		private bool _hasPortraitDesktop;

		private bool _hasLandscape;

		private bool _hasLandscapePhone;

		private bool _hasLandscapeTablet;

		private bool _hasLandscapeDesktop;

		private bool _hasDefault;

		public Thickness Portrait
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

		public Thickness Landscape
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

		public Thickness PortraitPhone
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

		public Thickness PortraitTablet
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

		public Thickness PortraitDesktop
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

		public Thickness LandscapePhone
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

		public Thickness LandscapeTablet
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

		public Thickness LandscapeDesktop
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

		public Thickness Default
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

		public OnOrientationThickness()
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
				Source = OnOrientationHelper.GetBindingSource(this, default(Thickness)),
				Path = "Magnitude"
			};
		}
	}
}
