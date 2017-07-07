using DDI.Shared;
using DDI.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace DDI.WebApi.IoC
{
    /// <summary>
    /// A custom DependencyScope that references a disposable Factory object for ensuring that objects created during a Http request scope will be disposed.
    /// </summary>
    public class DependencyScope : IDependencyScope
    {
        private IFactory _factory;

        public DependencyScope()
        {
            _factory = Factory.CreateDisposableFactory();            
        }

        public IFactory GetFactory()
        {
            return _factory;
        }

        public void Dispose()
        {
            _factory.Dispose();
        }

        public object GetService(Type serviceType)
        {
            return _factory.CreateController(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new object[] { GetService(serviceType) };
        }
    }
}