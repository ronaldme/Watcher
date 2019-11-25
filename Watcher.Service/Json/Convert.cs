using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Watcher.Messages.Movie;
using Watcher.Messages.Person;
using Watcher.Messages.Show;
using Watcher.Service.Json.Movies;
using Watcher.Service.Json.Persons;
using Watcher.Service.Json.Shows;

namespace Watcher.Service.Json
{
    public class Convert
    {
        public static List<ShowDto> ToShows(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<Root>(json);

            return deserialized.Results.Select(tvShow => new ShowDto
            {
                Id = tvShow.Id,
                Name = tvShow.Name,
                PosterPath = tvShow.Poster_Path
            }).ToList();
        }

        public static ShowDto ToShow(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<RootObject>(json);

            return new ShowDto
            {
                Id = deserialized.Id,
                Name = deserialized.Name,
                Seasons = deserialized.Seasons.Select(x => new Season
                {
                    Id = x.Id,
                    Air_Date = x.Air_Date,
                    Season_Number = x.Season_Number
                }).ToList(),
                NumberOfSeasons = deserialized.Number_Of_Seasons,
                PosterPath = deserialized.Poster_Path
            };
        }

        public static List<MovieDto> ToUpcoming(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<RootObjectMovies>(json);

            return deserialized.Results.Select(rootObjectMovie => new MovieDto
            {
                Id = rootObjectMovie.Id,
                Name = rootObjectMovie.Title,
                ReleaseDate = !string.IsNullOrEmpty(rootObjectMovie.Release_Date) ? DateTime.Parse(rootObjectMovie.Release_Date) : DateTime.MinValue,
                PosterPath = rootObjectMovie.Poster_Path
            }).ToList();
        }

        public static MovieDto ToMovie(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<MovieObject>(json);

            return new MovieDto
            {
                Id = deserialized.Id,
                Name = deserialized.Title,
                ReleaseDate = !string.IsNullOrEmpty(deserialized.Release_Date) ? DateTime.Parse(deserialized.Release_Date) : (DateTime?)null,
                PosterPath = deserialized.Poster_Path
            };
        }


        public static List<MovieDto> ToMovies(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<RootObjectMovies>(json);

            return deserialized.Results.Select(result => new MovieDto
            {
                Id = result.Id,
                Name = result.Title,
                ReleaseDate = !string.IsNullOrEmpty(result.Release_Date) ? DateTime.Parse(result.Release_Date) : DateTime.MinValue,
                PosterPath = result.Poster_Path
            }).ToList();
        }

        public static List<PersonDto> ToPersons(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<RootObjectPerson>(json);

            return deserialized.Results.Select(result => new PersonDto
            {
                Id = result.Id,
                Name = result.Name,
                ProfilePath = result.Profile_Path
            }).ToList();
        }

        public static PersonDto ToPerson(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<PersonObject>(json);

            return new PersonDto
            {
                Id = deserialized.Id,
                Name = deserialized.Name,
                Birthday = deserialized.Birthday,
                ProfilePath = deserialized.Profile_Path
            };
        }

        public static List<PersonDto> ToPersonInfo(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<RootObjectInfo>(json);

            return deserialized.Cast.Select(x => new PersonDto
            {
                ReleaseDate = !string.IsNullOrEmpty(x.release_date) ? DateTime.Parse(x.release_date) : (DateTime?)null,
                ProductionName = x.original_title
            }).ToList();
        }

        public static SeasonRootObject ToSeasons(string json)
        {
            return JsonConvert.DeserializeObject<SeasonRootObject>(json);
        }
    }
}