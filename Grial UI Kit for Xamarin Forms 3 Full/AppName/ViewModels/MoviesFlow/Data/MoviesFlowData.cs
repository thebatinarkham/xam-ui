using System.Linq;
using AppName.Resx;
using AppName.Core;

namespace AppName
{
    public class FlowMovieData
    {
        public FlowMoviesSectionData Section { get; set; }
        public string Title { get; set; }
        public string Source { get; set; }
        public string Seasons { get; set; }
        public double RatingValue { get; set; }
        public double RatingMax { get; set; }
        public string Director { get; set; }
        public string Plot { get; set; }
        public string TrailerVideo { get; set; }
        public string[] Cast { get; set; }
        public string[] Genres { get; set; }
    }

    public class FlowMoviesSectionData
    {
        public string Title { get; set; }
        public FlowMovieData[] Movies { get; set; }

        public void CreateMoviesSections()
        {
            foreach (var movie in Movies)
            {
                movie.Section = new FlowMoviesSectionData
                {
                    Title = string.Format(AppResources.StringMoreFrom, Title),
                    Movies = Movies.Where(x => x.Title != movie.Title).ToArray()
                };
            }
        }
    }

    public class FlowFeaturedMovieData
    {
        public string Name { get; set; }
        public string Director { get; set; }
        public string Year { get; set; }
        public string PosterImage { get; set; }
        public string BackgroundImage { get; set; }
        public string Overview { get; set; }
        public string TrailerVideo { get; set; }
        public int Rating { get; set; }
        public string RatingLabel { get; set; }
        public string AlternativeImage { get; set; }
        public string BackdropImage { get; set; }
        public string Color { get; set; }
        public string[] Genres { get; set; }
        public FlowFeaturedMovieCrewData[] Crew { get; set; }
        public FlowFeaturedMovieCastData[] Cast { get; set; }
        public string[] Pictures { get; set; }
    }

    public class FlowFeaturedMovieCrewData
    {
        public string Name { get; set; }
        public string Role { get; set; }
    }

    public class FlowFeaturedMovieCastData
    {
        public string Name { get; set; }
        public string Character { get; set; }
        public string Avatar { get; set; }
    }
}
