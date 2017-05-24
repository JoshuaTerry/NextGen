using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Business.Core;
using DDI.Business.GL;
using DDI.Business.Tests.GL.DataSources;
using DDI.Shared;
using DDI.Shared.Enums.GL;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.GL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Business.Tests.Core
{
    /// <summary>
    /// DI Container tests for creating Business Logic.
    /// </summary>
    [TestClass]
    public class DIContainerTest : TestBase
    {

        private const string TESTDESCR = "Business | Core";

        [TestInitialize]
        public void Initialize()
        {
            Factory.ConfigureForTesting();
        }
        
        [TestMethod, TestCategory(TESTDESCR)]
        public void Dependency_Injection_for_Business_Logic()
        {
            DIContainer.Register(typeof(TransactionLogic));

            // Create a UnitOfWork and initalize a Transaction repository.
            IUnitOfWork uow = Factory.CreateUnitOfWork();
            uow.CreateRepositoryForDataSource(new List<Transaction>());

            var logic = DIContainer.Resolve(typeof(TransactionLogic), uow);
            Assert.IsNotNull(logic, "Resolved object is not null.");
            Assert.IsInstanceOfType(logic, typeof(TransactionLogic), "Resolved object is correct type.");
            Assert.IsNotNull(((TransactionLogic)logic).UnitOfWork, "Business logic has a valid UnitOfWork");
        }

    }
}
