using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace AppName.Core
{
    [ContentProperty("Children")]
	public class CardView : ContentView
	{
		private class DelegateCollection : Collection<View>
		{
			private readonly Grid.IGridList<View> _inner;

			public DelegateCollection(Grid.IGridList<View> inner)
			{
				_inner = inner;
			}

			protected override void InsertItem(int index, View item)
			{
				base.InsertItem(index, item);
				_inner.Insert(index, item);
			}

			protected override void RemoveItem(int index)
			{
				base.RemoveItem(index);
				_inner.RemoveAt(index);
			}

			protected override void SetItem(int index, View item)
			{
				base.SetItem(index, item);
				_inner[index] = item;
			}

			protected override void ClearItems()
			{
				base.ClearItems();
				_inner.Clear();
			}
		}

		private readonly DelegateCollection _children;

		public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create("RowSpacing", typeof(double), typeof(CardView), 0.0, BindingMode.OneWay, null, delegate(BindableObject b, object o, object n)
		{
			((CardView)b).Content.RowSpacing = (double)n;
		});

		public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create("ColumnSpacing", typeof(double), typeof(Grid), 0.0, BindingMode.OneWay, null, delegate(BindableObject b, object o, object n)
		{
			((CardView)b).Content.ColumnSpacing = (double)n;
		});

		public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create("CornerRadius", typeof(double), typeof(CardView), 5.0, BindingMode.OneWay, null, delegate(BindableObject b, object o, object n)
		{
			((CardView)b).UpdateCorners();
		});

		public static readonly BindableProperty ShadowSizeProperty = BindableProperty.Create("ShadowSize", typeof(float), typeof(CardView), 5f, BindingMode.OneWay, null, delegate(BindableObject b, object o, object n)
		{
			((CardView)b).UpdateShadowSize();
		});

		public static readonly BindableProperty ShadowOpacityProperty = BindableProperty.Create("ShadowOpacity", typeof(float), typeof(CardView), 1f, BindingMode.OneWay, null, delegate(BindableObject b, object o, object n)
		{
			((CardView)b).UpdateShadowOpacity();
		});

		public static readonly BindableProperty HasShadowProperty = BindableProperty.Create("HasShadow", typeof(bool), typeof(CardView), true, BindingMode.OneWay, null, delegate(BindableObject b, object o, object n)
		{
			((CardView)b).UpdateShadow();
		});

		public new static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create("BackgroundColor", typeof(Color), typeof(CardView), Color.White, BindingMode.OneWay, null, delegate(BindableObject b, object o, object n)
		{
			((CardView)b).UpdateBackground();
		});

		public new static readonly BindableProperty PaddingProperty = BindableProperty.Create("Padding", typeof(Thickness), typeof(CardView), default(Thickness), BindingMode.OneWay, null, delegate(BindableObject b, object o, object n)
		{
			((CardView)b).UpdatePadding();
		});

		public double CornerRadius
		{
			get
			{
				return (double)GetValue(CornerRadiusProperty);
			}
			set
			{
				SetValue(CornerRadiusProperty, value);
			}
		}

		public float ShadowSize
		{
			get
			{
				return (float)GetValue(ShadowSizeProperty);
			}
			set
			{
				SetValue(ShadowSizeProperty, value);
			}
		}

		public float ShadowOpacity
		{
			get
			{
				return (float)GetValue(ShadowOpacityProperty);
			}
			set
			{
				SetValue(ShadowOpacityProperty, value);
			}
		}

		public bool HasShadow
		{
			get
			{
				return (bool)GetValue(HasShadowProperty);
			}
			set
			{
				SetValue(HasShadowProperty, value);
			}
		}

		public new Color BackgroundColor
		{
			get
			{
				return (Color)GetValue(BackgroundColorProperty);
			}
			set
			{
				SetValue(BackgroundColorProperty, value);
			}
		}

		public new Thickness Padding
		{
			get
			{
				return (Thickness)GetValue(PaddingProperty);
			}
			set
			{
				SetValue(PaddingProperty, value);
			}
		}

		public RowDefinitionCollection RowDefinitions
		{
			get
			{
				return Content.RowDefinitions;
			}
			set
			{
				Content.RowDefinitions = value;
			}
		}

		public ColumnDefinitionCollection ColumnDefinitions
		{
			get
			{
				return Content.ColumnDefinitions;
			}
			set
			{
				Content.ColumnDefinitions = value;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Grid Content
		{
			get;
			private set;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ControlTemplate ControlTemplate
		{
			get;
			private set;
		}

		public new IList<View> Children => _children;

		public double ColumnSpacing
		{
			get
			{
				return (double)GetValue(ColumnSpacingProperty);
			}
			set
			{
				SetValue(ColumnSpacingProperty, value);
			}
		}

		public double RowSpacing
		{
			get
			{
				return (double)GetValue(RowSpacingProperty);
			}
			set
			{
				SetValue(RowSpacingProperty, value);
			}
		}

		public CardView()
		{
			Grid grid2 = Content = new Grid
			{
				RowSpacing = 0.0,
				ColumnSpacing = 0.0
			};
			AppName.Core.Effects.SetShadowIOSColor(this, new Color(0.0, 0.0, 0.0, 0.5));
			UpdateCorners();
			UpdateShadow();
			UpdateBackground();
			UpdateShadowSize();
			UpdateShadowOpacity();
			_children = new DelegateCollection(grid2.Children);
		}

		protected override void OnParentSet()
		{
			base.OnParentSet();
			SetValue(ContentView.ContentProperty, Content);
		}

		private void UpdateCorners()
		{
			if (Content == null)
			{
				return;
			}
			string runtimePlatform = Device.RuntimePlatform;
			if (runtimePlatform == null)
			{
				return;
			}
			if (!(runtimePlatform == "iOS"))
			{
				if (runtimePlatform == "Android")
				{
					AppName.Core.Effects.SetCornerRadius(this, CornerRadius);
				}
			}
			else
			{
				AppName.Core.Effects.SetCornerRadius(Content, CornerRadius);
			}
		}

		private void UpdateShadowSize()
		{
			AppName.Core.Effects.SetShadowSize(this, ShadowSize);
		}

		private void UpdateShadowOpacity()
		{
			AppName.Core.Effects.SetShadowOpacity(this, ShadowOpacity);
		}

		private void UpdateShadow()
		{
			AppName.Core.Effects.SetShadow(this, HasShadow);
		}

		private void UpdateBackground()
		{
			if (Content == null)
			{
				return;
			}
			string runtimePlatform = Device.RuntimePlatform;
			if (runtimePlatform == null)
			{
				return;
			}
			if (!(runtimePlatform == "iOS"))
			{
				if (runtimePlatform == "Android")
				{
					SetValue(VisualElement.BackgroundColorProperty, BackgroundColor);
				}
			}
			else
			{
				Content.BackgroundColor = BackgroundColor;
			}
		}

		private void UpdatePadding()
		{
			string runtimePlatform = Device.RuntimePlatform;
			if (runtimePlatform == null)
			{
				return;
			}
			if (!(runtimePlatform == "iOS"))
			{
				if (runtimePlatform == "Android")
				{
					SetValue(Xamarin.Forms.Layout.PaddingProperty, Padding);
				}
			}
			else
			{
				Content.Padding = Padding;
			}
		}
	}
}
