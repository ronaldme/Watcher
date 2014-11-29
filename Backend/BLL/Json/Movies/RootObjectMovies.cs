using System.Collections.Generic;

namespace BLL.Json.Movies
{
    public class RootObjectMovies
    {
        public Dates Dates { get; set; }
        public int Page { get; set; }
        public List<Result> Results { get; set; }
        public int Total_Pages { get; set; }
        public int Total_Results { get; set; }
    }
}
