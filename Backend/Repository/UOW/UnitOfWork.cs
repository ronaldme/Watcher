using System;
using System.Data.Entity;

namespace Repository.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext DbContext { get; private set; }
        public static UnitOfWork Current
        {
            get { return current; }
            set { current = value; }
        }

        [ThreadStatic]
        private static UnitOfWork current;
        private DbContextTransaction transaction;

        public UnitOfWork(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public void BeginTransaction()
        {
            transaction = DbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            DbContext.SaveChanges();
            transaction.Commit();
        }

        public void Rollback()
        {
            transaction.Rollback();
        }
    }
}