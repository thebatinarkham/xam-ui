using System.Collections.ObjectModel;

using System.Globalization;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public class NavigationViewModel : ObservableObject
    {
        private readonly string _variantPageName;
        private NavigationCategoryData _category;
        private NavigationItemData _selectedItem;

        public NavigationViewModel(string variantPageName = null)
            : base(listenCultureChanges: true)
        {
            _variantPageName = variantPageName;

            LoadData();
        }

        public ObservableCollection<NavigationItemData> Items { get; } = new ObservableCollection<NavigationItemData>();

        public NavigationCategoryData Category
        {
            get { return _category; }
            set { SetProperty(ref _category, value); }
        }

        public NavigationItemData SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (SetProperty(ref _selectedItem, value) && value != null)
                {
                    Application.Current.MainPage.DisplayAlert("Item Selected!", $"You have selected: {value.Name}", "OK");
                    SetProperty(ref _selectedItem, null);
                }
            }
        }

        protected override void OnCultureChanged(CultureInfo culture)
        {
            LoadData();
        }

        private void LoadData()
        {
            Category = null;
            Items.Clear();

            JsonHelper.Instance.LoadViewModel(this, pageName: _variantPageName, source: "NavigationDashboards.json");
        }
    }
}