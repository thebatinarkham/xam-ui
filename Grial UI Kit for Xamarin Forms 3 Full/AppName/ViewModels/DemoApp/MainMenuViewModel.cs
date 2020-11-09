using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public class MainMenuViewModel : ObservableObject
    {
        private readonly INavigation _navigation;
        private readonly Action<Page> _openPageAsRoot;
        private List<MenuEntry> _mainMenuEntries;
        private MenuEntry _selectedMainMenuEntry;

        public MainMenuViewModel(INavigation navigation, Action<Page> openPageAsRoot)
            : base(listenCultureChanges: true)
        {
            _navigation = navigation;
            _openPageAsRoot = openPageAsRoot;

            LoadData();

            var firstEntry = _mainMenuEntries[0];
            if (firstEntry.IsModal)
            {
                openPageAsRoot(new NavigationPage(CreateDetailDefaultBackgroundPage()));
            }
            else
            {
                MainMenuSelectedItem = firstEntry;
            }
        }

        public List<MenuEntry> MainMenuEntries
        {
            get { return _mainMenuEntries; }
            set { SetProperty(ref _mainMenuEntries, value); }
        }

        public MenuEntry MainMenuSelectedItem
        {
            get { return _selectedMainMenuEntry; }
            set 
            { 
                if (SetProperty(ref _selectedMainMenuEntry, value) && value != null)
                {
                    Page page;

                    if (value.PageType != null)
                    {
                        page = CreatePage(value.PageType);
                    }
                    else
                    {
                        page = value.CreatePage();
                    }

                    NavigationPage navigationPage;

                    if (value.NavigationPageType == null)
                    {
                        navigationPage = new NavigationPage(page);
                    }
                    else
                    {
                        navigationPage = (NavigationPage)Activator.CreateInstance(value.NavigationPageType, page);
                    }

                    if (value.UseTransparentNavBar)
                    {
                        GrialNavigationPage.SetIsBarTransparent(navigationPage, true);
                    }

                    if (_selectedMainMenuEntry.IsModal)
                    {
                        _navigation.PushModalAsync(navigationPage);
                    }
                    else
                    {
                        _openPageAsRoot(navigationPage);
                    }

                    _selectedMainMenuEntry = null;
                    NotifyPropertyChanged(nameof(MainMenuSelectedItem));
                }
            }
        }

        protected override void OnCultureChanged(CultureInfo culture)
        {
            LoadData();
        }

        private static ContentPage CreateDetailDefaultBackgroundPage()
        {
            var content = new Grid();
            var logo = new Label
            {
                Text = GrialIconsFont.LogoGrial,
                FontSize = 100,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Style = (Style)Application.Current.Resources["FontIcon"]
            };
            logo.SetDynamicResource(Label.TextColorProperty, "ComplementColor");
            content.Children.Add(logo);
            return new ContentPage { Content = content };
        }

        private void LoadData()
        {
            MainMenuEntries = new List<MenuEntry>();

            foreach (var cat in SamplesCatalog.SamplesCategoryList)
            {
                MainMenuEntries.Add(new MenuEntry
                {
                    Name = cat.Name,
                    Icon = cat.Icon,
                    CreatePage = () => new SamplesListPage(cat.Id)
                });
            }
        }

        private Page CreatePage(Type pageType)
        {
            return Activator.CreateInstance(pageType) as Page;
        }

        public class MenuEntry
        {
            public string Name { get; set; }
            public string Icon { get; set; }
            public bool UseTransparentNavBar { get; set; }
            public Type PageType { get; set; }
            public Func<Page> CreatePage { get; set; }
            public Type NavigationPageType { get; set; }
            public bool IsModal { get; set; }
        }
    }
}