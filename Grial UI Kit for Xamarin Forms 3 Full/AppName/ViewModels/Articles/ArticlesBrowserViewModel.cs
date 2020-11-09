using System.Collections.ObjectModel;
using System.Globalization;
using AppName.Core;

namespace AppName
{
    public class ArticlesBrowserViewModel : ObservableObject
    {
        public ArticlesBrowserViewModel()
            : base(listenCultureChanges: true)
        {
            LoadData();
        }

        public ObservableCollection<ArticleData> Articles { get; } = new ObservableCollection<ArticleData>();
        public ObservableCollection<ArticleData> Related { get; } = new ObservableCollection<ArticleData>();

        protected override void OnCultureChanged(CultureInfo culture)
        {
            LoadData();
        }

        private void LoadData()
        {
            Articles.Clear();
            Related.Clear();

            JsonHelper.Instance.LoadViewModel(this, source: "Articles.json");
        }
    }
}
