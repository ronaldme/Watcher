using System.Collections.Generic;

namespace BLL.Json.Shows
{
    public class SeasonRootObject
    {
        public string Air_Date { get; set; }
        public List<Episode> Episodes { get; set; }
        public string Name { get; set; }
        public string Overview { get; set; }
        public int Id { get; set; }
        public string Poster_Path { get; set; }
        public int Season_Number { get; set; }
    }
}