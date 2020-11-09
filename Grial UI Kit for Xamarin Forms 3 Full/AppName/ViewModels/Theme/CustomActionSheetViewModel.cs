using System;
using System.Collections.ObjectModel;
using System.Globalization;
using AppName.Core;

namespace AppName
{
    public class CustomActionSheetViewModel : ObservableObject
    {
        private string _title;

        public CustomActionSheetViewModel()
            : base(listenCultureChanges: true)
        {
            LoadData();
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<ActionData> Actions { get; } = new ObservableCollection<ActionData>();

        protected override void OnCultureChanged(CultureInfo culture)
        {
            LoadData();
        }

        private void LoadData()
        {
            Actions.Clear();

            JsonHelper.Instance.LoadViewModel(this, source: "Theme.json", pageName: "CustomActionSheet.xaml");
        }

        public class ActionData
        {
            public string Icon { get; set; }

            public string Label { get; set; }
        }
    }
}
