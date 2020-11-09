using System.Collections.ObjectModel;
using AppName.Core;

namespace AppName
{
    public class FeaturedMoviesViewModel : ObservableObject
    {
        private int _position;

        public FeaturedMoviesViewModel()
        {            
            LoadData();
        }

        public int Position
        {
            get { return _position; }
            set { SetProperty(ref _position, value); }
        }

        public ObservableCollection<FlowFeaturedMovieData> Movies { get; } = new ObservableCollection<FlowFeaturedMovieData>();

        private void LoadData()
        {
            Movies.Clear();

            JsonHelper.Instance.LoadViewModel(this, source: "MoviesFlow.json");
        }
    }
}
