using System;
using System.Collections.ObjectModel;
using System.Globalization;
using AppName.Core;

namespace AppName
{
    public class ContactSimpleDetailViewModel : ObservableObject
    {
        private string _avatar;

        public ContactSimpleDetailViewModel()
            : base(listenCultureChanges: true)
        {
            LoadData();
        }

        public string Avatar
        {
            get { return _avatar; }
            set { SetProperty(ref _avatar, value); }
        }

        public ObservableCollection<ValueData> Values { get; } = new ObservableCollection<ValueData>();

        protected override void OnCultureChanged(CultureInfo culture)
        {
            LoadData();
        }

        private void LoadData()
        {
            Values.Clear();

            JsonHelper.Instance.LoadViewModel(this, source: "Social.json");
        }

        public class ValueData
        {
            public string Label { get; set; }
            public string Value { get; set; }
        }
    }
}
