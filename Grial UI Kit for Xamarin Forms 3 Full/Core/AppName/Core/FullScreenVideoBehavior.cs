using System;
using Xamarin.Forms;

namespace AppName.Core
{
	public class FullScreenVideoBehavior : Behavior<Button>
	{
		public static readonly BindableProperty SourceProperty = BindableProperty.Create("Source", typeof(string), typeof(FullScreenVideoBehavior));

		public static readonly BindableProperty SkinTemplateProperty = BindableProperty.Create("SkinTemplate", typeof(ControlTemplate), typeof(FullScreenVideoBehavior));

		public static readonly BindableProperty LoadingTemplateProperty = BindableProperty.Create("LoadingTemplate", typeof(ControlTemplate), typeof(FullScreenVideoBehavior));

		public static readonly BindableProperty ErrorTemplateProperty = BindableProperty.Create("ErrorTemplate", typeof(ControlTemplate), typeof(FullScreenVideoBehavior));

		public string Source
		{
			get
			{
				return (string)GetValue(SourceProperty);
			}
			set
			{
				SetValue(SourceProperty, value);
			}
		}

		public ControlTemplate SkinTemplate
		{
			get
			{
				return (ControlTemplate)GetValue(SkinTemplateProperty);
			}
			set
			{
				SetValue(SkinTemplateProperty, value);
			}
		}

		public ControlTemplate LoadingTemplate
		{
			get
			{
				return (ControlTemplate)GetValue(LoadingTemplateProperty);
			}
			set
			{
				SetValue(LoadingTemplateProperty, value);
			}
		}

		public ControlTemplate ErrorTemplate
		{
			get
			{
				return (ControlTemplate)GetValue(ErrorTemplateProperty);
			}
			set
			{
				SetValue(ErrorTemplateProperty, value);
			}
		}

		public Button Target
		{
			get;
			private set;
		}

		protected override void OnAttachedTo(Button bindable)
		{
			base.OnAttachedTo(bindable);
			Target = bindable;
			if (Target != null)
			{
				Target.Clicked += OnTargetClicked;
			}
		}

		protected override void OnDetachingFrom(Button bindable)
		{
			if (Target != null)
			{
				Target.Clicked -= OnTargetClicked;
			}
			Target = null;
			base.OnDetachingFrom(bindable);
		}

		private void OnTargetClicked(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(Source))
			{
				return;
			}
			INavigation navigation = FindNavigation();
			if (navigation != null)
			{
				VideoPlayer videoPlayer = new VideoPlayer
				{
					Source = Source,
					AutoPlay = true,
					Repeat = false
				};
				if (ErrorTemplate != null)
				{
					videoPlayer.ErrorTemplate = ErrorTemplate;
				}
				if (SkinTemplate != null)
				{
					videoPlayer.SkinTemplate = SkinTemplate;
				}
				if (LoadingTemplate != null)
				{
					videoPlayer.LoadingTemplate = LoadingTemplate;
				}
				videoPlayer.MakeParentlessVideoPlayerFullScreen(navigation);
			}
		}

		private INavigation FindNavigation()
		{
			Element element = Target;
			while (element != null && !(element is Page))
			{
				element = element.Parent;
			}
			object obj = (element as Page)?.Navigation;
			if (obj == null)
			{
				Page mainPage = Application.Current.MainPage;
				if (mainPage == null)
				{
					return null;
				}
				obj = mainPage.Navigation;
			}
			return (INavigation)obj;
		}
	}
}
