using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Repository.Repositories.Interfaces;
using Repository.UOW;

namespace Repository.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext dbContext
        {
            get
            {
                return UnitOfWork.Current.DbContext;
            }
        }

        public T Insert(T entity)
        {
            dbContext.Set<T>().Add(entity);
            dbContext.SaveChanges();

            return entity;
        }

        public void Update(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            dbContext.Set<T>().Remove(entity);
        }
        
        public IQueryable<T> All()
        {
            return dbContext.Set<T>();
        }

        public IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return dbContext.Set<T>().Where(predicate);
        }
        
        public T GetById(int id)
        {
            return dbContext.Set<T>().Find(id);
        }
    }
}