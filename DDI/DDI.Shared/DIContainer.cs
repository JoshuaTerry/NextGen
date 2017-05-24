using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Extensions;

namespace DDI.Shared
{
    public static class DIContainer
    {

        private static Dictionary<Type, RegisteredType> _registeredTypes;

        private static Type _iunitOfWorkType = typeof(IUnitOfWork);
        private static Type _ibusinessLogicType = typeof(IBusinessLogic);
        private static Type _irepositoryType = typeof(IRepository<>);
        private static Type _iserviceType = typeof(IService);
        private static MethodInfo _getRepository = null;
        private static object _lockObject;

        static DIContainer()
        {
            _registeredTypes = new Dictionary<Type, RegisteredType>();
            _lockObject = new object();
        }

        public static void Register(Type fromType, Type toType)
        {
            _registeredTypes[fromType] = new RegisteredType(toType);
        }

        public static object Resolve(Type requestedType) => Resolve(requestedType, null, null);

        public static object Resolve(Type requestedType, IUnitOfWork unitOfWork, Func<Type,Type> typeResolver)
        {
            lock (_lockObject)
            {
                RegisteredType rtype = _registeredTypes.GetValueOrDefault(requestedType);
                if (rtype == null && typeResolver != null)
                {
                    Type otherType = typeResolver(requestedType);
                    if (otherType != null)
                    {
                        Register(requestedType, otherType);
                        rtype = _registeredTypes[requestedType];
                    }
                }
                if (rtype == null)
                {
                    Register(requestedType, requestedType);
                    rtype = _registeredTypes[requestedType];
                }

                if (rtype.Parameters == null)
                {
                    GetParameterTypes(rtype);
                }

                List<object> parameters = new List<object>();
                foreach (var entry in rtype.Parameters)
                {
                    switch (entry.Mode)
                    {
                        case ServiceParameterMode.UnitOfWork:
                            if (unitOfWork == null)
                            {
                                unitOfWork = Factory.CreateUnitOfWork();
                            }
                            parameters.Add(unitOfWork);
                            break;

                        case ServiceParameterMode.BusinessLogic:
                            if (unitOfWork == null)
                            {
                                unitOfWork = Factory.CreateUnitOfWork();
                            }
                            parameters.Add(unitOfWork.GetBusinessLogic(entry.Type));
                            break;

                        case ServiceParameterMode.Repository:
                            if (unitOfWork == null)
                            {
                                unitOfWork = Factory.CreateUnitOfWork();
                            }
                            if (_getRepository == null)
                            {
                                _getRepository = _iunitOfWorkType.GetMethod(nameof(IUnitOfWork.GetRepository));
                            }
                            parameters.Add(_getRepository.MakeGenericMethod(entry.Type).Invoke(unitOfWork, null));
                            break;

                        case ServiceParameterMode.Service:
                            if (unitOfWork == null)
                            {
                                unitOfWork = Factory.CreateUnitOfWork();
                            }
                            IService service = Factory.CreateService(entry.Type, unitOfWork);
                            if (service == null)
                            {
                                throw new InvalidOperationException($"Cannot create a service of type {entry.Type.FullName}.");
                            }
                            parameters.Add(service);
                            break;
                    }
                }

                return Activator.CreateInstance(rtype.MappedType, parameters.ToArray());
            }
        }

        /// <summary>
        /// Get the list of parameter types for the service constructor.
        private static void GetParameterTypes(RegisteredType registeredType)
        {
            IList<ServiceParameter> paramTypes = new List<ServiceParameter>();

            foreach (var entry in registeredType.MappedType.GetConstructors().Where(p => p.IsPublic && !p.IsStatic))
            {
                paramTypes.Clear();
                bool isValid = true;

                // Look for constructor parameters that are IService, IBusinessLogic, IUnitOfWork, or IRepository.
                foreach (var param in entry.GetParameters())
                {
                    Type paramType = param.ParameterType;
                    if (paramType == _iunitOfWorkType)
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
                    else if (_iserviceType.IsAssignableFrom(paramType))
                    {
                        paramTypes.Add(new ServiceParameter(paramType, ServiceParameterMode.Service));
                    }
                    else if (_ibusinessLogicType.IsAssignableFrom(paramType))
                    {
                        paramTypes.Add(new ServiceParameter(paramType, ServiceParameterMode.BusinessLogic));
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

            registeredType.Parameters = paramTypes;            
        }


        private class RegisteredType
        {
            public Type MappedType { get; set; }
            public IList<ServiceParameter> Parameters { get; set; }

            public RegisteredType(Type type)
            {
                MappedType = type;
                Parameters = null;
            }
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
            Service, BusinessLogic, UnitOfWork, Repository
        }
    }
}
