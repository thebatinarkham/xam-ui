using AppName.Core;
namespace AppName
{
    public class DashboardCardItemData
    {
        public string Title { get; set; }
        public string Section { get; set; }
        public string Author { get; set; }
        public string Avatar { get; set; }
        public string BackgroundImage { get; set; }
        public string BackgroundColor { get; set; }
    }

    public class DashboardMultipleTileItemData
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Avatar { get; set; }
        public string BackgroundColor { get; set; }
        public string BackgroundImage { get; set; }
        public bool ShowBackgroundColor { get; set; }
        public bool IsNotification { get; set; }
        public string Icon { get; set; }
        public int Badge { get; set; }
    }

    public class NavigationItemData
    {
        public string Name { get; set; }
        public string BackgroundColor { get; set; }
        public string BackgroundImage { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public int Badge { get; set; }
        public int ItemCount { get; set; }
    }

    public class NavigationCategoryData
    {
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}
