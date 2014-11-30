using System;
using System.Collections.Generic;
using BLL.Json.Shows;

namespace BLL
{
    public class Testing
    {
        public int Id { get; set; }
        public DateTime AirDate { get; set; }
        public string Name { get; set; }
        public int Number_Of_Seasons { get; set; }
        public List<Season> Seasons { get; set; }
    }
}
