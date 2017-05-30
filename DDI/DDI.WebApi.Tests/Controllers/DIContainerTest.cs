using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared;
using DDI.Shared.Data;
using DDI.Shared.Models.Client.Core;
using DDI.WebApi.Controllers.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.WebApi.Tests.Controllers
{
    /// <summary>
    /// DI Container tests for creating Controllers.
    /// </summary>
    [TestClass]
    public class DIContainerTest : TestBase
    {

        [TestInitialize]
        public override void Initialize() => base.Initialize();

        private const string TESTDESCR = "WebApi | Controllers";

        [TestMethod, TestCategory(TESTDESCR)]
        public void Dependency_Injection_for_Controller()
        {
            var controller = DIContainer.Resolve(typeof(LanguagesController)) as LanguagesController;

            Assert.IsNotNull(controller, "Resolved controller is not null.");
            Assert.IsInstanceOfType(controller, typeof(LanguagesController), "Resolved controller is correct type.");
            Assert.IsNotNull(controller.Service, "Controller Service is not null.");
            Assert.IsInstanceOfType(controller.Service, typeof(IService<Language>), "Controller service is correct type.");
        }

    }
}