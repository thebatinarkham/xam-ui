using System.Collections.ObjectModel;
using System.Globalization;
using AppName.Core;

namespace AppName
{
    public class SocialViewModel : ObservableObject
    {
        private readonly string _variantPageName;
        private ProfileData _profile;

        public SocialViewModel(string variantPageName = null)
            : base(listenCultureChanges: true)
        {
            _variantPageName = variantPageName;

            LoadData();
        }

        public ProfileData Profile
        {
            get { return _profile; }
            set { SetProperty(ref _profile, value); }
        }

        public RelatedInfo Related { get; } = new RelatedInfo();

        protected override void OnCultureChanged(CultureInfo culture)
        {
            LoadData();
        }

        private void LoadData()
        {
            Related.Friends.Clear();
            Related.Images.Clear();
            Profile = null;

            JsonHelper.Instance.LoadViewModel(this, pageName: _variantPageName, source: "Social.json");
        }

        public class RelatedInfo
        {
            public ObservableCollection<FriendData> Friends { get; } = new ObservableCollection<FriendData>();
            public ObservableCollection<string> Images { get; } = new ObservableCollection<string>();
        }
    }
}
