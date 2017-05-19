using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Data;
using DDI.Shared.Interfaces;

namespace DDI.Shared
{
    /// <summary>
    /// Static factory for creating repositories, UnitOfWork.
    /// </summary>
    public static class Factory
    {
        private static IRepositoryFactory _repositoryFactory = null;

        public static void RegisterRepositoryFactory(IRepositoryFactory factory)
        {
            _repositoryFactory = factory;
        }

        /// <summary>
        /// Create a UnitOfWork.
        /// </summary>
        /// <returns></returns>
        public static IUnitOfWork CreateUnitOfWork()
        {
            return _repositoryFactory.CreateUnitOfWork();
        }

        /// <summary>
        /// Create a Repository.
        /// </summary>
        public static IRepository<T> CreateRepository<T>() where T : class
        {
            return _repositoryFactory.CreateRepository<T>();
        }

        public static void ConfigureForTesting()
        {
            if (_repositoryFactory == null)
            {
                _repositoryFactory = new RepositoryFactoryNoDb();
            }
        }
    }
}
