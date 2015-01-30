using Repository.Entities;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories.Implementations
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
    }
}
