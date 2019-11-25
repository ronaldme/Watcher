using System.Collections.Generic;

namespace Watcher.Service.Json.Persons
{
    public class PersonObject
    {
        public bool Adult { get; set; }
        public List<object> Also_Known_As { get; set; }
        public string Biography { get; set; }
        public string Birthday { get; set; }
        public string Deathday { get; set; }
        public string Homepage { get; set; }
        public int Id { get; set; }
        public string Imdb_Id { get; set; }
        public string Name { get; set; }
        public string Place_Of_Birth { get; set; }
        public double Popularity { get; set; }
        public string Profile_Path { get; set; }
    }
}