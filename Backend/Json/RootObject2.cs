using System.Collections.Generic;

namespace Backend.Json
{

    public class RootObject2
    {
        public int page { get; set; }
        public List<Result2> results { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
    }
}
