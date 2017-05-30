﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Interfaces
{
    // Interface for a factory that can generate UnitOfWork, Repository, Service and Controller objects.
    public interface IFactory : IDisposable
    {
        IUnitOfWork CreateUnitOfWork();
        IRepository<T> CreateRepository<T>() where T : class;
        T CreateService<T>() where T : IService;
        T CreateService<T>(IUnitOfWork unitOfWork) where T : IService;
        IService CreateService(Type serviceType, IUnitOfWork unitOfWork);
        object CreateController(Type controllerType);
        object CreateController(Type controllerType, IUnitOfWork unitOfWork);
    }
}
