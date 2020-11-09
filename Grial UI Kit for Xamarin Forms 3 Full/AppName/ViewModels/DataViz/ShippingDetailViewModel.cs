using System.Collections.ObjectModel;
using System.Globalization;
using AppName.Core;

namespace AppName
{
    public class ShippingDetailViewModel : ObservableObject
    {
        public ShippingDetailViewModel()
            : base(listenCultureChanges: true)
        {
            LoadData();
        }

        public ObservableCollection<ShipmentData> InTransit { get; } = new ObservableCollection<ShipmentData>();
        public ObservableCollection<ShipmentData> Closed { get; } = new ObservableCollection<ShipmentData>();
        public ObservableCollection<ShipmentData> All { get; } = new ObservableCollection<ShipmentData>();

        protected override void OnCultureChanged(CultureInfo culture)
        {
            LoadData();
        }

        private void LoadData()
        {
            InTransit.Clear();
            Closed.Clear();
            All.Clear();

            JsonHelper.Instance.LoadViewModel(this, source: "DataViz.json");
        }
    }
}
