using System;
using System.Collections;

namespace AppName.Core
{
	internal class ReadonlyReverseListAdapter : IList, ICollection, IEnumerable
	{
		private readonly IList _adaptee;

		public object this[int index]
		{
			get
			{
				return _adaptee[MapIndex(index)];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public int Count => _adaptee.Count;

		public bool IsFixedSize => _adaptee.IsFixedSize;

		public bool IsReadOnly => true;

		public bool IsSynchronized => _adaptee.IsSynchronized;

		public object SyncRoot => _adaptee.SyncRoot;

		public ReadonlyReverseListAdapter(IList adaptee)
		{
			_adaptee = adaptee;
		}

		public int Add(object value)
		{
			throw new NotSupportedException();
		}

		public void Clear()
		{
			throw new NotSupportedException();
		}

		public bool Contains(object value)
		{
			return _adaptee.Contains(value);
		}

		public void CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		public IEnumerator GetEnumerator()
		{
			for (int i = _adaptee.Count - 1; i >= 0; i--)
			{
				yield return _adaptee[i];
			}
		}

		public int IndexOf(object value)
		{
			return _adaptee.IndexOf(value);
		}

		public void Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		public void Remove(object value)
		{
			throw new NotSupportedException();
		}

		public void RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		private int MapIndex(int index)
		{
			return _adaptee.Count - index - 1;
		}
	}
}
