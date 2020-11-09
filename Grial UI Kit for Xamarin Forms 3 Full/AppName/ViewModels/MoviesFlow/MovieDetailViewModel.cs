using AppName.Core;
namespace AppName
{
    public class MovieDetailViewModel : ObservableObject
    {
        private readonly FlowMovieData _targetMovie;
        private FlowMovieData _movie;

        public MovieDetailViewModel(FlowMovieData movie = null)
        {
            _targetMovie = movie;

            LoadData();
        }

        public FlowMovieData Movie
        {
            get { return _movie; }
            set { SetProperty(ref _movie, value); }
        }

        private void LoadData()
        {
            JsonHelper.Instance.LoadViewModel(this, source: "MoviesFlow.json");

            if (_targetMovie != null)
            {
                Movie = _targetMovie;
            }
        }
    }
}
