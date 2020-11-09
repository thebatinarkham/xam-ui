using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace AppName.Core
{
	internal class GradientChangeListener
	{
		private readonly Collection<GradientColor> _colors;

		public Gradient Target
		{
			get;
		}

		private event EventHandler _updated;

		public event EventHandler Updated
		{
			add
			{
				if (this._updated == null)
				{
					StartListening();
				}
				_updated += value;
			}
			remove
			{
				_updated -= value;
				if (this._updated == null)
				{
					StopListening();
				}
			}
		}

		public GradientChangeListener(Gradient target)
		{
			Target = target;
			_colors = new Collection<GradientColor>();
		}

		private void StartListening()
		{
			Target.PropertyChanged += OnGradientPropertyChanged;
			Target.Colors.CollectionChanged += OnColorsCollectionChanged;
			SubscribeToChangesInColors();
		}

		private void StopListening()
		{
			Target.PropertyChanged -= OnGradientPropertyChanged;
			Target.Colors.CollectionChanged -= OnColorsCollectionChanged;
			UnSubscribeToChangesInColors();
		}

		private void SubscribeToChangesInColors()
		{
			for (int i = 0; i < Target.Colors.Count; i++)
			{
				GradientColor gradientColor = Target.Colors[i];
				_colors.Add(gradientColor);
				gradientColor.PropertyChanged += OnColorPropertyChanged;
			}
		}

		private void UnSubscribeToChangesInColors()
		{
			for (int i = 0; i < _colors.Count; i++)
			{
				_colors[i].PropertyChanged -= OnColorPropertyChanged;
			}
			_colors.Clear();
		}

		private void OnColorPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this._updated?.Invoke(this, e);
		}

		private void OnColorsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			UnSubscribeToChangesInColors();
			SubscribeToChangesInColors();
			this._updated?.Invoke(this, e);
		}

		private void OnGradientPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this._updated?.Invoke(this, e);
		}
	}
}
