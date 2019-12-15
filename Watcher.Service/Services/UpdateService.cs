using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Watcher.DAL;
using Watcher.Service.API;

namespace Watcher.Service.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly ITheMovieDb _theMovieDb;
        private readonly ILogger<UpdateService> _logger;

        public UpdateService(ITheMovieDb theMovieDb,
            ILogger<UpdateService> logger)
        {
            _theMovieDb = theMovieDb;
            _logger = logger;
        }

        public async Task UpdateMovies()
        {
            _logger.LogDebug("Start updating movies");

            await using var db = new WatcherDbContext();
            var movies = await db.Movies.ToListAsync();

            foreach (var movie in movies)
            {
                var movieInfo = _theMovieDb.GetMovieById(movie.TheMovieDbId);

                if (movie.ReleaseDate.HasValue)
                {
                    if (movie.ReleaseDate.Value != movieInfo.ReleaseDate)
                    {
                        movie.ReleaseDate = movieInfo.ReleaseDate;
                        movie.Name = movieInfo.Name;
                        _logger.LogDebug($"Updating {movie.Name}, new values: {movie.ReleaseDate}, {movie.Name}");
                    }
                }
                else
                {
                    if (movieInfo.ReleaseDate.HasValue)
                    {
                        movie.ReleaseDate = movieInfo.ReleaseDate;
                        movie.Name = movieInfo.Name;
                        _logger.LogDebug($"Updating {movie.Name}, new values: {movie.ReleaseDate}, {movie.Name}");
                    }
                }
            }

            await db.SaveChangesAsync();
        }

        public async Task UpdateShows()
        {
            _logger.LogDebug("Start updating shows");

            await using var db = new WatcherDbContext();
            var shows = await db.Shows.ToListAsync();

            foreach (var show in shows)
            {
                var showInfo = _theMovieDb.GetShowById(show.TheMovieDbId);
                var showDto = _theMovieDb.GetLatestEpisode(showInfo.Id, showInfo.Seasons);

                if (showDto.ReleaseNextEpisode.HasValue)
                {
                    if (show.NextEpisode == showDto.NextEpisode &&
                        show.ReleaseNextEpisode != showDto.ReleaseNextEpisode)
                    {
                        show.ReleaseNextEpisode = showDto.ReleaseNextEpisode.Value;
                        show.CurrentSeason = showDto.CurrentSeason;
                        show.EpisodeCount = showDto.EpisodeCount;
                        show.NextEpisode = showDto.NextEpisode;
                        show.Name = showDto.Name;
                        _logger.LogDebug(
                            $"Updating {show.Name}, new values: next episode {show.NextEpisode}, season: {show.CurrentSeason} release date: {show.ReleaseNextEpisode}");
                    }

                    // if release next episode is two days old we can update it
                    else if (DateTime.UtcNow.AddDays(2) > show.ReleaseNextEpisode &&
                             showDto.ReleaseNextEpisode.Value > show.ReleaseNextEpisode)
                    {
                        show.ReleaseNextEpisode = showDto.ReleaseNextEpisode.Value;
                        show.CurrentSeason = showDto.CurrentSeason;
                        show.EpisodeCount = showDto.EpisodeCount;
                        show.NextEpisode = showDto.NextEpisode;
                        show.Name = showDto.Name;
                        _logger.LogDebug(
                            $"Updating {show.Name}, new values: next episode {show.NextEpisode}, season: {show.CurrentSeason} release date: {show.ReleaseNextEpisode}");
                    }
                }
            }

            await db.SaveChangesAsync();
        }
        public async Task UpdatePersons()
        {
            _logger.LogDebug("Start updating persons");

            await using var db = new WatcherDbContext();
            var persons = await db.Persons.ToListAsync();

            foreach (var person in persons)
            {
                var personInfo = _theMovieDb.GetPersonById(person.TheMovieDbId);

                if (person.ReleaseDate.HasValue && personInfo.ReleaseDate.HasValue)
                {
                    if (personInfo.ReleaseDate != person.ReleaseDate)
                    {
                        // Update to a new release date with possibly a new production name.
                        person.ReleaseDate = personInfo.ReleaseDate;
                        person.ProductionName = personInfo.ProductionName;
                        person.Name = personInfo.Name;

                        _logger.LogDebug($"Updating {person.Name}, new values: production name {person.ProductionName}, release date: {person.ReleaseDate}");
                    }
                }
                else if (personInfo.ReleaseDate.HasValue)
                {
                    person.ReleaseDate = personInfo.ReleaseDate;
                    person.ProductionName = personInfo.ProductionName;
                    person.Name = personInfo.Name;

                    _logger.LogDebug($"Updating {person.Name}, new values: production name {person.ProductionName}, release date: {person.ReleaseDate}");
                }
            }

            await db.SaveChangesAsync();
        }
    }
}