using AppName.Core;
namespace AppName
{
    public class ShipmentData
    {
        public string Id { get; set; }
        public string Icon { get; set; }
        public double Progress { get; set; }
        public string Time { get; set; }
        public double Weight { get; set; }
        public double Price { get; set; }
    }

    public class TaskOverviewGroupData
    {
        public string Name { get; set; }
        public int Completed { get; set; }
        public int Remaining { get; set; }
        public string RemainingTasks => string.Format(Resx.AppResources.RemainingTasks, Remaining);
        public int[] Activity { get; set; }
    }

    public class BrowserTaskOverviewData
    {
        public string Title { get; set; }
        public int Total { get; set; }
        public int[] Evolution { get; set; }
    }

    public class BrowserPeopleTaskOverviewData
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Division { get; set; }
        public double Change { get; set; }
        public int[] Evolution { get; set; }
    }

    public class DocumentTimelineEventData
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Icon { get; set; }
        public string When { get; set; }
        public bool IsInbound { get; set; }
    }
}
