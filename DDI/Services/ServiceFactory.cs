using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Shared;
using DDI.Shared.Extensions;

namespace DDI.Services
{
    public class ServiceFactory : IServiceFactory
    {
        private static Dictionary<Type, Type> _serviceImplementations = null;
        private static Dictionary<Type, IList<ServiceParameter>> _parameterTypes = null;

        private static Type _iunitOfWorkType = typeof(IUnitOfWork);
        private static Type _ibusinessLogicType = typeof(IBusinessLogic);
        private static Type _irepositoryType = typeof(IRepository<>);
        private static Type _iserviceType = typeof(IService<>);
        private static Type _iserviceBaseType = typeof(ServiceBase<>);

        private static object _lockObject = new object();

        #region Public Methods

        public T CreateService<T>(IUnitOfWork unitOfWork) where T : IService
        {
            return (T)CreateService(typeof(T), unitOfWork);
        }

        /// <summary>
        /// Create a service for a specified type, which can be a concrete class or interface.
        /// </summary>
        public IService CreateService(Type serviceType, IUnitOfWork unitOfWork)
        {
            lock (_lockObject)
            {
                if (_serviceImplementations == null)
                {
                    GetServiceImplementations();
                }

                // Try to get the concrete type from the set of known services.

                Type concreteType = _serviceImplementations.GetValueOrDefault(serviceType);
                if (concreteType == null)
                {
                    // If serviceType is IService<entity> then use ServiceBase<entity>
                    if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == _iserviceType)
                    {
                        concreteType = _iserviceBaseType.MakeGenericType(serviceType.GenericTypeArguments);
                    }
                    // If serviceType is ServiceBase<entity> then use it.
                    else if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == _iserviceBaseType)
                    {
                        concreteType = serviceType;
                    }
                }
                if (concreteType == null)
                {
                    throw new ArgumentException($"Type {serviceType.FullName} is not a valid Service type.", nameof(serviceType));
                }

                // Get the parameter types for the constructor
                var paramTypes = GetParameterTypes(concreteType);
                if (paramTypes == null)
                {
                    throw new InvalidOperationException($"Type { concreteType.FullName } does not have a usable constructor.");
                }

                // Create the parameters for the constructor, which will must be UnitOfWork, Repository, or BusinessLogic.

                List<object> parameters = new List<object>();

                foreach (var entry in paramTypes)
                {
                    if (entry.Mode == ServiceParameterMode.UnitOfWork)
                    {
                        parameters.Add(unitOfWork);
                    }
                    else if (entry.Mode == ServiceParameterMode.BusinessLogic)
                    {
                        parameters.Add(unitOfWork.GetBusinessLogic(entry.Type));
                    }
                    else if (entry.Mode == ServiceParameterMode.Repository)
                    {
                        parameters.Add(_iunitOfWorkType.GetMethod(nameof(IUnitOfWork.GetRepository)).MakeGenericMethod(entry.Type).Invoke(unitOfWork, null));
                    }
                }

                return (IService)Activator.CreateInstance(concreteType, parameters.ToArray());
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Use Reflection to get all the services in this assembly and map them to their interfaces and base classes.
        /// </summary>
        private void GetServiceImplementations()
        {
            Type serviceType = typeof(IService);

            _serviceImplementations = new Dictionary<Type, Type>();
            foreach (var type in typeof(ServiceFactory).Assembly.GetTypes().Where(p => p.IsClass && p.IsPublic && !p.IsAbstract))
            {
                // IF this type is an IService, it maps to itself.
                if (serviceType.IsAssignableFrom(type))
                {
                    _serviceImplementations[type] = type;
                }

                // Any IService interfaces should map to this type.
                foreach (var interfaceType in type.GetInterfaces())
                {
                    if (interfaceType != serviceType && serviceType.IsAssignableFrom(interfaceType))
                    {
                        _serviceImplementations[interfaceType] = type;
                    }
                }

                // Any base classes of type IService should map to this type
                Type baseType = type.BaseType;
                while (true)
                {
                    if (serviceType.IsAssignableFrom(baseType))
                    {
                        _serviceImplementations[baseType] = type;
                        baseType = baseType.BaseType;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Get the list of parameter types for the service constructor.
        private static IList<ServiceParameter> GetParameterTypes(Type serviceType)
        {
            if (_parameterTypes == null)
            {
                _parameterTypes = new Dictionary<Type, IList<ServiceParameter>>();
            }

            IList<ServiceParameter> paramTypes = _parameterTypes.GetValueOrDefault(serviceType);
            if (paramTypes == null)
            {
                paramTypes = new List<ServiceParameter>();

                foreach (var entry in serviceType.GetConstructors().Where(p => p.IsPublic && !p.IsStatic))
                {
                    paramTypes.Clear();
                    bool isValid = true;

                    // Look for service constructor parameters that are IService, IBusinessLogic, or IUnitOfWork.
                    foreach (var param in entry.GetParameters())
                    {
                        Type paramType = param.ParameterType;
                        if (_ibusinessLogicType.IsAssignableFrom(paramType))
                        {
                            paramTypes.Add(new ServiceParameter(paramType, ServiceParameterMode.BusinessLogic));
                        }
                        else if (paramType == _iunitOfWorkType)
                        {
                            paramTypes.Add(new ServiceParameter(paramType, ServiceParameterMode.UnitOfWork));
                        }
                        else if (paramType.IsConstructedGenericType && paramType.GetGenericTypeDefinition() == _irepositoryType)
                        {
                            var typeArgs = paramType.GenericTypeArguments;
                            if (typeArgs.Length == 1)
                            {
                                paramTypes.Add(new ServiceParameter(typeArgs[0], ServiceParameterMode.Repository));
                            }
                            else
                            {
                                isValid = false;
                            }
                        }
                        else
                        {
                            isValid = false;
                        }
                        if (!isValid)
                        {
                            break;
                        }
                    }

                    if (!isValid)
                    {
                        continue;
                    }
                    if (paramTypes.Count > 0)
                    {
                        // Once we find a constructor that takes a recognized parameter types, stop looking for other constructors.
                        break;
                    }
                }

                _parameterTypes.Add(serviceType, paramTypes);
            }
            return paramTypes;
        }


        #endregion

        #region Nested Types

        private class ServiceParameter
        {
            public Type Type { get; set; }
            public ServiceParameterMode Mode { get; set; }

            public ServiceParameter(Type type, ServiceParameterMode mode)
            {
                Type = type;
                Mode = mode;
            }
        }

        private enum ServiceParameterMode
        {
            BusinessLogic, UnitOfWork, Repository
        }

        #endregion

    }
}
