using System.Collections.Generic;
using Watcher.Messages.Movie;
using Watcher.Messages.Person;
using Watcher.Messages.Show;

namespace Watcher.Service.TheMovieDb
{
    public interface ITheMovieDb
    {
        List<ShowDto> SearchTvShows(string search);
        List<MovieDto> SearchMovie(string search);
        List<PersonDto> SearchPerson(string search);

        List<ShowDto> TopRated();
        List<MovieDto> Upcoming();
        List<PersonDto> Populair();

        ShowDto GetShowBy(int id);
        MovieDto GetMovieBy(int id);
        PersonDto GetPersonBy(int id);

        ShowDto GetLatestEpisode(int tvId, List<Season> seasons);
    }
}