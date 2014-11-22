using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using BLL.Json;
using Messages.DTO;

namespace BLL
{
    public class TheMovieDb
    {
        private readonly string apiKey = ConfigurationManager.AppSettings.Get("apiKey");

        public List<Show> SearchTv(string search)
        {
            var request = (HttpWebRequest)WebRequest.Create(Urls.searchTv + "?api_key=" + apiKey + "&query=" + ReplaceSpaces(search) + "&vote_count.gte=10" + "&sort_by=popularity.desc");
            string json = GetResponse(request);

            return Convert.ToShows(json);
        }
        
        public List<Show> GetTopRated()
        {
            var request = (HttpWebRequest)WebRequest.Create(Urls.topRated + "?api_key=" + apiKey);
            string json = GetResponse(request);

            return Convert.ToShows(json);
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

        public string searchId = "http://api.themoviedb.org/3/tv/";
        
        public Show GetBy(int id)
        {
            var request = (HttpWebRequest)WebRequest.Create(searchId + id + "?api_key=" + apiKey);
            string json = GetResponse(request);

            return Convert.ToShow(json);
        }
    }
}