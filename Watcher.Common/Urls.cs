using System;
using System.Configuration;

namespace Watcher.Common
{
    public class Urls
    {
        private const string PreFixUrl = "http://api.themoviedb.org/3/";
        private static readonly string SuffixUrl = "?api_key=" + ConfigurationManager.AppSettings.Get("theMovieDb");

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

        public static string PrefixImages = "http://image.tmdb.org/t/p/w185";

        public static string SearchTvSeasons(int tvId, int season)
        {
            return $"{PreFixUrl}tv/{tvId}/season/{season}{SuffixUrl}";
        }

        public static string PersonCredits(int personId)
        {
            return $"{PreFixUrl}person/{personId}/combined_credits{SuffixUrl}";
        }

        public static string SearchBy(string searchUrl, int id)
        {
            return $"{PreFixUrl}{searchUrl}{id}{SuffixUrl}";
        }

        private static string FormatUrl(string urlMiddlePart)
        {
            return $"{PreFixUrl}{urlMiddlePart}{SuffixUrl}";
        }
    }
}
