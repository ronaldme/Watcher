using System.Collections.Generic;
using System.Linq;
using BLL.Json.Movies;
using BLL.Json.Persons;
using BLL.Json.Shows;
using Messages.DTO;
using Newtonsoft.Json;

namespace BLL.Json
{
    public class Convert
    {
        public static List<MovieDTO> toNew(string json)
        {
            var v = JsonConvert.DeserializeObject<RootObjectMovies>(json);

            return v.Results.Select(rootObjectMovie => new MovieDTO
            {
                Id = rootObjectMovie.Id,
                Name = rootObjectMovie.Title,
                ReleaseDate = rootObjectMovie.Release_Date
            }).ToList();
        }

        public static List<TvShowDTO> ToShows(string json)
        {
            return Map(JsonConvert.DeserializeObject<Root>(json));
        }

        public static TvShowDTO ToShow(string json)
        {
            var v = JsonConvert.DeserializeObject<Item>(json);

            return new TvShowDTO
            {
                Id = v.Id,
                Name = v.Name,
                OriginalName = v.Original_Name,
                AirDate = v.First_Air_Date,
                VoteAverage = v.Vote_Average,
                VoteCount = v.Vote_Count
            };
        }

        private static List<TvShowDTO> Map(Root rootObject2)
        {
            return rootObject2.Results.Select(result2 => new TvShowDTO
            {
                Id = result2.Id,
                Name = result2.Name,
                AirDate = result2.First_Air_Date,
                VoteAverage = result2.Vote_Average,
                VoteCount = result2.Vote_Count
            }).ToList();
        }

        public static List<MovieDTO> ToMovies(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<RootObjectMovies>(json);

            return deserialized.Results.Select(result => new MovieDTO
            {
                Id = result.Id,
                Name = result.Title,
                ReleaseDate = result.Release_Date
            }).ToList();
        }

        public static List<Episode> ToEpisodes(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<RootObject2>(json);

            return null;
        }

        public static RootObject ToObj(string json)
        {
            return JsonConvert.DeserializeObject<RootObject>(json);
        }

        public static List<PersonDTO> ToPersons(string json)
        {
            var v = JsonConvert.DeserializeObject<RootObjectPerson>(json);

            return v.Results.Select(result => new PersonDTO
            {
                Id = result.Id, 
                Name = result.Name
            }).ToList();
        }
    }
}