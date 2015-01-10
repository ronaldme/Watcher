using System.Collections.Generic;
using Messages.DTO;

namespace BLL
{
    public interface ITheMovieDb
    {
        List<ShowDTO> SearchTv(string search);
        List<MovieDTO> SearchMovie(string search);
        List<PersonDTO> SearchPerson(string search);

        List<ShowDTO> TopRated();
        List<MovieDTO> Upcoming();
        List<PersonDTO> Populair();

        ShowDTO GetShowBy(int id);
        MovieDTO GetMovieBy(int id);
        PersonDTO GetPersonBy(int id);

        ShowDTO GetLatestEpisode(int tvId, List<Messages.DTO.Season> seasons);
    }
}