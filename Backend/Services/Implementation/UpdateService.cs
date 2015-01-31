using System;
using System.Configuration;
using System.Timers;
using BLL;
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

        private readonly int intervalMovie;
        private readonly int intervalShow;

        private Timer movieInterval;
        private Timer showInterval;

        public UpdateService(ITheMovieDb theMovieDb, IMovieRepository movieRepository, IShowRepository showRepository)
        {
            this.theMovieDb = theMovieDb;
            this.movieRepository = movieRepository;
            this.showRepository = showRepository;

            intervalMovie = Convert.ToInt32(ConfigurationManager.AppSettings.Get("movieIntervalHours"));
            intervalShow = Convert.ToInt32(ConfigurationManager.AppSettings.Get("showIntervalHours"));
        }

        public void Start()
        {
            UpdateMovies();
            UpdateEpisodes();
        }

        public void Stop()
        {
            movieInterval.Enabled = false;
            showInterval.Enabled = false;
        }

        public void UpdateMovies()
        {
            movieInterval = new Timer(intervalMovie);
            movieInterval.Elapsed += UpdateMoviesNow;
            movieInterval.Enabled = true;
        }

        private void UpdateMoviesNow(object sender, ElapsedEventArgs args)
        {
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
                            }
                        }
                        else
                        {
                            if (movieInfo.ReleaseDate > DateTime.MinValue)
                            {
                                movie.ReleaseDate = movieInfo.ReleaseDate;
                            }
                        }
                    }
                }
                finally
                {
                    UnitOfWork.Current.Commit();
                }
            }
        }
        
        public void UpdateEpisodes()
        {
            showInterval = new Timer(intervalShow);
            showInterval.Elapsed += UpdateShowsNow;
            showInterval.Enabled = true;
        }

        private void UpdateShowsNow(object sender, ElapsedEventArgs args)
        {
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

                        if (showDto.ReleaseNextEpisode.HasValue && showDto.ReleaseNextEpisode != show.ReleaseNextEpisode)
                        {
                            show.ReleaseNextEpisode = showDto.ReleaseNextEpisode.Value;
                        }
                    }
                }
                finally
                {
                    UnitOfWork.Current.Commit();
                }
            }
        }
    }
}