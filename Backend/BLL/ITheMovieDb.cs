using System.Collections.Generic;
using BLL.Json.Shows;
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

        Testing GetShowBy(int id);
        MovieDTO GetMovieBy(int id);
        PersonDTO GetPersonBy(int id);

        TvShowDTO GetLatestEpisode(int tvId, List<Season> seasons);
    }
}