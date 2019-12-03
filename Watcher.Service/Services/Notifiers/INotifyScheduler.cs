using System.Threading.Tasks;

namespace Watcher.Service.Services.Notifiers
{
    public interface INotifyScheduler
    {
        Task Start();
        Task Stop();
    }
}