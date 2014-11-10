using System;
using System.Data.Entity;

namespace Repository.Repositories.UnitOfWork
{
    namespace VLC.Infrastructure.Repository
    {
        /// <summary>
        /// Implements Unit of work
        /// </summary>
        public class UnitOfWork : IUnitOfWork
        {
            /// <summary>
            /// Gets current instance of the NhUnitOfWork.
            /// It gets the right instance that is related to current thread.
            /// </summary>
            public static UnitOfWork Current
            {
                get { return current; }
                set { current = value; }
            }
            [ThreadStatic]
            private static UnitOfWork current;

            /// <summary>
            /// Triggers when the transaction is successfully completed.
            /// </summary>
            public event EventHandler TransactionCompleted;

            /// <summary>
            /// Gets Nhibernate session object to perform queries.
            /// </summary>
            public DbContext DbContext { get; private set; }

            /// <summary>
            /// Reference to the currently running transaction.
            /// </summary>
            private DbContextTransaction transaction;

            /// <summary>
            /// Creates a new instance of NhUnitOfWork.
            /// </summary>
            /// <param name="dbContext"></param>
            public UnitOfWork(DbContext dbContext)
            {
                DbContext = dbContext;
            }

            /// <summary>
            /// Opens database connection and begins transaction.
            /// </summary>
            public void BeginTransaction()
            {
                // this.DbContext.Configuration.AutoDetectChangesEnabled = false;
                transaction = DbContext.Database.BeginTransaction();
            }

            /// <summary>
            /// Commits transaction and closes database connection.
            /// </summary>
            public void Commit()
            {
                try
                {
                    DbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                { }

                EventHandler handler = TransactionCompleted;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }

            /// <summary>
            /// Rollbacks transaction and closes database connection.
            /// </summary>
            public void Rollback()
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}