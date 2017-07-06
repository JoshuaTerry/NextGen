using System;
using System.Linq;

namespace DDI.Shared
{
    public interface IRepositoryFactory : IDisposable
    {
        IUnitOfWork CreateUnitOfWork();
        IRepository<T> CreateRepository<T>() where T : class;
        IRepository<T> CreateRepository<T>(IQueryable<T> dataSource) where T : class;
    }
}
