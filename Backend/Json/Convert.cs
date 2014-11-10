using System.Collections.Generic;
using System.Linq;
using Models;
using Newtonsoft.Json;

namespace Backend.Json
{
    public class Convert
    {
        public static List<Show> ToShows(string json)
        {
            var v = JsonConvert.DeserializeObject<RootObject2>(json);
            
            return Map(v);
        }

        private static List<Show> Map(RootObject2 rootObject2)
        {
            // ToDO: Use automapper
            return rootObject2.results.Select(result2 => new Show
            {
                Id = result2.id,
                Name = result2.name,
                AirDate = result2.first_air_date,
                VoteAverage = result2.vote_average,
                VoteCount = result2.vote_count
            }).ToList();
        }

        public static List<Movie> ToMovies(string json)
        {
            return null;
        }

        public static Show ToShow(string data)
        {
            return null;
        }
    }
}