using System;
using System.Linq;
using System.Linq.Expressions;

namespace Repository.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        T Insert(T entity);
        void Delete(T entity);
        void Update();
        T GetById(int id);
        IQueryable<T> All();
        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);
    }
}
