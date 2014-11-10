using System;

namespace Repository.Repositories.UnitOfWork
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UnitOfWorkAttribute : Attribute
    {
        public UnitOfWorkAttribute()
            : this(false)
        { }


        public UnitOfWorkAttribute(bool distributedTransactional)
        {
            this.distributedTransactional = distributedTransactional;
        }

        public bool DistributedTransactional { get { return distributedTransactional; } }

        private readonly bool distributedTransactional;
    }
}