namespace AppName.Core
{
	internal struct PendingMirrorInfo
	{
		public LayoutDirection Direction
		{
			get;
		}

		public bool ChildrenOnly
		{
			get;
		}

		public PendingMirrorInfo(LayoutDirection direction, bool childrenOnly)
		{
			Direction = direction;
			ChildrenOnly = childrenOnly;
		}
	}
}
