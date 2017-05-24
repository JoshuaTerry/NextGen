using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Extensions;

namespace DDI.Shared
{
    /// <summary>
    /// Dependency Injection (DI) Container Class
    /// </summary>
    /// <remarks>
    /// Simplified DI functionality targeted to Controllers, Services, and Business Logic. 
    /// The only types of constructor parameters than be injected are services, UnitOfWork, Repository, and business logic.
    /// Types can be registered via Register(), with the ability to register a mapping from an interface to a concrete class.
    /// The container will auto-register types when Resolve() is called for a non-registered type.
    /// 
    /// </remarks>
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

        #region Public Methods

        /// <summary>
        /// Register a type and the actual type it should resolve to.
        /// </summary>
        /// <param name="fromType">Type to be registered.</param>
        /// <param name="toType">Type to be created by Resolve().</param>
        public static void Register(Type fromType, Type toType) => RegisterWithReturn(fromType, toType);

        /// <summary>
        /// Register a type.
        /// </summary>
        public static void Register(Type type) =>  RegisterWithReturn(type, type);

        /// <summary>
        /// Return an instance of a type, injecting any dependencies to its constructor.
        /// <param name="requestedType">Type to be resolved.</param>
        /// </summary>
        public static object Resolve(Type requestedType) => Resolve(requestedType, null, null);

        /// <summary>
        /// Return an instance of a type, injecting any dependencies to its constructor.
        /// <param name="requestedType">Type to be resolved.</param>
        /// <param name="unitOfWork">A IUnitOfWork that will be used for any dependencies.</param>
        /// </summary>
        public static object Resolve(Type requestedType, IUnitOfWork unitOfWork) => Resolve(requestedType, unitOfWork, null);

        /// <summary>
        /// Return an instance of a type, injecting any dependencies to its constructor.
        /// </summary>
        /// <param name="requestedType">Type to be resolved.</param>
        /// <param name="unitOfWork">A IUnitOfWork that will be used for any dependencies.</param>
        /// <param name="typeResolver">A function that can be called to resolve unregistered types.</param>
        /// <returns></returns>
        public static object Resolve(Type requestedType, IUnitOfWork unitOfWork, Func<Type,Type> typeResolver)
        {
            RegisteredType rtype = null;
            lock (_lockObject)
            {
                // If the type is registered, get the RegisteredType object.
                rtype = _registeredTypes.GetValueOrDefault(requestedType);
            }
            if (rtype == null && typeResolver != null)
            {
                // The type is not registered yet, but call the typeResolver to resolve it to a concrete class type.
                Type otherType = typeResolver(requestedType);
                if (otherType != null)
                {
                    rtype = RegisterWithReturn(requestedType, otherType);
                }
            }
            if (rtype == null)
            {
                // As a last resort, simply register the type.
                rtype = RegisterWithReturn(requestedType, requestedType);
            }

            if (rtype.Parameters == null)
            {
                // If the parameter list has not been determined, get it.
                GetParameterTypes(rtype);
            }

            // Build the parameter list.
            List<object> parameters = new List<object>();
            foreach (var entry in rtype.Parameters)
            {
                switch (entry.Mode)
                {
                    case ParameterMode.UnitOfWork:
                        if (unitOfWork == null)
                        {
                            unitOfWork = Factory.CreateUnitOfWork();
                        }
                        parameters.Add(unitOfWork);
                        break;

                    case ParameterMode.BusinessLogic:
                        if (unitOfWork == null)
                        {
                            unitOfWork = Factory.CreateUnitOfWork();
                        }
                        parameters.Add(unitOfWork.GetBusinessLogic(entry.Type));
                        break;

                    case ParameterMode.Repository:
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

                    case ParameterMode.Service:
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

            // Create an instance of the type.
            return Activator.CreateInstance(rtype.MappedType, parameters.ToArray());
        }

        #endregion

        #region Private Methods

        private static RegisteredType RegisterWithReturn(Type fromType, Type toType)
        {
            if (fromType == null)
            {
                throw new ArgumentNullException(nameof(fromType));
            }
            if (toType == null)
            {
                throw new ArgumentNullException(nameof(toType));
            }
            if (!toType.IsClass)
            {
                throw new InvalidOperationException($"In order to register the type {fromType.FullName}, it must either be a class or be mapped to a class.");
            }
            lock (_lockObject)
            {
                RegisteredType rtype = new RegisteredType(toType);
                _registeredTypes[fromType] = rtype;
                return rtype;
            }
        }


        /// <summary>
        /// Get the list of parameter types for a registered type.
        /// </summary>
        private static void GetParameterTypes(RegisteredType registeredType)
        {
            lock (_lockObject)
            {
                List<Parameter> paramTypes = new List<Parameter>();
                List<Parameter> maxParamTypes = new List<Parameter>();

                foreach (var entry in registeredType.MappedType.GetConstructors().Where(p => p.IsPublic && !p.IsStatic))
                {
                    paramTypes.Clear();
                    bool isValid = true;

                    // Look for constructor parameters that are IService, IBusinessLogic, IUnitOfWork, or IRepository.  These are the only types that can be injected.
                    foreach (var param in entry.GetParameters())
                    {
                        Type paramType = param.ParameterType;
                        if (paramType == _iunitOfWorkType)
                        {
                            paramTypes.Add(new Parameter(paramType, ParameterMode.UnitOfWork));
                        }
                        else if (paramType.IsConstructedGenericType && paramType.GetGenericTypeDefinition() == _irepositoryType)
                        {
                            var typeArgs = paramType.GenericTypeArguments;
                            if (typeArgs.Length == 1)
                            {
                                paramTypes.Add(new Parameter(typeArgs[0], ParameterMode.Repository));
                            }
                            else
                            {
                                isValid = false;
                            }
                        }
                        else if (_iserviceType.IsAssignableFrom(paramType))
                        {
                            paramTypes.Add(new Parameter(paramType, ParameterMode.Service));
                        }
                        else if (_ibusinessLogicType.IsAssignableFrom(paramType))
                        {
                            paramTypes.Add(new Parameter(paramType, ParameterMode.BusinessLogic));
                        }
                        else
                        {
                            // This type cannot be injected.
                            isValid = false;
                        }
                        if (!isValid)
                        {
                            break;
                        }
                    }

                    if (isValid && paramTypes.Count > maxParamTypes.Count)
                    {
                        // Go with the constructor with the largest # of parameters that can be injected.
                        maxParamTypes.Clear();
                        maxParamTypes.AddRange(paramTypes);
                    }
                }

                registeredType.Parameters = maxParamTypes;
            }
        }

        #endregion

        #region Nested Classes

        private class RegisteredType
        {
            public Type MappedType { get; set; }
            public IList<Parameter> Parameters { get; set; }

            public RegisteredType(Type type)
            {
                MappedType = type;
                Parameters = null;
            }
        }

        private class Parameter
        {
            public Type Type { get; set; }
            public ParameterMode Mode { get; set; }

            public Parameter(Type type, ParameterMode mode)
            {
                Type = type;
                Mode = mode;
            }
        }

        private enum ParameterMode
        {
            Service, BusinessLogic, UnitOfWork, Repository
        }

        #endregion
    }
}
