using Repository.Entities;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories.Implementations
{
    public class UsersRepository : Repository<User>, IUsersRepository
    {
        public UsersRepository(WatcherData dataContext)
            : base(dataContext)
        {
        }
    }
}
