using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Watcher.Common;
using Watcher.Messages.Movie;
using Watcher.Messages.Person;
using Watcher.Messages.Show;
using Watcher.Service.Json.Shows;
using Convert = Watcher.Service.Json.Convert;
using Season = Watcher.Messages.Show.Season;

namespace Watcher.Service.API
{
    public class TheMovieDb : ITheMovieDb
    {
        public List<ShowDto> SearchTvShows(string search)
        {
            var request = (HttpWebRequest)WebRequest.Create(Urls.SearchTv + "&query=" + ReplaceSpaces(search));
            string json = GetResponse(request);

            return Convert.ToShows(json);
        }

        public List<MovieDto> SearchMovie(string search)
        {
            var request = (HttpWebRequest)WebRequest.Create(Urls.SearchMovie + "&query=" + ReplaceSpaces(search));
            string json = GetResponse(request);

            return Convert.ToMovies(json);
        }

        public List<PersonDto> SearchPerson(string search)
        {
            var request = (HttpWebRequest)WebRequest.Create(Urls.SearchPerson + "&query=" + ReplaceSpaces(search));
            string json = GetResponse(request);

            return Convert.ToPersons(json);
        }

        public List<ShowDto> TopRated()
        {
            var request = (HttpWebRequest)WebRequest.Create(Urls.TopRated);
            string json = GetResponse(request);

            return Convert.ToShows(json);
        }

        public List<PersonDto> Populair()
        {
            var request = (HttpWebRequest)WebRequest.Create(Urls.PopularPersons);
            string json = GetResponse(request);

            return Convert.ToPersons(json);
        }

        public List<MovieDto> Upcoming()
        {
            var request = (HttpWebRequest)WebRequest.Create(Urls.UpcomingMovies);
            string json = GetResponse(request);

            return Convert.ToUpcoming(json);
        }

        public ShowDto GetShowBy(int id)
        {
            var request = (HttpWebRequest)WebRequest.Create(Urls.SearchBy(Urls.SearchTvById, id));
            string json = GetResponse(request);

            return Convert.ToShow(json);
        }

        public ShowDto GetLatestEpisode(int tvId, List<Season> seasons)
        {
            CurrentNextSeason currentNextSeason = GetCurrentNextSeason(seasons);

            if(currentNextSeason.Current == null) return new ShowDto();

            var requestCurrent = (HttpWebRequest)WebRequest.Create(Urls.SearchTvSeasons(tvId, currentNextSeason.Current.Season_Number));
            string jsonCurrent = GetResponse(requestCurrent);

            var episodes = Convert.ToSeasons(jsonCurrent).Episodes; // episodes current season

            // True: this season is finished
            if (episodes.Count > 0 && !string.IsNullOrEmpty(episodes[episodes.Count - 1].Air_Date) && DateTime.Parse(episodes[episodes.Count - 1].Air_Date).Date < DateTime.UtcNow.Date)
            {
                if (currentNextSeason.Next != null)
                {
                    var requestNext = (HttpWebRequest)WebRequest.Create(Urls.SearchTvSeasons(tvId, currentNextSeason.Next.Season_Number));
                    string jsonNext = GetResponse(requestNext);

                    SeasonRootObject nextSeason = Convert.ToSeasons(jsonNext);

                    if (DateTime.Parse(nextSeason.Air_Date) > DateTime.UtcNow) // season not yet released
                    {
                        if (!string.IsNullOrEmpty(nextSeason.Episodes[0].Air_Date))
                        {
                            return new ShowDto
                            {
                                NextEpisode = 1,
                                CurrentSeason = nextSeason.Season_Number,
                                EpisodeCount = nextSeason.Episodes.Count,
                                ReleaseNextEpisode = DateTime.Parse(nextSeason.Episodes[0].Air_Date)
                            };
                        }
                        return new ShowDto
                        {
                            NextEpisode = 1,
                            CurrentSeason = nextSeason.Season_Number,
                            EpisodeCount = nextSeason.Episodes.Count,
                            ReleaseNextEpisode = null
                        };
                    }

                    foreach (Episode episode in nextSeason.Episodes)
                    {
                        if (!string.IsNullOrEmpty(episode.Air_Date) && DateTime.Parse(episode.Air_Date).Date >= DateTime.UtcNow.Date)
                        {
                            return new ShowDto
                            {
                                NextEpisode = episode.Episode_Number,
                                CurrentSeason = nextSeason.Season_Number,
                                ReleaseNextEpisode = DateTime.Parse(episode.Air_Date),
                                EpisodeCount = nextSeason.Episodes.Count
                            };
                        }
                    }
                }
                else
                {
                    return new ShowDto
                    {
                        CurrentSeason = currentNextSeason.Current.Season_Number, 
                        NextEpisode = 0
                    };
                }
            }

            // Current season is not yet finished
            for (int i = episodes.Count - 1; i >= 0; i--)
            {
                if (!string.IsNullOrEmpty(episodes[i].Air_Date) && DateTime.Parse(episodes[i].Air_Date).Date < DateTime.UtcNow.Date)
                {
                    var episodeNumber = i == episodes.Count ? episodes.Count : i + 1;

                    Episode nextEpisode = episodes[episodeNumber];

                    return new ShowDto
                    {
                        NextEpisode = nextEpisode.Episode_Number,
                        ReleaseNextEpisode = DateTime.Parse(nextEpisode.Air_Date),
                        CurrentSeason = nextEpisode.Season_Number,
                        EpisodeCount = episodes.Count
                    };
                }
            }

            return new ShowDto();
        }

        public MovieDto GetMovieBy(int id)
        {
            var request = (HttpWebRequest)WebRequest.Create(Urls.SearchBy(Urls.SearchMovieById, id));
            string json = GetResponse(request);

            return Convert.ToMovie(json);
        }

        public PersonDto GetPersonBy(int id)
        {
            var request = (HttpWebRequest)WebRequest.Create(Urls.SearchBy(Urls.SearchPersonById, id));
            string json = GetResponse(request);
            var personDto = Convert.ToPerson(json);

            var requestInfo = (HttpWebRequest)WebRequest.Create(Urls.PersonCredits(id));
            string jsonInfo = GetResponse(requestInfo);

            var personInfo = Convert.ToPersonInfo(jsonInfo).OrderByDescending(x => x.ReleaseDate).ToList();

            for (int i = 0; i < personInfo.Count; i++)
            {
                var dto = personInfo[i];

                if (dto.ReleaseDate.HasValue && dto.ReleaseDate.Value.Date < DateTime.UtcNow.Date)
                {
                    var number = i == personInfo.Count ? i : (i - 1 < 0 ? 0 : i - 1);

                    personDto.ProductionName = personInfo[number].ProductionName;
                    personDto.ReleaseDate = personInfo[number].ReleaseDate;
                    break;
                }
            }

            return personDto;
        }

        private CurrentNextSeason GetCurrentNextSeason(List<Season> seasons)
        {
            var currentNextSeason = new CurrentNextSeason();

            for (int i = 0; i < seasons.Count; i++)
            {
                Season season = seasons[i];

                if (!string.IsNullOrEmpty(season.Air_Date) &&
                    DateTime.Parse(season.Air_Date) > DateTime.UtcNow)
                {
                    currentNextSeason.Next = season;

                    if (i > 1)
                    {
                        // Current season could be ended but we need to check that.
                        currentNextSeason.Current = seasons[i - 1];
                    }
                }
            }

            if (currentNextSeason.Current == null)
            {
                currentNextSeason.Current = seasons.FindLast(x => !string.IsNullOrEmpty(x.Air_Date));
            }

            return currentNextSeason;
        }

        private string GetResponse(HttpWebRequest request)
        {
            request.KeepAlive = true;
            request.Method = "GET";
            request.Accept = "application/json";
            request.ContentLength = 0;

            string content;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    content = reader.ReadToEnd();
                }
            }

            return content;
        }

        private string ReplaceSpaces(string input)
        {
            return input.Contains(" ") ? input.Replace(' ', '+') : input;
        }
    }
}