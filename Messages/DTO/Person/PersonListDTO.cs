﻿using System.Collections.Generic;

namespace Messages.DTO
{
    public class PersonListDTO
    {
        public List<PersonDTO> Persons { get; set; }
        public string PrefixPath { get; set; }
    }
}
