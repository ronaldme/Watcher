using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Watcher.Service.API;

namespace Watcher.Service.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly ITheMovieDb theMovieDb;
        private readonly ILogger<UpdateService> _logger;

        public UpdateService(ITheMovieDb theMovieDb,
            ILogger<UpdateService> logger)
        {
            this.theMovieDb = theMovieDb;
            _logger = logger;
        }

        public async Task UpdateMovies()
        {
            _logger.LogDebug("Start updating movies");
        }
        
        public async Task UpdateShows()
        {
            _logger.LogDebug("Start updating shows");
        }
        public async Task UpdatePersons()
        {
            _logger.LogDebug("Start updating persons");
        }
    }
}