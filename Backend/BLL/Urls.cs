using System.Configuration;

namespace BLL
{
    public class Urls
    {
        private const string PreFixUrl = "http://api.themoviedb.org/3/";
        private static readonly string SuffixUrl = "?api_key=" + ConfigurationManager.AppSettings.Get("theMovieDb");
        
        private const string PreFixUrlNotify = "https://www.notifymyandroid.com/publicapi/notify?apikey=";
        private static readonly string ApiKey = ConfigurationManager.AppSettings.Get("notifyMyAndroid");

        /// <summary>
        /// Movie urls
        /// </summary>
        public static string SearchMovie = FormatUrl("search/movie");
        public static string SearchMovieById = "movie/";
        public static string UpcomingMovies = FormatUrl("movie/upcoming");

        /// <summary>
        /// Tv urls
        /// </summary>
        public static string SearchTv = FormatUrl("search/tv");
        public static string SearchTvById = "tv/";
        public static string TopRated = FormatUrl("tv/top_rated");
        public static string Seasons = "tv/57243/season/8" + SuffixUrl;

        /// <summary>
        /// Person urls
        /// </summary>
        public static string SearchPerson = FormatUrl("search/person");
        public static string SearchPersonById = "person/";
        public static string PopularPersons = FormatUrl("person/popular");

        /// <summary>
        /// NotifyMyAndroid URL
        /// </summary>
        public static string NotifyMyAndroid = PreFixUrlNotify + ApiKey + "&application=Watcher&event=New%20Releases&description=";
        
        public static string SearchTvSeasons(int tvId, int season)
        {
            return string.Format("{0}{1}{2}{3}{4}{5}", PreFixUrl, "tv/", tvId, "/season/", season, SuffixUrl); 
        }

        public static string SearchBy(string searchUrl, int id)
        {
            return string.Format("{0}{1}{2}{3}", PreFixUrl, searchUrl, id, SuffixUrl); 
        }

        private static string FormatUrl(string urlMiddlePart)
        {
            return string.Format("{0}{1}{2}", PreFixUrl, urlMiddlePart, SuffixUrl);
        }
    }
}