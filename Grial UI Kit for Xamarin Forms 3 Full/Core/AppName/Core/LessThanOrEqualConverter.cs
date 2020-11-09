using System;
using System.Globalization;
using Xamarin.Forms;

namespace AppName.Core
{
	public class LessThanOrEqualConverter<T> : IValueConverter
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

		public LessThanOrEqualConverter()
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
				result = ((double)(int)(byte)value <= to);
			}
			if (value is short)
			{
				result = ((double)(short)value <= to);
			}
			if (value is ushort)
			{
				result = ((double)(int)(ushort)value <= to);
			}
			if (value is int)
			{
				result = ((double)(int)value <= to);
			}
			if (value is uint)
			{
				result = ((double)(uint)value <= to);
			}
			if (value is long)
			{
				result = ((double)(long)value <= to);
			}
			if (value is ulong)
			{
				result = ((double)(ulong)value <= to);
			}
			if (value is double)
			{
				result = ((double)value <= to);
			}
			if (value is float)
			{
				result = ((double)(float)value <= to);
			}
			return result;
		}
	}
	public class LessThanOrEqualConverter : LessThanOrEqualConverter<bool>
	{
		public LessThanOrEqualConverter()
		{
			base.TrueValue = true;
			base.FalseValue = false;
		}
	}
}
