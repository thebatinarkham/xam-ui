using System;
using System.Collections.Generic;

namespace AppName.Core
{
	internal static class CollectionsExtensions
	{
		public static void ForEach<T>(this IList<T> collection, Action<T> action)
		{
			if (collection != null)
			{
				for (int i = 0; i < collection.Count; i++)
				{
					action(collection[i]);
				}
			}
		}

		public static void ForEach<T>(this IList<T> collection, Action<T, int> action)
		{
			if (collection != null)
			{
				for (int i = 0; i < collection.Count; i++)
				{
					action(collection[i], i);
				}
			}
		}
	}
}
