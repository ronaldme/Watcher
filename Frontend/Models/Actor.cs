using System.Collections.Generic;

namespace Models
{
    public class Actor
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public List<Movie> PlayedIn { get; set; }
    }
}
