using AppName.Core;
namespace AppName
{    
    public class FlowTasksData
    {
        public FlowRingSeriesData LastWeek { get; set; }
        public FlowRingSeriesData LastMonth { get; set; }
        public FlowRingSeriesData LastYear { get; set; }
    }

    public class FlowRingSeriesData
    {
        public FlowRingData[] RingSeries { get; set; }
    }

    public class FlowRingData
    {
        public int Value { get; set; }
        public string ValueLabel { get; set; }
        public string Label { get; set; }
        public string Color { get; set; }
    }
    
    public class FlowChartData
    {
        public FlowSeriesData LastWeek { get; set; }
        public FlowSeriesData LastMonth { get; set; }
        public FlowSeriesData LastYear { get; set; }
    }

    public class FlowSeriesData
    {
        public int MaxValue { get; set; }
        public FlowEntryData[] Series { get; set; }
    }

    public class FlowEntryData
    {
        public int Value { get; set; }
        public string Label { get; set; }
        public string Color { get; set; }
    }

    public class FlowEmployeeData
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Status { get; set; }
        public string Team { get; set; }
        public int Open { get; set; }
        public int Closed { get; set; }
        public int Score { get; set; }
        public string ScoreLabel { get; set; }
        public string[] Tags { get; set; }
    }

    public class FlowMetricData
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public int ValueDifference { get; set; }
    }
}
