using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbSet<T> DbSet;
        private readonly DbContext dbContext;

        public Repository(DbContext dataContext)
        {
            DbSet = dataContext.Set<T>();
            dbContext = dataContext;
        }

        public T Insert(T entity)
        {
            DbSet.Add(entity);
            dbContext.SaveChanges();

            return entity;
        }

        public void Update()
        {
            dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
            dbContext.SaveChanges();
        }

        public IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public T GetById(int id)
        {
            return DbSet.Find(id);
        }
    }
}