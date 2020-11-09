using System.Collections.ObjectModel;
using System.Linq;
using AppName.Core;

namespace AppName
{
    public class EmployeePerformanceDashboardViewModel : ObservableObject
    {
        private FlowEmployeeData _employee;
        private string _selectedPeriod;
        private FlowRingSeriesData _selectedPeriodData;
        private FlowTasksData _tasksData;
        private FlowSeriesData _performance;
        private FlowSeriesData _workedHours;
        private FlowSeriesData _services;

        public EmployeePerformanceDashboardViewModel(FlowEmployeeData employee)
        {
            LoadData();

            if (employee != null)
            {
                Employee = employee;
            }
        }

        public ObservableCollection<string> Periods { get; } = new ObservableCollection<string>();

        public FlowEmployeeData Employee
        {
            get { return _employee; }
            set { SetProperty(ref _employee, value); }
        }

        public FlowTasksData TasksData
        {
            get { return _tasksData; }
            set { SetProperty(ref _tasksData, value); }
        }

        public FlowRingSeriesData SelectedPeriodData
        {
            get { return _selectedPeriodData; }
            set { SetProperty(ref _selectedPeriodData, value); }
        }

        public FlowSeriesData Performance
        {
            get { return _performance; }
            set { SetProperty(ref _performance, value); }
        }

        public FlowSeriesData Services
        {
            get { return _services; }
            set { SetProperty(ref _services, value); }
        }

        public FlowSeriesData WorkedHours
        {
            get { return _workedHours; }
            set { SetProperty(ref _workedHours, value); }
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
                        SelectedPeriodData = TasksData.LastMonth;
                    }
                    else if (value == Resx.AppResources.StringLastYear)
                    {
                        SelectedPeriodData = TasksData.LastYear;
                    }
                    else
                    {
                        SelectedPeriodData = TasksData.LastWeek;
                    }
                }
            }
        }

        private void LoadData()
        {
            Periods.Clear();

            JsonHelper.Instance.LoadViewModel(this, source: "TasksFlow.json");

            SelectedPeriod = Periods.FirstOrDefault();
        }
    }
}
