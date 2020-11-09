using System;
using Xamarin.Forms;

namespace AppName.Core
{
	internal class OrientationChangePatch
	{
		private readonly bool _isEnabled;

		private double _dimension;

		private bool? _isPortrait;

		public OrientationChangePatch(string targetPlatform)
		{
			_dimension = -1.0;
			_isEnabled = (string.IsNullOrEmpty(targetPlatform) || Device.RuntimePlatform == targetPlatform);
			ReadOrientation();
		}

		public void OnDimensionChanged(double newDimension, Action changeVisual, Action restoreVisual)
		{
			if (_dimension != newDimension)
			{
				_dimension = newDimension;
				bool? isPortrait = _isPortrait;
				_isPortrait = ReadOrientation();
				if (isPortrait.HasValue && _isPortrait.HasValue && isPortrait != _isPortrait)
				{
					changeVisual();
					Device.StartTimer(TimeSpan.FromMilliseconds(10.0), delegate
					{
						restoreVisual();
						return false;
					});
				}
			}
		}

		private bool? ReadOrientation()
		{
			if (!_isEnabled)
			{
				return null;
			}
			Page page = Application.Current?.MainPage;
			if (page != null)
			{
				double height = page.Height;
				double width = page.Width;
				if (!double.IsNaN(height) && height > 0.0 && !double.IsNaN(width) && width > 0.0)
				{
					return height > width;
				}
			}
			return null;
		}
	}
}
