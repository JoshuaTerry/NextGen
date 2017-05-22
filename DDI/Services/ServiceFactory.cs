using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared;
using DDI.Shared.Extensions;
using DDI.Shared.Helpers;
using DDI.Shared.Models;

namespace DDI.Services
{
    public class ServiceFactory : IServiceFactory
    {
        private static Dictionary<Type, Type> _serviceImplementations = null;
        private static Dictionary<Type, IList<ServiceParameter>> _parameterTypes = null;
        private static Type iunitOfWorkType = typeof(IUnitOfWork);
        private static Type ibusinessLogicType = typeof(IBusinessLogic);

        public T CreateService<T>(IUnitOfWork unitOfWork) where T : IService
        {
            return (T)CreateService(typeof(T), unitOfWork);
        }

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
                        if (ibusinessLogicType.IsAssignableFrom(paramType))
                        {
                            paramTypes.Add(new ServiceParameter(paramType, ServiceParameterMode.BusinessLogic));
                        }
                        else if (paramType == iunitOfWorkType)
                        {
                            paramTypes.Add(new ServiceParameter(paramType, ServiceParameterMode.UnitOfWork));
                        }
                        else
                        {
                            isValid = false;
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


        public IService CreateService(Type serviceType, IUnitOfWork unitOfWork)
        {
            if (_serviceImplementations == null)
            {
                GetServiceImplementations();
            }

            serviceType = _serviceImplementations.GetValueOrDefault(serviceType);
            if (serviceType == null)
            {
                throw new ArgumentException($"Type {serviceType.FullName} is not a valid Service type.", nameof(serviceType));
            }

            var paramTypes = GetParameterTypes(serviceType);
            if (paramTypes == null)
            {
                throw new InvalidOperationException($"Type { serviceType.FullName } does not have a usable constructor.");
            }

            // Create the parameters for the constructor, which will be the actual service objects.
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
            }

            return (IService)Activator.CreateInstance(serviceType, parameters.ToArray());
        }

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
            BusinessLogic, UnitOfWork
        }
    }
}
