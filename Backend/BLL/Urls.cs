using System.Configuration;

namespace BLL
{
    public class Urls
    {
        private const string PreFixUrl = "http://api.themoviedb.org/3/";
        private static readonly string SuffixUrl = "?api_key=" + ConfigurationManager.AppSettings.Get("apiKey");

        /// <summary>
        /// Movie urls
        /// </summary>
        public static string SearchMovie = FormatUrl("search/movie");
        public static string SearchMovieById = FormatUrl("movie");
        public static string UpcomingMovies = FormatUrl("movie/upcoming");

        /// <summary>
        /// Tv urls
        /// </summary>
        public static string SearchTv = FormatUrl("search/tv");
        public static string SearchTvById = FormatUrl("tv");
        public static string TopRated = FormatUrl("tv/top_rated");

        /// <summary>
        /// Person urls
        /// </summary>
        public static string SearchPerson = FormatUrl("search/person");
        public static string PopularPersons = FormatUrl("person/popular");
        
        private static string FormatUrl(string urlMiddlePart)
        {
            return string.Format("{0}{1}{2}", PreFixUrl, urlMiddlePart, SuffixUrl);
        }
    }
}