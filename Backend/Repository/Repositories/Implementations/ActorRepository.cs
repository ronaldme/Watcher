using Repository.Repositories.Interfaces;

namespace Repository.Repositories.Implementations
{
    public class ActorRepository : Repository<Actor>, IActorRepository
    {
        public ActorRepository(WatcherData dataContext)
            : base(dataContext)
        {
        }
    }
}
