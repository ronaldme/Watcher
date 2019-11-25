using System;
using System.Configuration;
using System.Linq;
using System.Timers;
using log4net;
using Watcher.DAL;
using Watcher.DAL.Entities;
using Watcher.Messages.Movie;
using Watcher.Messages.Person;
using Watcher.Messages.Show;
using Watcher.Service.TheMovieDb;
using Convert = System.Convert;

namespace Watcher.Service.Services
{
    public class UpdateService : IUpdateService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UpdateService));

        private readonly ITheMovieDb theMovieDb;
        private readonly Timer movieInterval;
        private readonly Timer showInterval;
        private readonly Timer personInterval;

        public UpdateService(ITheMovieDb theMovieDb)
        {
            this.theMovieDb = theMovieDb;
            movieInterval = new Timer(Convert.ToInt32(ConfigurationManager.AppSettings["movieInterval"]));
            showInterval = new Timer(Convert.ToInt32(ConfigurationManager.AppSettings["showInterval"]));
            personInterval = new Timer(Convert.ToInt32(ConfigurationManager.AppSettings["personInterval"]));
        }

        public void Start()
        {
            UpdateMovies();
            UpdateShows();
            UpdatePersons();
        }

        public void Stop()
        {
            movieInterval.Enabled = false;
            showInterval.Enabled = false;
            personInterval.Enabled = false;
        }

        private void UpdateMovies()
        {
            movieInterval.Elapsed += UpdateMoviesNow;
            movieInterval.Enabled = true;
        }

        private void UpdateMoviesNow(object sender, ElapsedEventArgs args)
        {
            using (var context = new WatcherDbContext())
            {
                var movies = context.Movies.ToList();

                foreach (Movie movie in movies)
                {
                    MovieDto movieInfo = theMovieDb.GetMovieBy(movie.TheMovieDbId);

                    if (movie.ReleaseDate.HasValue)
                    {
                        if (movie.ReleaseDate.Value != movieInfo.ReleaseDate)
                        {
                            movie.ReleaseDate = movieInfo.ReleaseDate;
                            movie.Name = movieInfo.Name;
                            log.Debug($"Updating {movie.Name}, new values: {movie.ReleaseDate}, {movie.Name}");
                        }
                    }
                    else
                    {
                        if (movieInfo.ReleaseDate.HasValue)
                        {
                            movie.ReleaseDate = movieInfo.ReleaseDate;
                            movie.Name = movieInfo.Name;
                            log.Debug($"Updating {movie.Name}, new values: {movie.ReleaseDate}, {movie.Name}");
                        }
                    }
                }

                context.SaveChanges();
            }
        }
        
        private void UpdateShows()
        {
            showInterval.Elapsed += UpdateShowsNow;
            showInterval.Enabled = true;
        }

        private void UpdateShowsNow(object sender, ElapsedEventArgs args)
        {
            log.Info("Updating shows");
            using (var context = new WatcherDbContext())
            {
                var shows = context.Shows.ToList();

                foreach (Show show in shows)
                {
                    ShowDto showInfo = theMovieDb.GetShowBy(show.TheMovieDbId);
                    ShowDto showDto = theMovieDb.GetLatestEpisode(showInfo.Id, showInfo.Seasons);

                    if (showDto.ReleaseNextEpisode.HasValue)
                    {
                        if (show.NextEpisode == showDto.NextEpisode && show.ReleaseNextEpisode != showDto.ReleaseNextEpisode)
                        {
                            show.ReleaseNextEpisode = showDto.ReleaseNextEpisode.Value;
                            show.CurrentSeason = showDto.CurrentSeason;
                            show.EpisodeCount = showDto.EpisodeCount;
                            show.NextEpisode = showDto.NextEpisode;
                            show.Name = showDto.Name;
                            log.Debug($"Updating {show.Name}, new values: next episode {show.NextEpisode}, season: {show.CurrentSeason} release date: {show.ReleaseNextEpisode}");
                        }

                        // if release next episode is two days old we can update it
                        else if (DateTime.UtcNow.AddDays(2) > show.ReleaseNextEpisode && showDto.ReleaseNextEpisode.Value > show.ReleaseNextEpisode)
                        {
                            show.ReleaseNextEpisode = showDto.ReleaseNextEpisode.Value;
                            show.CurrentSeason = showDto.CurrentSeason;
                            show.EpisodeCount = showDto.EpisodeCount;
                            show.NextEpisode = showDto.NextEpisode;
                            show.Name = showDto.Name;
                            log.Debug($"Updating {show.Name}, new values: next episode {show.NextEpisode}, season: {show.CurrentSeason} release date: {show.ReleaseNextEpisode}");
                        }
                    }
                }

                context.SaveChanges();
            }
        }

        private void UpdatePersons()
        {
            personInterval.Elapsed += UpdatePersonsNow;
            personInterval.Enabled = true;
        }

        private void UpdatePersonsNow(object sender, ElapsedEventArgs e)
        {
            using (var context = new WatcherDbContext())
            {
                var persons = context.Persons.ToList();

                foreach (Person person in persons)
                {
                    PersonDto personInfo = theMovieDb.GetPersonBy(person.TheMovieDbId);

                    if (person.ReleaseDate.HasValue && personInfo.ReleaseDate.HasValue)
                    {
                        if (personInfo.ReleaseDate != person.ReleaseDate)
                        {
                            // Update to a new release date with possibly a new production name.
                            person.ReleaseDate = personInfo.ReleaseDate;
                            person.ProductionName = personInfo.ProductionName;
                            person.Name = personInfo.Name;

                            log.Debug($"Updating {person.Name}, new values: production name {person.ProductionName}, release date: {person.ReleaseDate}");
                        }
                    }
                    else if (personInfo.ReleaseDate.HasValue)
                    {
                        person.ReleaseDate = personInfo.ReleaseDate;
                        person.ProductionName = personInfo.ProductionName;
                        person.Name = personInfo.Name;

                        log.Debug($"Updating {person.Name}, new values: production name {person.ProductionName}, release date: {person.ReleaseDate}");
                    }
                }

                context.SaveChanges();
            }
        }
    }
}