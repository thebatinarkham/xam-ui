using System;
using System.Collections.Generic;
using System.Reflection;

namespace AppName.Core
{
	internal static class ReflectionHelper
	{
		private static HashSet<Type> NumericTypes = new HashSet<Type>
		{
			typeof(byte),
			typeof(sbyte),
			typeof(short),
			typeof(ushort),
			typeof(int),
			typeof(uint),
			typeof(long),
			typeof(ulong),
			typeof(double),
			typeof(decimal),
			typeof(float)
		};

		public static bool TryGetPropertyOrField(object obj, string name, out PropertyInfo property, out FieldInfo field)
		{
			field = null;
			property = RuntimeReflectionExtensions.GetRuntimeProperty(obj.GetType(), name);
			if (property == null)
			{
				field = RuntimeReflectionExtensions.GetRuntimeField(obj.GetType(), name);
				if (field == null)
				{
					IReflectableType reflectableType = obj as IReflectableType;
					if (reflectableType == null)
					{
						return false;
					}
					property = reflectableType.GetTypeInfo().GetDeclaredProperty(name);
					if (property == null)
					{
						field = reflectableType.GetTypeInfo().GetDeclaredField(name);
						return field != null;
					}
				}
			}
			return true;
		}

		public static bool TryGetPropertyOrFieldValue(object obj, string name, out object value)
		{
			value = null;
			if (TryGetPropertyOrField(obj, name, out PropertyInfo property, out FieldInfo field))
			{
				value = ((property != null) ? property.GetValue(obj) : field.GetValue(obj));
				return true;
			}
			return false;
		}

		public static object EvaluateBindingPath(this object obj, string bindingPath)
		{
			if (obj == null || string.IsNullOrWhiteSpace(bindingPath))
			{
				return null;
			}
			if (bindingPath.Trim() == ".")
			{
				return obj;
			}
			string[] array = bindingPath.Split(new char[1]
			{
				'.'
			});
			object obj2 = null;
			if (TryGetPropertyOrField(obj, array[0], out PropertyInfo property, out FieldInfo field))
			{
				obj2 = ((property != null) ? property.GetValue(obj) : field.GetValue(obj));
				if (obj2 != null && array.Length > 1)
				{
					obj2 = obj2.EvaluateBindingPath(bindingPath.Substring(array[0].Length + 1));
				}
			}
			return obj2;
		}

		public static Type EvaluateBindingPathTargetType(this object obj, string bindingPath)
		{
			if (obj == null || string.IsNullOrWhiteSpace(bindingPath))
			{
				return null;
			}
			if (bindingPath.Trim() == ".")
			{
				return obj.GetType();
			}
			string[] array = bindingPath.Split(new char[1]
			{
				'.'
			});
			if (TryGetPropertyOrField(obj, array[0], out PropertyInfo property, out FieldInfo field))
			{
				if (array.Length <= 1)
				{
					if (!(property != null))
					{
						return field.FieldType;
					}
					return property.PropertyType;
				}
				object obj2 = (property != null) ? property.GetValue(obj) : field.GetValue(obj);
				if (obj2 != null)
				{
					return obj2.EvaluateBindingPathTargetType(bindingPath.Substring(array[0].Length + 1));
				}
			}
			return null;
		}

		public static bool IsNumericType(this Type type)
		{
			if (!NumericTypes.Contains(type))
			{
				return NumericTypes.Contains(Nullable.GetUnderlyingType(type));
			}
			return true;
		}

		public static bool IsDateType(this Type type)
		{
			if (!type.Equals(typeof(DateTime)))
			{
				return Nullable.GetUnderlyingType(type)?.Equals(typeof(DateTime)) ?? false;
			}
			return true;
		}
	}
}
