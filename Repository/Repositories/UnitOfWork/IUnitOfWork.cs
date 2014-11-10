using System;

namespace Repository.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Opens database connection and begins transaction.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Commits transaction and closes database connection.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollbacks transaction and closes database connection.
        /// </summary>
        void Rollback();

        /// <summary>
        /// Triggers when the transaction is successfully completed.
        /// </summary>
        event EventHandler TransactionCompleted;
    }
}