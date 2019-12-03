using System.Threading.Tasks;

namespace Watcher.Service.Services
{
    public interface INotifyScheduler
    {
        Task Start();
        Task Stop();
    }
}