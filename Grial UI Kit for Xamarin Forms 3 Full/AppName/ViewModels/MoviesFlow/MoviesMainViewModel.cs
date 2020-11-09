using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AppName.Core;

namespace AppName
{
    public class MoviesMainViewModel : ObservableObject
    {
        public MoviesMainViewModel()
        {
            LoadData();
        }

        public FeaturedMoviesViewModel Featured { get; } = new FeaturedMoviesViewModel();

        public ObservableCollection<FlowMoviesSectionData> Sections { get; } = new ObservableCollection<FlowMoviesSectionData>();

        private void LoadData()
        {
            Sections.Clear();
            Featured.Movies.Clear();

            JsonHelper.Instance.LoadViewModel(this, source: "MoviesFlow.json");

            foreach (var s in Sections)
            {
                s.CreateMoviesSections();
            }
        }
    }
}
