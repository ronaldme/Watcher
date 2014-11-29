using System.Collections.Generic;

namespace BLL.Json.Persons
{
    public class RootObjectPerson
    {
        public int Page { get; set; }
        public List<ResultAct> Results { get; set; }
        public int Total_Pages { get; set; }
        public int Total_Results { get; set; }
    }
}
