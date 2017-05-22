using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

namespace DDI.WebApi.IoC
{
    /// <summary>
    /// A custom DependencyResolver for the IoC logic - creates IoC DependencyScope objects.
    /// </summary>
    public class DependencyResolver : System.Web.Http.Dependencies.IDependencyResolver
    {
        public IDependencyScope BeginScope()
        {

            return new DependencyScope();
        }

        public void Dispose()
        {

        }

        public object GetService(Type serviceType)
        {
            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new object[0];
        }
    }
}