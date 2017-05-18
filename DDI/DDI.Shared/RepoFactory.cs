using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared
{
    public static class RepoFactory
    {
        private static Type _unitOfWorkType = null;
        private static Type _repositoryType = null;

        public static IUnitOfWork CreateUnitOfWork()
        {
            return (IUnitOfWork)Activator.CreateInstance(_unitOfWorkType);
        }

        public static IRepository<T> CreateRepository<T>() where T : class
        {
            return (IRepository<T>)Activator.CreateInstance(_repositoryType.MakeGenericType(typeof(T)));
        }

        public static void RegisterTypes(Type unitOfWork, Type repository)
        {
            _unitOfWorkType = unitOfWork;
            _repositoryType = repository;
        }

    }
}
