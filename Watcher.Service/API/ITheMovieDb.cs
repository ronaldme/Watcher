using System.Collections.Generic;
using Watcher.Messages.Movie;
using Watcher.Messages.Person;
using Watcher.Messages.Show;

namespace Watcher.Service.API
{
    public interface ITheMovieDb
    {
        List<ShowDto> SearchTvShows(string search);
        List<MovieDto> SearchMovies(string search);
        List<PersonDto> SearchPersons(string search);

        List<ShowDto> PopularShows();
        List<PersonDto> PopularPersons();
        List<MovieDto> PopularMovies();

        ShowDto GetShowById(int id);
        MovieDto GetMovieById(int id);
        PersonDto GetPersonById(int id);

        ShowDto GetLatestEpisode(int tvId, List<Season> seasons);
    }
}