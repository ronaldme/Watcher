using System;
using System.Configuration;
using System.Timers;
using BLL;
using Messages.DTO;
using Repository.Entities;
using Repository.Repositories.Interfaces;
using Services.Interfaces;
using Convert = System.Convert;

namespace Services
{
    public class UpdateService : IUpdateService, IStartable
    {
        private readonly ITheMovieDb theMovieDb;
        private readonly IMovieRepository movieRepository;
        private readonly IShowRepository showRepository;

        private readonly int intervalMovieHours;
        private readonly int intervalShowHours;

        private Timer movieInterval;
        private Timer showInterval;

        public UpdateService(ITheMovieDb theMovieDb, IMovieRepository movieRepository, IShowRepository showRepository)
        {
            this.theMovieDb = theMovieDb;
            this.movieRepository = movieRepository;
            this.showRepository = showRepository;

            intervalMovieHours = Convert.ToInt32(ConfigurationManager.AppSettings.Get("movieIntervalHours"));
            intervalShowHours = Convert.ToInt32(ConfigurationManager.AppSettings.Get("showIntervalHours"));
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
            movieInterval = new Timer(intervalMovieHours);
            movieInterval.Elapsed += UpdateMoviesNow;
            movieInterval.Enabled = true;
        }

        private void UpdateMoviesNow(object sender, ElapsedEventArgs args)
        {
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
                movieRepository.Update();
            }
            catch (Exception e) { }
        }
        
        public void UpdateEpisodes()
        {
            showInterval = new Timer(intervalShowHours);
            showInterval.Elapsed += UpdateShowsNow;
            showInterval.Enabled = true;
        }

        private void UpdateShowsNow(object sender, ElapsedEventArgs args)
        {
            try
            {
                var shows = showRepository.All();

                foreach (Show show in shows)
                {
                    if (show.ReleaseNextEpisode < DateTime.UtcNow)
                    {
                        ShowDTO showInfo = theMovieDb.GetShowBy(show.TheMovieDbId);
                        ShowDTO showDTO = theMovieDb.GetLatestEpisode(showInfo.Id, showInfo.Seasons);

                        if (showDTO.ReleaseNextEpisode.HasValue && showDTO.ReleaseNextEpisode != show.ReleaseNextEpisode)
                        {
                            show.ReleaseNextEpisode = show.ReleaseNextEpisode;
                        }
                    }
                }

                showRepository.Update();
            }
            catch (Exception e) { }
        }
    }
}