using System.Collections.ObjectModel;
using System.Globalization;
using AppName.Core;

namespace AppName
{
    public class ArticlesColumnsViewModel : ObservableObject
    {
        private ArticleData _main;

        public ArticlesColumnsViewModel()
            : base(listenCultureChanges: true)
        {
            LoadData();
        }

        public ArticleData Main
        {
            get { return _main; }
            set { SetProperty(ref _main, value); }
        }

        public SecondaryInfo Secondary { get; } = new SecondaryInfo();

        protected override void OnCultureChanged(CultureInfo culture)
        {
            LoadData();
        }

        private void LoadData()
        {
            Secondary.List.Clear();
            Main = null;

            JsonHelper.Instance.LoadViewModel(this, source: "Articles.json");
        }

        public class SecondaryInfo
        {
            public ObservableCollection<ArticleData> List { get; } = new ObservableCollection<ArticleData>();
        }
    }
}
