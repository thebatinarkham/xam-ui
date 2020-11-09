using System.Collections.ObjectModel;
using System.Globalization;
using AppName.Core;

namespace AppName
{
    public class TimelineViewModel : ObservableObject
    {
        public TimelineViewModel()
            : base(listenCultureChanges: true)
        {
            LoadData();
        }

        public ObservableCollection<TimelineEventData> TimelineList { get; } = new ObservableCollection<TimelineEventData>();

        protected override void OnCultureChanged(CultureInfo culture)
        {
            LoadData();
        }

        private void LoadData()
        {
            TimelineList.Clear();

            JsonHelper.Instance.LoadViewModel(this, source: "Social.json");
        }
    }
}
