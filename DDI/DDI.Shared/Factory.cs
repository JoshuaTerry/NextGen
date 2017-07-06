using System;
using DDI.Shared.Data;
using DDI.Shared.Helpers;
using DDI.Shared.Interfaces;
using System;

namespace DDI.Shared
{
    /// <summary>
    /// Static factory for creating Repository, UnitOfWork, and Service objects.
    /// </summary>
    public static class Factory
    {
        private static Type _repositoryFactoryType = null;
        private static Type _serviceFactoryType = null;

        private static IFactoryProvider _defaultProvider = null;
        private static IFactoryProvider _provider = null;
        private static bool _isForTesting = false;

        private const string NOTREGISTERED = "The RepositoryFactory and/or ServiceFactory types have not been properly registered.";

        /// <summary>
        /// Register the Repository/UnitOfWork factory class.
        /// </summary>
        public static void RegisterRepositoryFactory<T>() where T : IRepositoryFactory
        {
            _repositoryFactoryType = typeof(T);
        }

        /// <summary>
        /// Register the Service factory class.
        /// </summary>
        public static void RegisterServiceFactory<T>() where T : IServiceFactory
        {
            _serviceFactoryType = typeof(T);
        }

        /// <summary>
        /// Register the factory provider class to be used (in place of the default factory provider.)
        /// </summary>
        public static void RegisterFactoryProvider<T>() where T : IFactoryProvider, new()
        {
            _provider = new T();
        }

        /// <summary>
        /// Create a UnitOfWork.
        /// </summary>
        /// <returns></returns>
        public static IUnitOfWork CreateUnitOfWork()
        {
            return GetChildFactory().CreateUnitOfWork();
        }

        /// <summary>
        /// Create a Repository.
        /// </summary>
        public static IRepository<T> CreateRepository<T>() where T : class
        {
            return GetChildFactory().CreateRepository<T>();
        }

        /// <summary>
        /// Configure the factory for use in a unit testing environment.
        /// </summary>
        public static void ConfigureForTesting()
        {            
            if (_repositoryFactoryType == null)
            {
                _repositoryFactoryType = typeof(RepositoryFactoryNoDb);
            }
            UnitOfWorkNoDb.ClearRepositories();
            GuidHelper.GenerateSequentialGuids = true;
            _isForTesting = true;
        }

        /// <summary>
        /// Create a Service.
        /// </summary>
        public static T CreateService<T>() where T : IService
        {
            return GetChildFactory().CreateService<T>(CreateUnitOfWork());
        }

        /// <summary>
        /// Create a Service.
        /// </summary>
        public static T CreateService<T>(IUnitOfWork unitOfWork) where T : IService
        {
            return GetChildFactory().CreateService<T>(unitOfWork);
        }

        /// <summary>
        /// Create a Service using the DI container.
        /// </summary>
        public static IService CreateService(Type serviceType, IUnitOfWork unitOfWork)
        {
            return GetChildFactory().CreateService(serviceType, unitOfWork);
        }

        /// <summary>
        /// Create a Controller.
        /// </summary>
        /// <param name="controllerType">Type of controller to create.</param>
        public static T CreateController<T>(IUnitOfWork unitOfWork) where T : class
        {
            if (!_isForTesting)
            {
                throw new InvalidOperationException("Controllers cannot be created outside of a unit testing project.");
            }

            return GetChildFactory().CreateController(typeof(T), unitOfWork) as T;
        }

        /// <summary>
        /// Create a disposable factory, used for disposing objects created by the factory.
        /// </summary>
        public static IFactory CreateDisposableFactory()
        {
            IRepositoryFactory _repoFactory = null;
            IServiceFactory _serviceFactory = null;

            if (_repositoryFactoryType != null)
            {
                _repoFactory = (IRepositoryFactory)Activator.CreateInstance(_repositoryFactoryType);
            }

            if (_serviceFactoryType != null)
            {
                _serviceFactory = (IServiceFactory)Activator.CreateInstance(_serviceFactoryType);
            }

            return new DisposableFactory(_repoFactory, _serviceFactory);
        }

        private static IFactory GetChildFactory()
        {
            IFactory factory = _provider?.GetFactory();
            if (factory == null)
            {
                if (_defaultProvider == null)
                {
                    _defaultProvider = new DefaultFactoryProvider();
                }
                return _defaultProvider.GetFactory();
            }
            return factory;
        }

        /// <summary>
        /// A Factory that implements IDisposable, used by the WebApi for each dependency scope.
        /// </summary>
        public class DisposableFactory : IFactory, IDisposable
        {
            private readonly IRepositoryFactory _repositoryFactory = null;
            private readonly IServiceFactory _serviceFactory = null;

            public DisposableFactory(IRepositoryFactory repoFactory, IServiceFactory serviceFactory)
            {
                _repositoryFactory = repoFactory;
                _serviceFactory = serviceFactory;
            }

            /// <summary>
            /// Create a UnitOfWork.
            /// </summary>
            /// <returns></returns>
            public IUnitOfWork CreateUnitOfWork()
            {
                if (_repositoryFactory == null)
                {
                    throw new InvalidOperationException(NOTREGISTERED);
                }
                return _repositoryFactory.CreateUnitOfWork();
            }

            /// <summary>
            /// Create a Repository.
            /// </summary>
            public IRepository<T> CreateRepository<T>() where T : class
            {
                if (_repositoryFactory == null)
                {
                    throw new InvalidOperationException(NOTREGISTERED);
                }
                return _repositoryFactory.CreateRepository<T>();
            }

            /// <summary>
            /// Create a Service.
            /// </summary>
            public T CreateService<T>() where T : IService
            {
                if (_serviceFactory == null)
                {
                    throw new InvalidOperationException(NOTREGISTERED);
                }
                return _serviceFactory.CreateService<T>(CreateUnitOfWork());
            }

            /// <summary>
            /// Create a Service.
            /// </summary>
            public T CreateService<T>(IUnitOfWork unitOfWork) where T : IService
            {
                if (_serviceFactory == null)
                {
                    throw new InvalidOperationException(NOTREGISTERED);
                }
                return _serviceFactory.CreateService<T>(unitOfWork);
            }

            /// <summary>
            /// Create a Service.
            /// </summary>
            public IService CreateService(Type serviceType, IUnitOfWork unitOfWork)
            {
                if (_serviceFactory == null)
                {
                    throw new InvalidOperationException(NOTREGISTERED);
                }
                return _serviceFactory.CreateService(serviceType, unitOfWork);
            }

            /// <summary>
            /// Create a Controller.
            /// </summary>
            /// <param name="controllerType">Type of controller to create.</param>
            public object CreateController(Type controllerType) => CreateController(controllerType, null);

            /// <summary>
            /// Create a Controller.
            /// </summary>
            /// <param name="controllerType">Type of controller to create.</param>
            /// <param name="unitOfWork">Unit of Work.</param>
            public object CreateController(Type controllerType, IUnitOfWork unitOfWork)
            {
                if (_serviceFactory == null || _repositoryFactory == null)
                {
                    throw new InvalidOperationException(NOTREGISTERED);
                }

                return DIContainer.Resolve(controllerType, unitOfWork);
            }

            #region IDisposable Support
            private bool disposedValue = false; // To detect redundant calls

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        _repositoryFactory?.Dispose();
                    }

                    disposedValue = true;
                }
            }

            // This code added to correctly implement the disposable pattern.
            public void Dispose()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);
            }
            #endregion
        }

        /// <summary>
        /// The default factory provider, used outside of an Http context.
        /// </summary>
        public class DefaultFactoryProvider : IFactoryProvider
        {
            private readonly IFactory _factory = null;

            public DefaultFactoryProvider()
            {
                _factory = DDI.Shared.Factory.CreateDisposableFactory();
            }

            public IFactory GetFactory()
            {
                return _factory;
            }
        }

    }


}
