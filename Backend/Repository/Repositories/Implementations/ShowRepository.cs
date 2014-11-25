using Repository.Entities;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories.Implementations
{
    public class ShowRepository : Repository<Show>, IShowRepository
    {
        public ShowRepository(WatcherData dataContext)
            : base(dataContext)
        {
        }
    }
}