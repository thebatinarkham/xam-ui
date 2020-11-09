using System;
using System.Globalization;
using Xamarin.Forms;

namespace AppName.Core
{
	public class GreaterThanOrEqualConverter<T> : IValueConverter
	{
		public double Threshold
		{
			get;
			set;
		}

		public T TrueValue
		{
			get;
			set;
		}

		public T FalseValue
		{
			get;
			set;
		}

		public GreaterThanOrEqualConverter()
		{
			Threshold = 0.0;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Compare(value, Threshold) ? TrueValue : FalseValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		public static bool Compare(object value, double to)
		{
			bool result = false;
			if (value is byte)
			{
				result = ((double)(int)(byte)value >= to);
			}
			else if (value is short)
			{
				result = ((double)(short)value >= to);
			}
			else if (value is ushort)
			{
				result = ((double)(int)(ushort)value >= to);
			}
			else if (value is int)
			{
				result = ((double)(int)value >= to);
			}
			else if (value is uint)
			{
				result = ((double)(uint)value >= to);
			}
			else if (value is long)
			{
				result = ((double)(long)value >= to);
			}
			else if (value is ulong)
			{
				result = ((double)(ulong)value >= to);
			}
			else if (value is double)
			{
				result = ((double)value >= to);
			}
			else if (value is float)
			{
				result = ((double)(float)value >= to);
			}
			return result;
		}
	}
	public class GreaterThanOrEqualConverter : GreaterThanOrEqualConverter<bool>
	{
		public GreaterThanOrEqualConverter()
		{
			base.TrueValue = true;
			base.FalseValue = false;
		}
	}
}
