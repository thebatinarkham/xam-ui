using System.Globalization;
using AppName.Core;

namespace AppName
{
    public class ProfileViewModel : ObservableObject
    {
        private string _name;
        private string _description;
        private string _image;

        public ProfileViewModel()
            : base(listenCultureChanges: true)
        {
            LoadData();
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public string Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        protected override void OnCultureChanged(CultureInfo culture)
        {
            LoadData();
        }

        private void LoadData()
        {
            JsonHelper.Instance.LoadViewModel(this, source: "Social.json");
        }
    }
}
