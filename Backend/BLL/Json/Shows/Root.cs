﻿using System.Collections.Generic;

namespace BLL.Json.Shows
{

    public class Root
    {
        public int Page { get; set; }
        public List<Item> Results { get; set; }
        public int Total_Pages { get; set; }
        public int Total_Results { get; set; }
    }
}