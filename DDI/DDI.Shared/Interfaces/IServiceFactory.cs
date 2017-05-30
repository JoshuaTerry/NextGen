using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared
{
    public interface IServiceFactory
    {
        T CreateService<T>(IUnitOfWork unitOfWork) where T : IService;
        IService CreateService(Type serviceType, IUnitOfWork unitOfWork);
    }
}
