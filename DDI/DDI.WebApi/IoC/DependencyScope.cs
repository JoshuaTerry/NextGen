using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using DDI.Shared;
using DDI.Shared.Interfaces;

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