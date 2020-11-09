using System.Collections.ObjectModel;
using System.Globalization;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public class DashboardCardsViewModel : ObservableObject
    {
        public DashboardCardsViewModel()
            : base(listenCultureChanges: true)
        {
            LoadData();
        }

        public ObservableCollection<DashboardCardItemData> Items { get; } = new ObservableCollection<DashboardCardItemData>();

        protected override void OnCultureChanged(CultureInfo culture)
        {
            LoadData();
        }

        private void LoadData()
        {
            Items.Clear();

            JsonHelper.Instance.LoadViewModel(this, source: "NavigationDashboards.json");
        }
    }
}