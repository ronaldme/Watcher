using System.Configuration;
using System.Timers;
using BLL;
using log4net;
using Messages.DTO;
using Repository;
using Repository.Entities;
using Repository.Repositories.Interfaces;
using Repository.UOW;
using Services.Interfaces;
using Convert = System.Convert;

namespace Services
{
    public class UpdateService : IUpdateService, IStartable
    {
        private readonly ITheMovieDb theMovieDb;
        private readonly IMovieRepository movieRepository;
        private readonly IShowRepository showRepository;
        private readonly IPersonRepository personRepository;

        private Timer movieInterval;
        private Timer showInterval;
        private Timer personInterval;
        private readonly int intervalMovie;
        private readonly int intervalShow;
        private readonly int intervalPerson;
        private static readonly ILog log = LogManager.GetLogger(typeof(UpdateService));

        public UpdateService(ITheMovieDb theMovieDb, IMovieRepository movieRepository, IShowRepository showRepository, IPersonRepository personRepository)
        {
            this.theMovieDb = theMovieDb;
            this.movieRepository = movieRepository;
            this.showRepository = showRepository;
            this.personRepository = personRepository;

            intervalMovie = Convert.ToInt32(ConfigurationManager.AppSettings.Get("movieInterval"));
            intervalShow = Convert.ToInt32(ConfigurationManager.AppSettings.Get("showInterval"));
            intervalPerson = Convert.ToInt32(ConfigurationManager.AppSettings.Get("personInterval"));
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

        public void UpdateMovies()
        {
            movieInterval = new Timer(intervalMovie);
            movieInterval.Elapsed += UpdateMoviesNow;
            movieInterval.Enabled = true;
        }

        private void UpdateMoviesNow(object sender, ElapsedEventArgs args)
        {
            log.Info("Updating movies");
            using (var watcherContext = new WatcherContext())
            {
                UnitOfWork.Current = new UnitOfWork(watcherContext);
                UnitOfWork.Current.BeginTransaction();
                try
                {
                    var movies = movieRepository.All();

                    foreach (Movie movie in movies)
                    {
                        MovieDTO movieInfo = theMovieDb.GetMovieBy(movie.TheMovieDbId);

                        if (movie.ReleaseDate.HasValue)
                        {
                            if (movie.ReleaseDate.Value != movieInfo.ReleaseDate)
                            {
                                movie.ReleaseDate = movieInfo.ReleaseDate;
                                movie.Name = movieInfo.Name;
                            }
                        }
                        else
                        {
                            if (movieInfo.ReleaseDate.HasValue)
                            {
                                movie.ReleaseDate = movieInfo.ReleaseDate;
                                movie.Name = movieInfo.Name;
                            }
                        }
                    }
                }
                finally
                {
                    log.Info("Movies updated");
                    UnitOfWork.Current.Commit();
                }
            }
        }
        
        public void UpdateShows()
        {
            showInterval = new Timer(intervalShow);
            showInterval.Elapsed += UpdateShowsNow;
            showInterval.Enabled = true;
        }

        private void UpdateShowsNow(object sender, ElapsedEventArgs args)
        {
            log.Info("Updating shows");
            using (var watcherContext = new WatcherContext())
            {
                UnitOfWork.Current = new UnitOfWork(watcherContext);
                UnitOfWork.Current.BeginTransaction();
                try
                {
                    var shows = showRepository.All();

                    foreach (Show show in shows)
                    {
                        ShowDTO showInfo = theMovieDb.GetShowBy(show.TheMovieDbId);
                        ShowDTO showDto = theMovieDb.GetLatestEpisode(showInfo.Id, showInfo.Seasons);

                        if (showDto.ReleaseNextEpisode.HasValue)
                        {
                            if (show.NextEpisode == showDto.NextEpisode && show.ReleaseNextEpisode != showDto.ReleaseNextEpisode)
                            {
                                show.ReleaseNextEpisode = showDto.ReleaseNextEpisode.Value;
                                show.CurrentSeason = showDto.CurrentSeason;
                                show.EpisodeCount = showDto.EpisodeCount;
                                show.NextEpisode = showDto.NextEpisode;
                            }
                            // if release next episode is two days old we can update it
                            else if (DateTime.UtcNow.AddDays(2) > show.ReleaseNextEpisode && showDto.ReleaseNextEpisode.Value > show.ReleaseNextEpisode)
                            {
                                show.ReleaseNextEpisode = showDto.ReleaseNextEpisode.Value;
                                show.CurrentSeason = showDto.CurrentSeason;
                                show.EpisodeCount = showDto.EpisodeCount;
                                show.NextEpisode = showDto.NextEpisode;
                            }
                        }
                    }
                }
                finally
                {
                    log.Info("Shows updated");
                    UnitOfWork.Current.Commit();
                }
            }
        }

        private void UpdatePersons()
        {
            personInterval = new Timer(intervalPerson);
            personInterval.Elapsed += UpdatePersonsNow;
            personInterval.Enabled = true;
        }

        private void UpdatePersonsNow(object sender, ElapsedEventArgs e)
        {
            log.Info("Updating persons");
            using (var watcherContext = new WatcherContext())
            {
                UnitOfWork.Current = new UnitOfWork(watcherContext);
                UnitOfWork.Current.BeginTransaction();
                try
                {
                    var persons = personRepository.All();

                    foreach (Person person in persons)
                    {
                        PersonDTO personInfo = theMovieDb.GetPersonBy(person.TheMovieDbId);

                        if (person.ReleaseDate.HasValue && personInfo.ReleaseDate.HasValue)
                        {
                            if (personInfo.ReleaseDate != person.ReleaseDate)
                            {
                                // Update to a new release date with possibly a new production name.
                                person.ReleaseDate = personInfo.ReleaseDate;
                                person.ProductionName = personInfo.ProductionName;
                            }
                        }
                        else if (personInfo.ReleaseDate.HasValue)
                        {
                            person.ReleaseDate = personInfo.ReleaseDate;
                            person.ProductionName = personInfo.ProductionName;
                        }
                    }
                }
                finally
                {
                    log.Info("Persons updated");
                    UnitOfWork.Current.Commit();
                }
            }
        }
    }
}