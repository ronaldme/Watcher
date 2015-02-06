using System.Collections.Generic;

namespace Messages.DTO
{
    public class MovieListDTO
    {
        public List<MovieDTO> Movies { get; set; }
        public string PrefixPath { get; set; }
    }
}
