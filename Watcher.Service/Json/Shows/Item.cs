using System.Collections.Generic;

namespace Watcher.Service.Json.Shows
{
    public class Item
    {
        public string Backdrop_Path { get; set; }
        public int Id { get; set; }
        public string Original_Name { get; set; }
        public string First_Air_Date { get; set; }
        public List<object> Origin_Country { get; set; }
        public string Poster_Path { get; set; }
        public double Popularity { get; set; }
        public string Name { get; set; }
        public double Vote_Average { get; set; }
        public int Vote_Count { get; set; }
        public string Overview { get; set; }
    }
}