using System;
using System.Collections.ObjectModel;
using System.Linq;
using AppName.Core;

namespace AppName
{
    public class EmployeeProfileDashboardViewModel : ObservableObject
    {
        private FlowEmployeeData _employee;
        private string _notes;

        public EmployeeProfileDashboardViewModel(FlowEmployeeData employee)
        {
            LoadData();

            if (employee != null)
            {
                Employee = employee;
            }

            Remove(Employee);
        }

        public ObservableCollection<FlowEmployeeData> TeamMembers { get; } = new ObservableCollection<FlowEmployeeData>();

        public FlowEmployeeData Employee
        {
            get { return _employee; }
            set { SetProperty(ref _employee, value); }
        }

        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }

        private void Remove(FlowEmployeeData employee)
        {
            for (var i = 0; i < TeamMembers.Count; i++)
            {
                if (TeamMembers[i].Name == employee.Name)
                {
                    TeamMembers.RemoveAt(i);
                    break;
                }
            }
        }

        private void LoadData()
        {
            TeamMembers.Clear();

            JsonHelper.Instance.LoadViewModel(this, source: "TasksFlow.json");
        }
    }
}