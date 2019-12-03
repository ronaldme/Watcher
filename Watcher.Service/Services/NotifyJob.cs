using System.Threading.Tasks;
using Quartz;

namespace Watcher.Service.Services
{
    public class NotifyJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            // TODO: Notify users
        }
    }
}