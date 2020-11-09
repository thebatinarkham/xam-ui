using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace AppName.Core
{
	internal abstract class MirrorViewBase<T> : IMirror where T : VisualElement
	{
		private IMirrorService _coordinator;

		public void SetCoordinator(IMirrorService coordinator)
		{
			_coordinator = coordinator;
		}

		public bool TryHandleMirror(VisualElement target, LayoutDirection direction, bool childrenOnly)
		{
			T val = target as T;
			if (val != null)
			{
				try
				{
					childrenOnly |= (Rtl.GetMirrorBehavior(target) == MirrorBehavior.SkipSelf);
					target.PropertyChanged -= OnTargetPropertyChanged;
					Mirror(val, direction, childrenOnly);
				}
				catch (Exception)
				{
				}
				return true;
			}
			return false;
		}

		protected void MirrorChild(VisualElement child, LayoutDirection direction)
		{
			_coordinator.Mirror(child, direction);
		}

		protected void MirrorChildren(IEnumerable<VisualElement> children, LayoutDirection direction)
		{
			foreach (VisualElement child in children)
			{
				MirrorChild(child, direction);
			}
		}

		protected void WaitForLoad(T target, LayoutDirection direction, bool childrenOnly)
		{
			if (target.Parent == null)
			{
				RtlInternal.SetPendingMirrorInfo(target, new PendingMirrorInfo(direction, childrenOnly));
				target.PropertyChanged -= OnTargetPropertyChanged;
				target.PropertyChanged += OnTargetPropertyChanged;
			}
		}

		private void OnTargetPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Parent")
			{
				T val = (T)sender;
				val.PropertyChanged -= OnTargetPropertyChanged;
				PendingMirrorInfo pendingMirrorInfo = RtlInternal.GetPendingMirrorInfo(val);
				RtlInternal.SetPendingMirrorInfo(val, default(PendingMirrorInfo));
				Mirror(val, pendingMirrorInfo.Direction, pendingMirrorInfo.ChildrenOnly);
			}
		}

		protected abstract void Mirror(T target, LayoutDirection direction, bool childrenOnly);

		protected static void InvertList<V>(IList<V> children)
		{
			if (children.Count > 1)
			{
				V[] array = new V[children.Count];
				children.CopyTo(array, 0);
				children.Clear();
				for (int num = array.Length - 1; num >= 0; num--)
				{
					children.Add(array[num]);
				}
			}
		}
	}
}
