using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business;
using DDI.Business.Core;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Services.Test
{
    /// <summary>
    /// DI Container tests for creating Services.
    /// </summary>
    [TestClass]
    public class DIContainerTest : TestBase
    {

        private const string TESTDESCR = "Service | Core";

        [TestInitialize]
        public void Initialize()
        {
            Factory.ConfigureForTesting();
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void Dependency_Injection_for_Service()
        {
            DIContainer.Register(typeof(TestService));
            DIContainer.Register(typeof(ServiceBase<Language>), typeof(TestService));
            DIContainer.Register(typeof(IService<Language>), typeof(TestService));

            // Create a UnitOfWork and initalize a Language repository.
            IUnitOfWork uow = Factory.CreateUnitOfWork();
            uow.CreateRepositoryForDataSource(new List<Language>());

            // Create the service via a concrete class.
            TestService service = DIContainer.Resolve(typeof(TestService), uow) as TestService;
            Assert.IsNotNull(service, "Resolved object is not null.");
            Assert.IsInstanceOfType(service, typeof(TestService), "Resolved object is correct type.");
            Assert.IsNotNull(service.UnitOfWork, "Service has a valid UnitOfWork.");
            Assert.IsNotNull(service.Repository, "Service has a valid Repository.");
            Assert.IsInstanceOfType(service.Repository, typeof(IRepository<Language>), "Service repository is correct type.");
            Assert.IsNotNull(service.Logic, "Service has valid business logic.");

            // Create the service via a ServiceBase class.
            service = DIContainer.Resolve(typeof(ServiceBase<Language>), uow) as TestService;
            Assert.IsNotNull(service, "Service created via ServiceBase");

            // Create the service via a IService<> interface.
            service = DIContainer.Resolve(typeof(IService<Language>), uow) as TestService;
            Assert.IsNotNull(service, "Service created via interface");

            // Create the service via an interface resolved via a function.
            service = DIContainer.Resolve(typeof(TestInterface), uow, p => p == typeof(TestInterface) ? typeof(TestService) : null) as TestService;
            Assert.IsNotNull(service, "Service created via resolved interface.");
        }
        
        private class TestService : ServiceBase<Language>
        {
            public IRepository<Language> Repository { get; set; }
            public IEntityLogic Logic { get; set; }

            public TestService(IUnitOfWork unitOfWork, IRepository<Language> repository,  EntityLogicBase<Language> logic) : base(unitOfWork)
            {
                Repository = repository;
                Logic = logic;
            }
        }

        private interface TestInterface
        {

        }
    }
}
