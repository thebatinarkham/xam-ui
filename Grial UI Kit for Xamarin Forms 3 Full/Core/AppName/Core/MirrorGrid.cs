using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AppName.Core
{
	internal class MirrorGrid : MirrorViewBase<Grid>
	{
		private class GridColumnViews
		{
			private readonly int _columnIndex;

			private List<View> _views;

			public GridColumnViews(int colIndex)
			{
				_columnIndex = colIndex;
			}

			public void Add(View child)
			{
				if (_views == null)
				{
					_views = new List<View>();
				}
				_views.Add(child);
			}

			public void AdjustCenterColumn()
			{
				if (_views == null)
				{
					return;
				}
				for (int i = 0; i < _views.Count; i++)
				{
					View bindable = _views[i];
					int columnSpan = Grid.GetColumnSpan(bindable);
					if (columnSpan > 1)
					{
						Grid.SetColumn(bindable, Math.Max(0, _columnIndex - columnSpan + 1));
					}
				}
			}

			public void SwapColumnViews(GridColumnViews other)
			{
				if (_views != null)
				{
					for (int i = 0; i < _views.Count; i++)
					{
						View view = _views[i];
						int columnSpan = Grid.GetColumnSpan(view);
						view.SetValue(Grid.ColumnProperty, Math.Max(0, other._columnIndex - columnSpan + 1));
					}
				}
				if (other._views != null)
				{
					for (int j = 0; j < other._views.Count; j++)
					{
						View view2 = other._views[j];
						int columnSpan2 = Grid.GetColumnSpan(view2);
						view2.SetValue(Grid.ColumnProperty, Math.Max(0, _columnIndex - columnSpan2 + 1));
					}
				}
			}
		}

		protected override void Mirror(Grid target, LayoutDirection direction, bool childrenOnly)
		{
			if (target.Children == null)
			{
				WaitForLoad(target, direction, childrenOnly);
				return;
			}
			RtlInternal.SetCurrentLayoutDirection(target, direction);
			MirrorChildren(target.Children, direction);
			if (childrenOnly)
			{
				return;
			}
			ColumnDefinitionCollection columnDefinitions = target.ColumnDefinitions;
			if (columnDefinitions != null && columnDefinitions.Count > 1)
			{
				int count = columnDefinitions.Count;
				List<GridColumnViews> list = new List<GridColumnViews>();
				for (int i = 0; i < count; i++)
				{
					list.Add(new GridColumnViews(i));
				}
				foreach (View child in target.Children)
				{
					int num = Grid.GetColumn(child);
					if (num >= list.Count)
					{
						num = list.Count - 1;
					}
					list[num].Add(child);
				}
				int num2 = count - 1;
				int num3 = 0;
				while (num3 < num2 && num2 > 0)
				{
					ColumnDefinition columnDefinition = columnDefinitions[num3];
					ColumnDefinition columnDefinition2 = columnDefinitions[num2];
					GridLength width = columnDefinition.Width;
					GridLength gridLength = columnDefinition.Width = columnDefinition2.Width;
					columnDefinition2.Width = width;
					GridColumnViews gridColumnViews = list[num3];
					GridColumnViews other = list[num2];
					gridColumnViews.SwapColumnViews(other);
					num3++;
					num2--;
				}
				if (count > 2 && count % 2 != 0)
				{
					list[count / 2].AdjustCenterColumn();
				}
			}
		}
	}
}
