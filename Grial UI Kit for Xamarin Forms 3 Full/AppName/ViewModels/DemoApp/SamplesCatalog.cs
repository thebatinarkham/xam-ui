using System;
using System.Collections.Generic;
using System.Linq;
using AppName.Resx;
using AppName.Core;

namespace AppName
{
    public enum FlowType
    {
        Chat,
        Ecommerce,
        Movies,
        Tasks
    }

    public static class SamplesCatalog
    {
        private static CultureChangeNotifier _notifier;

        private static List<SampleCategory> _samplesCategoryList;
        private static List<FlowSample> _flows;

        public static List<SampleCategory> SamplesCategoryList
        {
            get
            {
                if (_samplesCategoryList == null)
                {
                    _samplesCategoryList = CreateSamples();
                }

                return _samplesCategoryList;
            }
        }


        public static List<FlowSample> Flows
        {
            get
            {
                if (_flows == null)
                {
                    _flows = CreateFlows();
                }

                return _flows;
            }
        }


        public static void Initialize()
        {
            _notifier = new CultureChangeNotifier();
            _notifier.CultureChanged += (sender, args) => InitializeData();
        }

        public static void InitializeData()
        {
            _flows = null;
            _samplesCategoryList = null;

        }


        public static List<FlowSample> CreateFlows()
        {
            return new List<FlowSample>
            {
                new FlowSample(
                    AppResources.FlowTitleMovies,
                    DemoAppResources.FlowDescriptionMovies,
                    GrialIconsFont.Film,
                    typeof(MoviesNavigationPage),
                    FlowType.Movies)
                {
                    IndividualPages = new List<Sample>
                    {
                        new Sample(
                            AppResources.PageTitleMoviesMain,
                            DemoAppResources.MoviesMainPageDescription,
                            GrialIconsFont.Film,
                            typeof(MoviesMainPage)),
                        new Sample(
                            AppResources.PageTitleMovieDetail,
                            DemoAppResources.MovieDetailPageDescription,
                            GrialIconsFont.Film,
                            typeof(MovieDetailPage)),
                        new Sample(
                            AppResources.PageTitleFeaturedMovies,
                            DemoAppResources.FeaturedMoviesPageDescription,
                            GrialIconsFont.Film,
                            typeof(FeaturedMoviesPage))
                    }
                },
                new FlowSample(
                    AppResources.FlowTitleTasks,
                    DemoAppResources.FlowDescriptionTasks,
                    GrialIconsFont.Dashboard,
                    typeof(PerformanceDashboardNavigationPage),
                    FlowType.Tasks)
                {
                    IndividualPages = new List<Sample>
                    {
                        new Sample(
                            AppResources.PageTitlePerformanceDashboard,
                            DemoAppResources.PerformanceDashboardMainPageDescription,
                            GrialIconsFont.Clipboard,
                            typeof(PerformanceDashboardMainPage)),
                        new Sample(
                            AppResources.PageTitleEmployeePerformanceDashboard,
                            DemoAppResources.EmployeePerformanceDashboardPageDescription,
                            GrialIconsFont.Clipboard,
                            typeof(EmployeePerformanceDashboardPage)),
                        new Sample(
                            AppResources.PageTitleEmployeeProfileDashboard,
                            DemoAppResources.EmployeeProfileDashboardPageDescription,
                            GrialIconsFont.Clipboard,
                            typeof(EmployeeProfileDashboardPage))
                    }
                },
                new FlowSample(
                    AppResources.FlowTitleEcommerce,
                    DemoAppResources.FlowDescriptionEcommerce,
                    GrialIconsFont.ShoppingCart,
                    typeof(EcommerceNavigationPage),
                    FlowType.Ecommerce)
                {
                    IndividualPages = new List<Sample>
                    {
                        new Sample(
                            AppResources.PageTitleEcommerceMain,
                            DemoAppResources.EcommerceMainPageDescription,
                            GrialIconsFont.ShoppingCart,
                            typeof(EcommerceMainPage)),
                        new Sample(
                            AppResources.PageTitleProductDetail,
                            DemoAppResources.ProductDetailPageDescription,
                            GrialIconsFont.ShoppingCart,
                            typeof(ProductDetailPage)),
                        new Sample(
                            AppResources.PageTitleOrderConfirmation,
                            DemoAppResources.OrderConfirmationPageDescription,
                            GrialIconsFont.ShoppingCart,
                            typeof(OrderConfirmationPage)),
                        new Sample(
                            AppResources.PageTitleCheckout,
                            DemoAppResources.CheckoutPageDescription,
                            GrialIconsFont.ShoppingCart,
                            typeof(CheckoutPage))
                    }
                },
                new FlowSample(
                    AppResources.FlowTitleMessages,
                    DemoAppResources.FlowDescriptionMessages,
                    GrialIconsFont.MessagesSquare,
                    typeof(ChatNavigationPage),
                    FlowType.Chat)
                {
                    IndividualPages = new List<Sample>
                    {
                        new Sample(
                            AppResources.PageTitleChatMain,
                            DemoAppResources.ChatMainPageDescription,
                            GrialIconsFont.MessagesCircle,
                            typeof(ChatMainPage)),
                        new Sample(
                            AppResources.PageTitleChatMessages,
                            DemoAppResources.ChatMessagesPageDescription,
                            GrialIconsFont.MessagesCircle,
                            typeof(ChatMessagesPage)),
                        new Sample(
                            AppResources.PageTitleContactDetail,
                            DemoAppResources.ContactDetailPageDescription,
                            GrialIconsFont.User,
                            typeof(ContactDetailPage)),
                        new Sample(
                            AppResources.PageTitleAddContact,
                            DemoAppResources.AddContactPageDescription,
                            GrialIconsFont.UserPlus,
                            typeof(AddContactPage))
                    }
                }
            };
        }


        public static List<SampleCategory> CreateSamples()
        {
            return new List<SampleCategory>
            {

                new SampleCategory
                {
                    Id = -1,
                    Name = "Flujos",
                    Icon = GrialIconsFont.Activity,
                    AllSamplesList = CreateFlows().Select(smp => (Sample) smp).ToList()
                },

                new SampleCategory
                {
                    Id = 0,
                    Name = AppResources.StringCategorySocial,
                    Icon = GrialIconsFont.User,
                    AllSamplesList = new List<Sample>
                    {
                        Flows.First(x => x.FlowType == FlowType.Chat),
                        new Sample(
                            AppResources.SampleUserProfile,
                            DemoAppResources.ProfilePageTimelinePageDescription,
                            GrialIconsFont.AccountCircle,
                            typeof(ProfilePage)),
                        new Sample(
                            AppResources.PageTitleSocial,
                            DemoAppResources.SocialPageDescription,
                            GrialIconsFont.Users,
                            typeof(SocialPage)),
                        new Sample(
                            AppResources.PageTitleSocialVariant,
                            DemoAppResources.SocialVariantPageDescription,
                            GrialIconsFont.Users,
                            typeof(SocialVariantPage)),
                        new Sample(
                            AppResources.PageTitleSocialCard,
                            DemoAppResources.SocialCardPageDescription,
                            GrialIconsFont.User,
                            typeof(SocialCardPage)),
                        new Sample(
                            AppResources.PageTitleContactSimpleDetail,
                            DemoAppResources.ContactSimpleDetailPageDescription,
                            GrialIconsFont.User,
                            typeof(ContactSimpleDetailPage))
                        {
                            IsModal = true
                        }
                    }
                },

                new SampleCategory
                {
                    Id = 1,
                    Name = AppResources.StringCategoryArticles,
                    Icon = GrialIconsFont.File,
                    AllSamplesList = new List<Sample>
                    {
                        new Sample(
                            AppResources.PageTitleParallaxHeaderArticle,
                            DemoAppResources.FullHeaderArticlePagePageDescription,
                            GrialIconsFont.File,
                            typeof(ParallaxHeaderArticlePage)),
                        new Sample(
                            AppResources.PageTitleCardArticle,
                            DemoAppResources.CardArticlePageDescription,
                            GrialIconsFont.File,
                            typeof(CardArticlePage)),
                        new Sample(
                            AppResources.PageTitleCurvedHeaderArticle,
                            DemoAppResources.CurvedHeaderArticlePageDescription,
                            GrialIconsFont.File,
                            typeof(CurvedHeaderArticlePage)),
                        new Sample(
                            AppResources.PageTitleArticlesBrowser,
                            DemoAppResources.ArticlesBrowserPageDescription,
                            GrialIconsFont.File,
                            typeof(ArticlesBrowserPage)),
                        new Sample(
                            AppResources.PageTitleArticlesClassicView,
                            DemoAppResources.ArticlesClassicViewPageDescription,
                            GrialIconsFont.File,
                            typeof(ArticlesClassicViewPage)),
                        new Sample(
                            AppResources.PageTitleArticleDetail,
                            DemoAppResources.ArticleDetailPageDescription,
                            GrialIconsFont.File,
                            typeof(ArticleDetailPage)),
                        new Sample(
                            AppResources.PageTitleArticlesColumns,
                            DemoAppResources.ArticlesColumnPageDescription,
                            GrialIconsFont.FileText,
                            typeof(ArticlesColumnsPage)),
                        new Sample(
                            AppResources.PageTitleArticlesList,
                            DemoAppResources.ArticlesListPageDescription,
                            GrialIconsFont.FileText,
                            typeof(ArticlesListPage)),
                        new Sample(
                            AppResources.PageTitleArticlesListVariant,
                            DemoAppResources.ArticlesListPageDescription,
                            GrialIconsFont.FileText,
                            typeof(ArticlesListVariantPage)),
                        new Sample(
                            AppResources.PageTitleArticlesFeed,
                            DemoAppResources.ArticlesFeedPageDescription,
                            GrialIconsFont.FileText,
                            typeof(ArticlesFeedPage))
                    }
                },

                new SampleCategory
                {
                    Id = 2,
                    Name = AppResources.StringCategoryECommerce,
                    Icon = GrialIconsFont.ShoppingCart,
                    AllSamplesList = new List<Sample>
                    {
                        Flows.First(x => x.FlowType == FlowType.Ecommerce),
                        new Sample(
                            AppResources.SampleProductFullscreen,
                            DemoAppResources.ProductImageFullScreenPageDescription,
                            GrialIconsFont.Gift,
                            typeof(ProductItemFullScreenPage)),
                        new Sample(
                            AppResources.PageTitleProductsCatalog,
                            DemoAppResources.ProductsCatalogPageDescription,
                            GrialIconsFont.Gift,
                            typeof(ProductsCatalogPage)),
                        new Sample(
                            AppResources.PageTitleProductOrder,
                            DemoAppResources.ProductOrderPageDescription,
                            GrialIconsFont.Gift,
                            typeof(ProductOrderPage)),
                        new Sample(
                            AppResources.PageTitleProductsGrid,
                            DemoAppResources.ProductsGridPageDescription,
                            GrialIconsFont.Module,
                            typeof(ProductsGridPage)),
                        new Sample(
                            AppResources.PageTitleProductsGridVariant,
                            DemoAppResources.ProductsGridVariantPageDescription,
                            GrialIconsFont.Module,
                            typeof(ProductsGridVariantPage)),
                        new Sample(
                            AppResources.SampleProductItemView,
                            DemoAppResources.ProductItemViewPageDescription,
                            GrialIconsFont.Gift,
                            typeof(ProductItemViewPage)),
                        new Sample(
                            AppResources.SampleProductCarousel,
                            DemoAppResources.ProductsCarouselPageDescription,
                            GrialIconsFont.Gift,
                            typeof(ProductsCarouselPage))
                    }
                },

                new SampleCategory
                {
                    Id = 3,
                    Name = AppResources.StringCategoryNavigation,
                    Icon = GrialIconsFont.Compass,
                    AllSamplesList = new List<Sample>
                    {
                        Flows.First(x => x.FlowType == FlowType.Movies),
                        new Sample(
                            AppResources.PageTitleDashboardCarousel,
                            DemoAppResources.DashboardCarouselPageDescription,
                            GrialIconsFont.Dashboard,
                            typeof(DashboardCarouselPage))
                        {
                            // Hidden from sample app as it's a copy of the demo app main dashboard  
                            // which the user already sees and is confusing to see it as an example too
                            IsHidden = true
                        },
                        new Sample(
                            AppResources.PageTitleNavigationCardsDescriptionList,
                            DemoAppResources.NavigationCardsDescriptionListPageDescription,
                            GrialIconsFont.List,
                            typeof(NavigationCardsDescriptionListPage))
                        {
                            // Hidden from sample app as it's a copy of the demo app category list
                            // which the user already sees and is confusing to see it as an example too
                            IsHidden = true
                        },
                        new Sample(
                            AppResources.PageTitleNavigationCardsList,
                            DemoAppResources.NavigationCardsListPageDescription,
                            GrialIconsFont.List,
                            typeof(NavigationCardsListPage)),
                        new Sample(
                            AppResources.PageTitleNavigationFlatList,
                            DemoAppResources.NavigationFlatListPageDescription,
                            GrialIconsFont.List,
                            typeof(NavigationFlatListPage)),
                        new Sample(
                            AppResources.PageTitleNavigationListWithImages,
                            DemoAppResources.NavigationListWithImagesPageDescription,
                            GrialIconsFont.List,
                            typeof(NavigationListWithImagesPage)),
                        new Sample(
                            AppResources.PageTitleNavigationListWithIcons,
                            DemoAppResources.NavigationListWithIconsPageDescription,
                            GrialIconsFont.List,
                            typeof(NavigationListWithIconsPage)),
                        new Sample(
                            AppResources.PageTitleDashboardCards,
                            DemoAppResources.DashboardCardsPageDescription,
                            GrialIconsFont.Dashboard,
                            typeof(DashboardCardsPage)),
                        new Sample(
                            AppResources.PageTitleDashboardMultipleTiles,
                            DemoAppResources.DashboardMultipleTilesPageDescription,
                            GrialIconsFont.Dashboard,
                            typeof(DashboardMultipleTilesPage)),
                        new Sample(
                            AppResources.PageTitleIconsDashboard,
                            DemoAppResources.IconsDashboardPageDescription,
                            GrialIconsFont.Dashboard,
                            typeof(IconsDashboardPage)),
                        new Sample(
                            AppResources.PageTitleFlatDashboard,
                            DemoAppResources.FlatDashboardPageDescription,
                            GrialIconsFont.Dashboard,
                            typeof(FlatDashboardPage)),
                        new Sample(
                            AppResources.PageTitleImagesDashboard,
                            DemoAppResources.ImagesDashboardPageDescription,
                            GrialIconsFont.Dashboard,
                            typeof(ImagesDashboardPage)),
                        new Sample(
                            AppResources.PageTitleTabControlCustomSample,
                            DemoAppResources.TabControlCustomSamplePageDescription,
                            GrialIconsFont.Tab,
                            typeof(TabControlCustomSamplePage)),
                        new Sample(
                            AppResources.PageTitleTabControlAndroidSample,
                            DemoAppResources.TabControlAndroidSamplePageDescription,
                            GrialIconsFont.Tab,
                            typeof(TabControlAndroidSamplePage)),
                        new Sample(
                            AppResources.PageTitleTabControliOSSample,
                            DemoAppResources.TabControliOSSamplePageDescription,
                            GrialIconsFont.Tab,
                            typeof(TabControliOSSamplePage)),
                        new Sample(
                            AppResources.PageTitleTabControlBottomPlacementSample,
                            DemoAppResources.TabControlBottomPlacementSamplePageDescription,
                            GrialIconsFont.Tab,
                            typeof(TabControlBottomPlacementSamplePage)),
                        new Sample(
                            AppResources.PageTitleTab,
                            DemoAppResources.TabPageDescription,
                            GrialIconsFont.Tab,
                            typeof(TabPage))
                    }
                },

                new SampleCategory
                {
                    Id = 4,
                    Name = AppResources.StringCategoryForms,
                    Icon = GrialIconsFont.Edit3,
                    AllSamplesList = new List<Sample>
                    {
                        new Sample(
                            AppResources.PageTitleTabbedLogin,
                            DemoAppResources.TabbedLoginPageDescription,
                            GrialIconsFont.Tab,
                            typeof(TabbedLoginPage))
                        {
                            IsModal = true
                        },
                        new Sample(
                            AppResources.PageTitleFullBackgroundLogin,
                            DemoAppResources.FullBackgroundLoginPageDescription,
                            GrialIconsFont.Lock,
                            typeof(FullBackgroundLoginPage))
                        {
                            IsModal = true
                        },
                        new Sample(
                            AppResources.PageTitleFullBackgroundSignup,
                            DemoAppResources.FullBackgroundSignupPageDescription,
                            GrialIconsFont.Lock,
                            typeof(FullBackgroundSignupPage))
                        {
                            IsModal = true
                        },
                        new Sample(
                            AppResources.PageTitleOrganizationForm,
                            DemoAppResources.OrganizationFormPageDescription,
                            GrialIconsFont.FileText,
                            typeof(OrganizationFormPage)),
                        new Sample(
                            AppResources.PageTitleEmployeeForm,
                            DemoAppResources.EmployeeFormPageDescription,
                            GrialIconsFont.FileText,
                            typeof(EmployeeFormPage)),
                        new Sample(
                            AppResources.PageTitleSimpleSignUp,
                            DemoAppResources.SimpleSignUpPageDescription,
                            GrialIconsFont.CheckCircle,
                            typeof(SimpleSignUpPage))
                        {
                            IsModal = true
                        },
                        new Sample(
                            AppResources.PageTitleSimpleLogin,
                            DemoAppResources.SimpleLoginPageDescription,
                            GrialIconsFont.CheckCircle,
                            typeof(SimpleLoginPage))
                        {
                            IsModal = true
                        },
                        new Sample(
                            AppResources.PageTitleLogin,
                            DemoAppResources.LoginPageDescription,
                            GrialIconsFont.Lock, 
                            typeof(LoginPage))
                        {
                            IsModal = true
                        },
                        new Sample(
                            AppResources.PageTitleSignUp,
                            DemoAppResources.SignUpPageDescription,
                            GrialIconsFont.CheckCircle, 
                            typeof(SignUpPage))
                        {
                            IsModal = true
                        },
                        new Sample(
                            AppResources.PageTitlePasswordRecovery,
                            DemoAppResources.PasswordRecoveryPageDescription,
                            GrialIconsFont.SettingsRestore, 
                            typeof(PasswordRecoveryPage))
                        {
                            IsModal = true
                        },
                    }
                },

                new SampleCategory
                {
                    Id = 5,
                    Name = AppResources.StringCategoryOnboarding,
                    Icon = GrialIconsFont.Carousel,
                    AllSamplesList = new List<Sample>
                    {
                        new Sample(
                            AppResources.PageTitleVideoCarouselHighlights,
                            DemoAppResources.VideoCarouselHighlightsPageDescription,
                            GrialIconsFont.Carousel,
                            typeof(VideoCarouselHighlightsPage))
                        {
                            IsModal = true
                        },
                        new Sample(
                            AppResources.SampleTitleWalkthrough,
                            DemoAppResources.WalkthroughPageDescription,
                            GrialIconsFont.Carousel, 
                            typeof(WalkthroughPage))
                        {
                            IsModal = true
                        },
                        new Sample(
                            AppResources.PageTitleWalkthroughGradient,
                            DemoAppResources.WalkthroughPageDescription,
                            GrialIconsFont.Carousel, 
                            typeof(WalkthroughGradientPage))
                        {
                            IsModal = true
                        },
                        new Sample(
                            AppResources.PageTitleWalkthroughIllustration,
                            DemoAppResources.WalkthroughPageDescription,
                            GrialIconsFont.Carousel, 
                            typeof(WalkthroughIllustrationPage))
                        {
                            IsModal = true
                        },
                        new Sample(
                            AppResources.PageTitleWalkthroughImage,
                            DemoAppResources.WalkthroughPageDescription,
                            GrialIconsFont.Carousel, 
                            typeof(WalkthroughImagePage))
                        {
                            IsModal = true
                        },
                        new Sample(
                            AppResources.PageTitleWalkthroughMinimal,
                            DemoAppResources.WalkthroughPageDescription,
                            GrialIconsFont.Carousel, 
                            typeof(WalkthroughMinimalPage))
                        {
                            IsModal = true
                        },
                        new Sample(
                            AppResources.SampleTitleWalkthroughFlat,
                            DemoAppResources.WalkthroughFlatPageDescription,
                            GrialIconsFont.Carousel, 
                            typeof(WalkthroughFlatPage))
                        {
                            IsModal = true
                        },
                        new Sample(
                            AppResources.SampleTitleWalkthroughVariant,
                            DemoAppResources.WalkthroughPageDescription,
                            GrialIconsFont.Carousel, 
                            typeof(WalkthroughVariantPage))
                        {
                            IsModal = true
                        },
                        new Sample(
                            AppResources.PageTitleEmptyState,
                            DemoAppResources.EmptyStatePageDescription,
                            GrialIconsFont.Hourglass,
                            typeof(EmptyStatePage)),
                        new Sample(
                            AppResources.PageTitleWelcome,
                            DemoAppResources.EmptyStatePageDescription,
                            GrialIconsFont.MapPin, 
                            typeof(WelcomePage))
                        {
                            IsModal = true
                        }
                    }
                },

                new SampleCategory
                {
                    Id = 6,
                    Name = AppResources.StringCategoryMessages,
                    Icon = GrialIconsFont.Mail,
                    AllSamplesList = new List<Sample>
                    {
                        new Sample(
                            AppResources.PageTitleChatTimeline,
                            DemoAppResources.ChatTimelinePageDescription,
                            GrialIconsFont.Clock,
                            typeof(ChatTimelinePage)),
                        new Sample(
                            AppResources.PageTitleRecentChatList,
                            DemoAppResources.RecentChatListPageDescription,
                            GrialIconsFont.Clock,
                            typeof(RecentChatListPage)),
                        new Sample(
                            AppResources.PageTitleMessagesList,
                            DemoAppResources.MessagesListPageDescription,
                            GrialIconsFont.Mail,
                            typeof(MessagesListPage)),
                        new Sample(
                            AppResources.PageTitleChatMessagesList,
                            DemoAppResources.ChatMessagesListPageDescription,
                            GrialIconsFont.MessagesCircle,
                            typeof(ChatMessagesListPage)),
                        new Sample(
                            AppResources.PageTitleNotifications,
                            DemoAppResources.NotificationsPageDescription,
                            GrialIconsFont.Bell,
                            typeof(NotificationsPage)),
                        new Sample(
                            AppResources.PageTitleTimeline,
                            DemoAppResources.TimelinePageDescription,
                            GrialIconsFont.Clock,
                            typeof(TimelinePage)),
                    }
                },

                new SampleCategory
                {
                    Id = 7,
                    Name = AppResources.StringCategoryDataViz,
                    Icon = GrialIconsFont.PieChart,
                    AllSamplesList = new List<Sample>
                    {
                        Flows.First(x => x.FlowType == FlowType.Tasks),
                        new Sample(
                            AppResources.PageTitleDocumentTimeline,
                            DemoAppResources.DocumentTimelinePageDescription,
                            GrialIconsFont.Clock,
                            typeof(DocumentTimelinePage)),
                        new Sample(
                            AppResources.PageTitleShippingDetail,
                            DemoAppResources.ShippingDetailPageDescription,
                            GrialIconsFont.Module,
                            typeof(ShippingDetailPage)),
                        new Sample(
                            AppResources.PageTitleTaskBrowser,
                            DemoAppResources.TaskBrowserPageDescription,
                            GrialIconsFont.BarChart,
                            typeof(TaskBrowserPage)),
                        new Sample(
                            AppResources.PageTitleTaskOverview,
                            DemoAppResources.TaskOverviewPageDescription,
                            GrialIconsFont.BarChart,
                            typeof(TaskOverviewPage))
                    }
                },

                new SampleCategory
                {
                    Id = 8,
                    Name = AppResources.StringCategoryTheme,
                    Icon = GrialIconsFont.ColorPalette,
                    AllSamplesList = new List<Sample>
                    {
                        new Sample(
                            AppResources.PageTitleThemeOverview,
                            DemoAppResources.ThemeOverviewPageDescription,
                            GrialIconsFont.ColorPalette,
                            typeof(ThemeOverviewPage)),
                        new Sample(
                            AppResources.PageTitlePopups,
                            DemoAppResources.PopupsPageDescription,
                            GrialIconsFont.MessageCircle,
                            typeof(PopupsPage)),
                        new Sample(
                            AppResources.PageTitleGenericAbout,
                            DemoAppResources.GenericAboutPageDescription,
                            GrialIconsFont.HelpCircle,
                            typeof(GenericAboutPage)),
                        new Sample(
                            AppResources.PageTitleRichAbout,
                            DemoAppResources.RichAboutPageDescription,
                            GrialIconsFont.HelpCircle,
                            typeof(RichAboutPage)),
                        new Sample(
                            AppResources.PageTitleCustomSettings,
                            DemoAppResources.CustomSettingsPageDescription,
                            GrialIconsFont.Settings,
                            typeof(CustomSettingsPage)),
                        new Sample(
                            AppResources.PageTitleCustomActivityIndicator,
                            DemoAppResources.CustomActivityIndicatorPageDescription,
                            GrialIconsFont.Loader,
                            typeof(CustomActivityIndicatorPage)),
                        new Sample(
                            AppResources.PageTitleResponsiveHelpers,
                            DemoAppResources.ResponsiveHelpersPageDescription,
                            GrialIconsFont.Tablet,
                            typeof(ResponsiveHelpersPage)),
                        new Sample(
                            AppResources.PageTitleSettings,
                            DemoAppResources.SettingsPageDescription,
                            GrialIconsFont.Settings,
                            typeof(SettingsPage))
                    }
                }
            };
        }

    }


    public class SampleCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public List<Sample> AllSamplesList { get; set; }
        public IEnumerable<Sample> SamplesList => AllSamplesList.Where(x => !x.IsHidden);
        public int ScreenCount => SamplesList.Sum(x => x.ScreenCount);
        public string ScreenCountDescription => string.Format(DemoAppResources.ScreenCount, ScreenCount);
    }


    public class Sample
    {
        public Sample(string name, string description, string icon, Type pageType)
        {
            Name = name;
            Description = description;
            Icon = icon;
            PageType = pageType;
        }

        public string Description { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public Type PageType { get; set; }
        public bool IsModal { get; set; }
        public bool IsHidden { get; set; }
        public bool IsFlow { get; set; }
        public virtual int ScreenCount => 1;
    }


    public class FlowSample : Sample
    {
        public FlowSample(string name, string description, string icon, Type pageType, FlowType flowType)
            : base(name, description, icon, pageType)
        {
            FlowType = flowType;
            IsFlow = true;
        }

        public FlowType FlowType { get; set; }
        public List<Sample> IndividualPages { get; set; }
        public override int ScreenCount => IndividualPages.Count;
    }
}
