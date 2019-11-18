using System.Collections.Generic;

namespace Watcher.Backend.Domain.Json.Persons
{
    public class RootObjectInfo
    {
        public int Id { get; set; }
        public List<Cast> Cast { get; set; }
    }
}