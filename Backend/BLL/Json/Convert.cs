using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Json.Movies;
using BLL.Json.Persons;
using BLL.Json.Shows;
using Messages.DTO;
using Messages.Request;
using Newtonsoft.Json;

namespace BLL.Json
{
    public class Convert
    {
        #region Tv shows
        public static List<TvShowDTO> ToShows(string json)
        {
            var root = JsonConvert.DeserializeObject<Root>(json);

            return root.Results.Select(tvShow => new TvShowDTO
            {
                Id = tvShow.Id,
                Name = tvShow.Name,
                AirDate = !string.IsNullOrEmpty(tvShow.First_Air_Date) ? DateTime.Parse(tvShow.First_Air_Date) : (DateTime?) null
            }).ToList();
        }

        public static Testing ToShow(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<RootObject>(json);

            return new Testing
            {
                Id = deserialized.Id,
                Name = deserialized.Name,
                AirDate = DateTime.Parse(deserialized.First_Air_Date),
                Seasons = deserialized.Seasons,
                Number_Of_Seasons = deserialized.Number_Of_Seasons
            };
        }

        #endregion

        #region Movies
        public static List<MovieDTO> toNew(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<RootObjectMovies>(json);

            return deserialized.Results.Select(rootObjectMovie => new MovieDTO
            {
                Id = rootObjectMovie.Id,
                Name = rootObjectMovie.Title,
                ReleaseDate = rootObjectMovie.Release_Date
            }).ToList();
        }

        public static MovieDTO ToMovie(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<Result>(json);

            return new MovieDTO
            {
                Id = deserialized.Id,
                Name = deserialized.Title,
                ReleaseDate = deserialized.Release_Date
            };
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
        #endregion

        #region Persons
        public static List<PersonDTO> ToPersons(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<RootObjectPerson>(json);

            return deserialized.Results.Select(result => new PersonDTO
            {
                Id = result.Id,
                Name = result.Name
            }).ToList();
        }

        public static PersonDTO ToPerson(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<PersonObject>(json);

            return new PersonDTO
            {
                Id = deserialized.Id,
                Name = deserialized.Name,
                Birthday = deserialized.Birthday
            };
        }
        #endregion

        public static SeasonRootObject ToSeasons(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<SeasonRootObject>(json);

            return deserialized;
        }
    }
}