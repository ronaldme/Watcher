using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using BLL.Json.Shows;
using Messages.DTO;
using Convert = BLL.Json.Convert;

namespace BLL
{
    public class TheMovieDb : ITheMovieDb
    {
        #region Search
        public List<ShowDTO> SearchTv(string search)
        {
            var request = (HttpWebRequest)WebRequest.Create(Urls.SearchTv + "&query=" + ReplaceSpaces(search) + "&vote_count.gte=10" + "&sort_by=popularity.desc");
            string json = GetResponse(request);

            return Convert.ToShows(json);
        }

        public List<MovieDTO> SearchMovie(string search)
        {
            var request = (HttpWebRequest)WebRequest.Create(Urls.SearchMovie + "&query=" + ReplaceSpaces(search));
            string json = GetResponse(request);

            return Convert.ToMovies(json);
        }

        public List<PersonDTO> SearchPerson(string search)
        {
            var request = (HttpWebRequest)WebRequest.Create(Urls.SearchPerson + "&query=" + ReplaceSpaces(search));
            string json = GetResponse(request);

            return Convert.ToPersons(json);
        }
        #endregion

        #region List DTO's
        public List<ShowDTO> TopRated()
        {
            var request = (HttpWebRequest)WebRequest.Create(Urls.TopRated);
            string json = GetResponse(request);

            return Convert.ToShows(json);
        }

        public List<PersonDTO> Populair()
        {
            var request = (HttpWebRequest)WebRequest.Create(Urls.PopularPersons);
            string json = GetResponse(request);

            return Convert.ToPersons(json);
        }

        public List<MovieDTO> Upcoming()
        {
            var request = (HttpWebRequest)WebRequest.Create(Urls.UpcomingMovies);
            string json = GetResponse(request);

            return Convert.toNew(json);
        }
        #endregion

        #region Get
        public ShowDTO GetShowBy(int id)
        {
            var request = (HttpWebRequest)WebRequest.Create(Urls.SearchBy(Urls.SearchTvById, id));
            string json = GetResponse(request);

            return Convert.ToShow(json);
        }

        public ShowDTO GetLatestEpisode(int tvId, List<Messages.DTO.Season> seasons)
        {
            CurrentNextSeason currentNextSeason = GetCurrentNextSeason(seasons);

            var requestCurrent = (HttpWebRequest)WebRequest.Create(Urls.SearchTvSeasons(tvId, currentNextSeason.Current.Season_Number));
            string jsonCurrent = GetResponse(requestCurrent);

            var episodes = Convert.ToSeasons(jsonCurrent).Episodes; // episodes current season

            // True: this season is finished
            if (!string.IsNullOrEmpty(episodes[episodes.Count - 1].Air_Date) && DateTime.Parse(episodes[episodes.Count - 1].Air_Date).Date < DateTime.UtcNow.Date)
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
                            return new ShowDTO
                            {
                                NextEpisode = 1,
                                CurrentSeason = nextSeason.Season_Number,
                                EpisodeCount = nextSeason.Episodes.Count,
                                ReleaseNextEpisode = DateTime.Parse(nextSeason.Episodes[0].Air_Date)
                            };
                        }
                        return new ShowDTO
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
                            return new ShowDTO
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
                    return new ShowDTO
                    {
                        CurrentSeason = currentNextSeason.Current.Season_Number, 
                        NextEpisode = 0
                    };
                }
            }

            // Current season is not yet finished
            for (int i = episodes.Count - 1; i >= 0; i--)
            {
                if (!string.IsNullOrEmpty(episodes[i].Air_Date) && DateTime.Parse(episodes[i].Air_Date).Date <= DateTime.UtcNow.Date)
                {
                    Episode nextEpisode = episodes[i];

                    return new ShowDTO
                    {
                        NextEpisode = nextEpisode.Episode_Number,
                        ReleaseNextEpisode = DateTime.Parse(nextEpisode.Air_Date),
                        CurrentSeason = nextEpisode.Season_Number,
                        EpisodeCount = episodes.Count
                    };
                }
            }

            return new ShowDTO();
        }

        public MovieDTO GetMovieBy(int id)
        {
            var request = (HttpWebRequest)WebRequest.Create(Urls.SearchBy(Urls.SearchMovieById, id));
            string json = GetResponse(request);

            return Convert.ToMovie(json);
        }

        public PersonDTO GetPersonBy(int id)
        {
            var request = (HttpWebRequest)WebRequest.Create(Urls.SearchBy(Urls.SearchPersonById, id));
            string json = GetResponse(request);

            return Convert.ToPerson(json);
        }
        #endregion

        #region Private methods
        private CurrentNextSeason GetCurrentNextSeason(List<Messages.DTO.Season> seasons)
        {
            var currentNextSeason = new CurrentNextSeason();

            for (int i = 0; i < seasons.Count; i++)
            {
                Messages.DTO.Season season = seasons[i];

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
        #endregion
    }
}