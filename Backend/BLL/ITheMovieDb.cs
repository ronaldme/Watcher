using System.Collections.Generic;
using Messages.DTO;

namespace BLL
{
    public interface ITheMovieDb
    {
        List<TvShowDTO> SearchTv(string search);
        List<MovieDTO> SearchMovie(string search);
        List<PersonDTO> SearchPerson(string search);

        List<TvShowDTO> TopRated();
        List<MovieDTO> Upcoming();
        List<PersonDTO> Populair();
        
        TvShowDTO GetShowBy(int id);
        MovieDTO GetMovieBy(int id);
        PersonDTO GetPersonBy(int id);
    }
}