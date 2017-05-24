using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Shared;
using DDI.Shared.Extensions;

namespace DDI.Services
{
    /// <summary>
    /// Factory class for creating Service instances using the DI Container.
    /// </summary>
    public class ServiceFactory : IServiceFactory
    {
        private static Type _iserviceTypeGeneric = typeof(IService<>);
        private static Type _iserviceType = typeof(IService);
        private static Type _iserviceBaseType = typeof(ServiceBase<>);
        private static bool _servicesRegistered = false;    
        private static object _lockObject = new object();

        #region Public Methods

        /// <summary>
        /// Create a service instance.
        /// </summary>
        /// <typeparam name="T">Service type, which can be a concrete class or interface.</typeparam>
        /// <param name="unitOfWork">UnitOfWork to use for the service instance.</param>
        /// <returns></returns>
        public T CreateService<T>(IUnitOfWork unitOfWork) where T : IService
        {
            return (T)CreateService(typeof(T), unitOfWork);
        }

        /// <summary>
        /// Create a service for a specified type, which can be a concrete class or interface.
        /// <param name="unitOfWork">UnitOfWork to use for the service instance.</param>
        /// </summary>
        public IService CreateService(Type serviceType, IUnitOfWork unitOfWork)
        {
            lock (_lockObject)
            {
                if (!_servicesRegistered)
                {
                    RegisterServices();
                }
            }
            return (IService)DIContainer.Resolve(serviceType, unitOfWork, ServiceTypeResolver);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Logic for helping to resolve a service interface or ServiceBase to an actual service type.
        /// </summary>
        private Type ServiceTypeResolver(Type serviceType)
        {
            Type concreteType = null;

            // If serviceType is IService<entity> then use ServiceBase<entity>
            if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == _iserviceTypeGeneric)
            {
                concreteType = _iserviceBaseType.MakeGenericType(serviceType.GenericTypeArguments);
            }
            // If serviceType is ServiceBase<entity> then use it.
            else if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == _iserviceBaseType)
            {
                concreteType = serviceType;
            }
            return concreteType;
        }

        /// <summary>
        /// Use Reflection to get all the services in this assembly and register them with the DI Container.
        /// Interfaces and service base classes are mapped to the actual service class so that the DI container will
        /// inject an AccountClass into a parameter of type ServiceBase&lt;Account&gt; or IService&lt;Account&gt;
        /// </summary>
        private void RegisterServices()
        {
            foreach (var type in typeof(ServiceFactory).Assembly.GetTypes().Where(p => p.IsClass && p.IsPublic && !p.IsAbstract))
            {
                // If this type implements IService, it maps to itself.
                if (_iserviceType.IsAssignableFrom(type))
                {
                    DIContainer.Register(type, type);
                }

                // Any interfaces implementing IService should map to this type.
                foreach (var interfaceType in type.GetInterfaces())
                {
                    if (interfaceType != _iserviceType && _iserviceType.IsAssignableFrom(interfaceType))
                    {
                        DIContainer.Register(interfaceType, type);
                    }
                }

                // Any base classes that implement IService should map to this type
                Type baseType = type.BaseType;
                while (true)
                {
                    if (_iserviceType.IsAssignableFrom(baseType))
                    {
                        baseType = baseType.BaseType;
                        DIContainer.Register(baseType, type);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            _servicesRegistered = true;
        }

        #endregion
    }
}
