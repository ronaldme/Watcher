using System.Data.Entity;
using Repository.Entities;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories.Implementations
{
    public class ActorRepository : Repository<Actor>, IActorRepository
    {
        public ActorRepository(DbContext dataContext)
            : base(dataContext)
        {
        }
    }
}
