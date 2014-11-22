﻿using System.Collections.Generic;
using System.Linq;
using Messages.DTO;
using Newtonsoft.Json;

namespace BLL.Json
{
    public class Convert
    {
        public static List<TvShowDTO> ToShows(string json)
        {
            var v = JsonConvert.DeserializeObject<Root>(json);
            
            return Map(v);
        }

        public static TvShowDTO ToShow(string json)
        {
            var v = JsonConvert.DeserializeObject<Item>(json);

            return new TvShowDTO
            {
                Id = v.id,
                Name = v.name,
                OriginalName = v.original_name,
                AirDate = v.first_air_date,
                VoteAverage = v.vote_average,
                VoteCount = v.vote_count
            };
        }

        private static List<TvShowDTO> Map(Root rootObject2)
        {
            // ToDO: Use automapper
            return rootObject2.results.Select(result2 => new TvShowDTO
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
    }
}