using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppName.Core
{
    [ContentProperty("Default")]
	public class OnOrientationColor : IMarkupExtension<Binding>, IMarkupExtension, IOnOrientationValues<Color>
	{
		private Color _portrait;

		private Color _portraitPhone;

		private Color _portraitTablet;

		private Color _portraitDesktop;

		private Color _landscape;

		private Color _landscapePhone;

		private Color _landscapeTablet;

		private Color _landscapeDesktop;

		private Color _default;

		private bool _hasPortrait;

		private bool _hasPortraitPhone;

		private bool _hasPortraitTablet;

		private bool _hasPortraitDesktop;

		private bool _hasLandscape;

		private bool _hasLandscapePhone;

		private bool _hasLandscapeTablet;

		private bool _hasLandscapeDesktop;

		private bool _hasDefault;

		public Color Portrait
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

		public Color Landscape
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

		public Color PortraitPhone
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

		public Color PortraitTablet
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

		public Color PortraitDesktop
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

		public Color LandscapePhone
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

		public Color LandscapeTablet
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

		public Color LandscapeDesktop
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

		public Color Default
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

		public OnOrientationColor()
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
				Source = OnOrientationHelper.GetBindingSource(this, Color.Default),
				Path = "Magnitude"
			};
		}
	}
}
