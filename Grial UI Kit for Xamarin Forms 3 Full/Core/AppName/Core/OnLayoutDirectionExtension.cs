using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppName.Core
{
    public class OnLayoutDirectionExtension<T> : IMarkupExtension<Binding>, IMarkupExtension
	{
		private class BindingSource<V> : INotifyPropertyChanged, NotificationObjectsTracker.INotifier
		{
			private OnLayoutDirectionExtension<V> _source;

			public V Value
			{
				get
				{
					if (LayoutDirectionExtensionStaticNotifier.LayoutDirectionService.LayoutDirection != 0)
					{
						return _source.Rtl;
					}
					return _source.Ltr;
				}
			}

			public event PropertyChangedEventHandler PropertyChanged;

			public BindingSource(OnLayoutDirectionExtension<V> source)
			{
				_source = source;
			}

			public void Notify()
			{
				this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
			}
		}

		public T Rtl
		{
			get;
			set;
		}

		public T Ltr
		{
			get;
			set;
		}

		public IValueConverter Converter
		{
			get;
			set;
		}

		public object ConverterParameter
		{
			get;
			set;
		}

		public string StringFormat
		{
			get;
			set;
		}

		public OnLayoutDirectionExtension()
		{
			LayoutDirectionExtensionStaticNotifier.Initialize();
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
			BindingSource<T> bindingSource = new BindingSource<T>(this);
			Binding result = new Binding
			{
				Source = bindingSource,
				Path = "Value",
				Converter = Converter,
				ConverterParameter = ConverterParameter,
				StringFormat = StringFormat
			};
			LayoutDirectionExtensionStaticNotifier.Add(bindingSource);
			return result;
		}
	}
}
