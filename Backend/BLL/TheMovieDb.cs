using System.Collections.Generic;
using System.IO;
using System.Net;
using BLL.Json;
using Messages.DTO;

namespace BLL
{
    public class TheMovieDb : ITheMovieDb
    {
        #region Search
        public List<TvShowDTO> SearchTv(string search)
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
        public List<TvShowDTO> TopRated()
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
        public TvShowDTO GetBy(int id)
        {
            var request = (HttpWebRequest)WebRequest.Create(Urls.SearchTvById + "/" + id);         
            string json = GetResponse(request);

            return Convert.ToShow(json);
        }

        public TvShowDTO GetShowBy(int id)
        {
            throw new System.NotImplementedException();
        }

        public MovieDTO GetMovieBy(int id)
        {
            throw new System.NotImplementedException();
        }

        public PersonDTO GetPersonBy(int id)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Private methods
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