using System;
using System.Reflection;
using Castle.Core.Internal;

namespace Repository.Repositories.UnitOfWork
{
    public static class UnitOfWorkHelper
    {
        public static bool IsRepositoryMethod(MethodInfo methodInfo)
        {
            return IsRepositoryClass(methodInfo.DeclaringType);
        }

        public static bool IsRepositoryClass(Type type)
        {
            return typeof(IRepository<>).IsAssignableFrom(type);
        }

        public static bool HasUnitOfWorkAttribute(MethodInfo methodInfo)
        {
            return methodInfo.IsDefined(typeof(UnitOfWorkAttribute), true);
        }

        public static bool IsDistributedTransactionalUnitOfWork(MethodInfo methodInfo)
        {
            return methodInfo.GetAttribute<UnitOfWorkAttribute>().DistributedTransactional;
        }
    }
}
