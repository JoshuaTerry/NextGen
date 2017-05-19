using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared
{
    public interface IRepositoryFactory
    {
        IUnitOfWork CreateUnitOfWork();
        IRepository<T> CreateRepository<T>() where T : class;
        IRepository<T> CreateRepository<T>(IQueryable<T> dataSource) where T : class;
    }
}
