using System;
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
        #region Tv shows
        public static List<ShowDTO> ToShows(string json)
        {
            var root = JsonConvert.DeserializeObject<Root>(json);

            return root.Results.Select(tvShow => new ShowDTO
            {
                Id = tvShow.Id,
                Name = tvShow.Name
            }).ToList();
        }

        public static ShowDTO ToShow(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<RootObject>(json);

            return new ShowDTO
            {
                Id = deserialized.Id,
                Name = deserialized.Name,
                Seasons = deserialized.Seasons.Select(x => new Messages.DTO.Season
                {
                    Id = x.Id,
                    Air_Date = x.Air_Date,
                    Season_Number = x.Season_Number
                }).ToList(),
                NumberOfSeasons = deserialized.Number_Of_Seasons
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
                ReleaseDate = !string.IsNullOrEmpty(rootObjectMovie.Release_Date) ? DateTime.Parse(rootObjectMovie.Release_Date) : DateTime.MinValue
            }).ToList();
        }

        public static MovieDTO ToMovie(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<Result>(json);

            return new MovieDTO
            {
                Id = deserialized.Id,
                Name = deserialized.Title,
                ReleaseDate = !string.IsNullOrEmpty(deserialized.Release_Date) ? DateTime.Parse(deserialized.Release_Date) : DateTime.MinValue
            };
        }


        public static List<MovieDTO> ToMovies(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<RootObjectMovies>(json);

            return deserialized.Results.Select(result => new MovieDTO
            {
                Id = result.Id,
                Name = result.Title,
                ReleaseDate = !string.IsNullOrEmpty(result.Release_Date) ? DateTime.Parse(result.Release_Date) : DateTime.MinValue
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