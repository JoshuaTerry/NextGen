using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using DDI.Business.CRM;
using DDI.Business.Helpers;
using DDI.Data;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace DDI.Business.Tests.Helpers
{
    [TestClass]
    public class BusinessLogicHelperTest
    {

        private const string TESTDESCR = "Business | Helpers";

        [TestMethod, TestCategory(TESTDESCR)]
        public void BusinessLogicHelper_GetBusinessLogicTyped()
        {
            var unitOfWork = new UnitOfWorkNoDb();
            unitOfWork.SetRepository(new RepositoryNoDb<Constituent>(new List<Constituent>().AsQueryable()));

            IEntityLogic logic = BusinessLogicHelper.GetBusinessLogic<Constituent>(unitOfWork);
            Assert.IsInstanceOfType(logic, typeof(ConstituentLogic), "Business logic for Constituent is ConstituentLogic.");

            logic = BusinessLogicHelper.GetBusinessLogic<LogEntry>(unitOfWork);
            Assert.IsInstanceOfType(logic, typeof(EntityLogicBase), "Business logic for LogEntry is EntityLogicBase.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void BusinessLogicHelper_GetBusinessLogic()
        {
            var unitOfWork = new UnitOfWorkNoDb();
            unitOfWork.SetRepository(new RepositoryNoDb<Constituent>(new List<Constituent>().AsQueryable()));

            IEntityLogic logic = BusinessLogicHelper.GetBusinessLogic(unitOfWork, typeof(Constituent));
            Assert.IsInstanceOfType(logic, typeof(ConstituentLogic), "Business logic for Constituent is ConstituentLogic.");

            logic = BusinessLogicHelper.GetBusinessLogic(unitOfWork, typeof(LogEntry));
            Assert.IsInstanceOfType(logic, typeof(EntityLogicBase), "Business logic for LogEntry is EntityLogicBase.");
        }
        [TestMethod, TestCategory(TESTDESCR)]
        public void BusinessLogicHelper_TimeInitialization()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            BusinessLogicHelper.ForceInitialize();
            stopwatch.Stop();

            // Note:  If this test fails, reflection logic in BusinessLogicHelper.Initialize is taking too long, and 
            // it may be necessary to hard-code the mapping of entity types to business logic types.

            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 20, "BusinessLogicHelper initialized in < 20 ms");

        }
    }
    }
