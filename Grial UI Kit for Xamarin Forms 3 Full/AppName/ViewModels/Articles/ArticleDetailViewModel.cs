using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using AppName.Core;

namespace AppName
{
    public class ArticleDetailViewModel : ObservableObject
    {
        private readonly string _articlaId;
        private ArticleData _article;

        public ArticleDetailViewModel(string articleId = null)
            : base(listenCultureChanges: true)
        {
            _articlaId = articleId;

            LoadData();
        }

        public ArticleData Article
        {
            get { return _article; }
            set { SetProperty(ref _article, value); }
        }

        public ObservableCollection<ArticleCommentData> Comments { get; } = new ObservableCollection<ArticleCommentData>();

        protected override void OnCultureChanged(CultureInfo culture)
        {
            LoadData();
        }

        private void LoadData()
        {
            Comments.Clear();
            Article = null;

            JsonHelper.Instance.LoadViewModel(this, source: "Articles.json");

            if (_articlaId != null)
            {
                Article = new ArticlesListViewModel()
                    .List.FirstOrDefault(article => article.Id == _articlaId);
            }
        }
    }
}
