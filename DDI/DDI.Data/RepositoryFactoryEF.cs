using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared;
using DDI.Shared.Data;
using DDI.Shared.Interfaces;

namespace DDI.Data
{
    public class RepositoryFactoryEF : IRepositoryFactory
    {
        public IRepository<T> CreateRepository<T>() where T : class
        {
            return new Repository<T>();
        }

        public IRepository<T> CreateRepository<T>(IQueryable<T> dataSource) where T : class
        {
            return new RepositoryNoDb<T>(dataSource);
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWorkEF();
        }
    }
}
