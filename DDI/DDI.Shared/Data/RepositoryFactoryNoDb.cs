using System;
using System.Linq;
using DDI.Shared.Interfaces;

namespace DDI.Shared.Data
{
    public class RepositoryFactoryNoDb : IRepositoryFactory
    {
        public IRepository<T> CreateRepository<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public IRepository<T> CreateRepository<T>(IQueryable<T> dataSource) where T : class
        {
            return new RepositoryNoDb<T>(dataSource);
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWorkNoDb();
        }
    }
}
