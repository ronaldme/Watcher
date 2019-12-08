using System.Threading.Tasks;

namespace Watcher.Service.Services
{
    public interface IUpdateService
    {
        Task UpdateMovies();
        Task UpdateShows();
        Task UpdatePersons();
    }
}