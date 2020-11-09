using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public class DashboardCarouselViewModel : ObservableObject
    {
        public DashboardCarouselViewModel()
            : base(listenCultureChanges: true)
        {
            LoadData();
        }

        public ObservableCollection<NavigationItemDataWithCommand> Items { get; } = new ObservableCollection<NavigationItemDataWithCommand>();
        public ObservableCollection<NavigationItemDataWithCommand> Headers { get; } = new ObservableCollection<NavigationItemDataWithCommand>();

        protected override void OnCultureChanged(CultureInfo culture)
        {
            LoadData();
        }

        private void LoadData()
        {
            Items.Clear();
            Headers.Clear();

            JsonHelper.Instance.LoadViewModel(this, source: "NavigationDashboards.json");
        }

        public class NavigationItemDataWithCommand : NavigationItemData
        {
            public NavigationItemDataWithCommand()
            {
                TapCommand = new Command(
                    () => Application.Current.MainPage.DisplayAlert("Item Tapped!", $"You have selected: {Name}", "OK"));
            }

            public ICommand TapCommand { get; }
        }
    }
}
