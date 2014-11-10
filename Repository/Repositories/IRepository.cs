using System.Linq;
using Repository.Entities;

namespace Repository.Repositories
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        IQueryable<TEntity> All();
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);
        TEntity Get(int key);
        int Count(); 
    }
}