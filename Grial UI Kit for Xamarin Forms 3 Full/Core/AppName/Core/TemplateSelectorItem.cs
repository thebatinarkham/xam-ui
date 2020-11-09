using System;
using Xamarin.Forms;

namespace AppName.Core
{
	[ContentProperty("DataTemplate")]
	public class TemplateSelectorItem
	{
		public DataTemplate DataTemplate
		{
			get;
			set;
		}

		public string HasMember
		{
			get;
			set;
		}

		[TypeConverter(typeof(TypeTypeConverter))]
		public Type TargetType
		{
			get;
			set;
		}
	}
}
