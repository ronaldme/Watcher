using System;

namespace Repository.Entities
{
    public class Actor : Entity
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}