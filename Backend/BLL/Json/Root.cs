using System.Collections.Generic;

namespace BLL.Json
{

    public class Root
    {
        public int page { get; set; }
        public List<Item> results { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
    }
}