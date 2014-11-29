using System.Collections.Generic;

namespace BLL.Json.Persons
{
    public class ResultAct
    {
        public bool Adult { get; set; }
        public int Id { get; set; }
        public List<KnownFor> Known_For { get; set; }
        public string Name { get; set; }
        public double Popularity { get; set; }
        public string Profile_Path { get; set; }
    }
}