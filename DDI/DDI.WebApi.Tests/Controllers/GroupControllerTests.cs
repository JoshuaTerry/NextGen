using DDI.Services;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.Security;
using DDI.Shared.Models.Common;
using DDI.WebApi.Controllers.Core;
using DDI.WebApi.Controllers.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace DDI.WebApi.Tests.Controllers
{
    [TestClass]
    public class GroupControllerTests : TestBase
    {
        private const string TESTDESCR = "WebApi | Controllers";

        [TestInitialize]
        public override void Initialize() => base.Initialize();

        [TestMethod, TestCategory(TESTDESCR)]
        public void GroupController_GetAll()
        {
            //initialize
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(m => m.GetEntities<Group>(null)).Returns(SetupRepo());

            var service = new ServiceBase<Group>(uow.Object);
            var controller = new GroupController(service);
            // end initialize 

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult result = controller.GetAll();
            var contentResult = result as OkNegotiatedContentResult<IDataResponse>;

            //return (dataResponse.Data as IList<ExpandoObject>).Cast<dynamic>().ToList();
            var data = ((contentResult.Content as DataResponse<object>).Data as List<ICanTransmogrify>).Cast<Group>().ToList();
            Assert.IsNotNull(contentResult, "IDataResponse is returned");
            Assert.IsNotNull(contentResult.Content, "IDataResponse is returned with content");
            Assert.AreEqual(((contentResult.Content as DataResponse<object>).Data as List<ICanTransmogrify>).Count, 3, "Content is the correct size");

            Assert.AreEqual(data[1].DisplayName, "Group 2", "Content is accurate");

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void GroupController_AddRolesToGroup()
        {

            //initialize
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(m => m.GetEntities<Group>(null)).Returns(SetupRepo());

            var service = new ServiceBase<Group>(uow.Object);
            var controller = new GroupController(service);
            // end initialize 


            // test cases:
            // add a new role to a group

            // add an existing role to a group
            // add an empty collection to a group (?)

        }


        private IQueryable<Group> SetupRepo()
        {
            return new List<Group>()
            {
                new Group { Name = "Group 1", Id = GuidHelper.NewSequentialGuid() },
                new Group { Name = "Group 2", Id = GuidHelper.NewSequentialGuid() },
                new Group { Name = "Group 3", Id = GuidHelper.NewSequentialGuid() }
            }
            .AsQueryable<Group>();
        }
    }
}
