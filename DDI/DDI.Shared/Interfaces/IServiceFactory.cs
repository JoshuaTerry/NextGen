using System;

namespace DDI.Shared
{
    public interface IServiceFactory
    {
        T CreateService<T>(IUnitOfWork unitOfWork) where T : IService;
        IService CreateService(Type serviceType, IUnitOfWork unitOfWork);
    }
}
