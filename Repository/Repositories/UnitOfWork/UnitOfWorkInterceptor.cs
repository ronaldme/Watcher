using System;
using System.Reflection;

namespace Repository.Repositories.UnitOfWork
{
    /// <summary>
    /// This interceptor is used to manage transactions.
    /// </summary>
    public class UnitOfWorkInterceptor : IInterceptor
    {
        private readonly IDbInstaller dbInstaller;
        private readonly bool distributed;

        /// <summary>
        /// Creates a new UnitOfWorkInterceptor object.
        /// </summary>
        /// <param name="dbInstaller">Nhibernate session factory.</param>
        /// <param name="distributed"></param>
        public UnitOfWorkInterceptor(IDbInstaller dbInstaller, bool distributed = false)
        {
            this.dbInstaller = dbInstaller;
            this.distributed = distributed;
        }

        /// <summary>
        /// Intercepts a method.
        /// </summary>
        /// <param name="invocation">Method invocation arguments</param>
        public void Intercept(IInvocation invocation)
        {
            //If there is a running transaction, just run the method
            if (VLC.Infrastructure.Repository.UnitOfWork.Current != null || !RequiresDbConnection(invocation.MethodInvocationTarget))
            {
                invocation.Proceed();
                return;
            }

            try
            {
                TransactionScope sc = null;
                if (!distributed)
                {
                    sc = new TransactionScope(TransactionScopeOption.Suppress);
                }

                using (var dbcontext = dbInstaller.CreateDbContext())
                {
                    VLC.Infrastructure.Repository.UnitOfWork.Current = new VLC.Infrastructure.Repository.UnitOfWork(dbcontext);
                    VLC.Infrastructure.Repository.UnitOfWork.Current.BeginTransaction();

                    try
                    {
                        invocation.Proceed();
                        if (!distributed)
                        {
                            VLC.Infrastructure.Repository.UnitOfWork.Current.Commit();
                            sc.Complete();
                            sc.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        log.WarnFormat("UnitOfWorkInterceptor exception: {0}", ex.Message);
                        try
                        {
                            VLC.Infrastructure.Repository.UnitOfWork.Current.Rollback();
                        }
                        catch
                        { }

                        throw;
                    }
                }
            }
            finally
            {
                VLC.Infrastructure.Repository.UnitOfWork.Current = null;
            }
        }

        private static bool RequiresDbConnection(MethodInfo methodInfo)
        {
            if (UnitOfWorkHelper.HasUnitOfWorkAttribute(methodInfo))
            {
                return true;
            }

            if (UnitOfWorkHelper.IsRepositoryMethod(methodInfo))
            {
                return true;
            }

            return false;
        }
    }
}
