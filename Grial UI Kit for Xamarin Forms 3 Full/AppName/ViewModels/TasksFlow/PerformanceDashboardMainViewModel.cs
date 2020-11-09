using System.Collections.ObjectModel;
using System.Linq;
using AppName.Core;

namespace AppName
{
    public class PerformanceDashboardMainViewModel : ObservableObject
    {
        private string _selectedPeriod;
        private FlowSeriesData _selectedPeriodData;
        private FlowChartData _chartData;

        public PerformanceDashboardMainViewModel()
        {
            LoadData();
        }

        public ObservableCollection<FlowEmployeeData> TeamMembers { get; } = new ObservableCollection<FlowEmployeeData>();

        public ObservableCollection<FlowMetricData> Metrics { get; } = new ObservableCollection<FlowMetricData>();

        public ObservableCollection<string> Periods { get; } = new ObservableCollection<string>();

        public FlowChartData ChartData
        {
            get { return _chartData; }
            set { SetProperty(ref _chartData, value); }
        }

        public FlowSeriesData SelectedPeriodData 
        {
            get { return _selectedPeriodData; }
            set { SetProperty(ref _selectedPeriodData, value); }
        }

        public string SelectedPeriod 
        {
            get { return _selectedPeriod; }
            set 
            {
                if (SetProperty(ref _selectedPeriod, value))
                {
                    if (value == Resx.AppResources.StringLastMonth)
                    {
                        SelectedPeriodData = ChartData.LastMonth;
                    }
                    else if (value == Resx.AppResources.StringLastYear)
                    {
                        SelectedPeriodData = ChartData.LastYear;
                    }
                    else
                    {
                        SelectedPeriodData = ChartData.LastWeek;
                    }
                }
            }
        }

        private void LoadData()
        {
            TeamMembers.Clear();
            Metrics.Clear();
            Periods.Clear();

            JsonHelper.Instance.LoadViewModel(this, source: "TasksFlow.json");

            SelectedPeriod = Periods.FirstOrDefault();
        }
    }
}
