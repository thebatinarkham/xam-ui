using System.Collections.Generic;
using Xamarin.Forms;

namespace AppName.Core
{
    internal class MirrorService : IMirrorService
	{
		private static MirrorService _Instance;

		private readonly List<IMirror> _mirrors;

		public static MirrorService Instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = new MirrorService();
				}
				return _Instance;
			}
		}

		public MirrorService()
		{
			_mirrors = new List<IMirror>();
			RegisterMirror(new MirrorLabel());
			RegisterMirror(new MirrorGrid());
			RegisterMirror(new MirrorStackLayout());
			RegisterMirror(new MirrorScrollView());
			RegisterMirror(new MirrorContentPage());
			RegisterMirror(new MirrorContentView());
			RegisterMirror(new MirrorLayout());
		}

		public void RegisterMirror(IMirror mirror)
		{
			mirror.SetCoordinator(this);
			_mirrors.Add(mirror);
		}

		public void Mirror(VisualElement target, LayoutDirection direction)
		{
			if (target == null || RtlInternal.GetCurrentLayoutDirection(target) == direction || Rtl.GetMirrorBehavior(target) == MirrorBehavior.Skip)
			{
				return;
			}
			bool childrenOnly = false;
			ILayoutDirectionAware layoutDirectionAware = target as ILayoutDirectionAware;
			if (layoutDirectionAware != null)
			{
				layoutDirectionAware.SetLayoutDirection(direction);
				childrenOnly = true;
			}
			for (int i = 0; i < _mirrors.Count; i++)
			{
				if (_mirrors[i].TryHandleMirror(target, direction, childrenOnly))
				{
					return;
				}
			}
			RtlInternal.SetCurrentLayoutDirection(target, direction);
		}
	}
}
