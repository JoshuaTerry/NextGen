using DDI.Services;
using DDI.Services.General;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.Security;
using DDI.Shared.Models.Common;
using DDI.WebApi.Controllers.Core;
using DDI.WebApi.Controllers.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            // end initialize

            IHttpActionResult result = controller.GetAll();
            var contentResult = result as OkNegotiatedContentResult<IDataResponse>;

            var data = ((contentResult.Content as DataResponse<object>).Data as List<ICanTransmogrify>).Cast<Group>().ToList();
            Assert.IsNotNull(contentResult, "IDataResponse is returned");
            Assert.IsNotNull(contentResult.Content, "IDataResponse is returned with content");
            Assert.AreEqual(data.Count, 3, "Content is the correct size");
            Assert.AreEqual(data[1].DisplayName, "Group 2", "Content is accurate");

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void GroupController_AddRolesToGroup_NewRole()
        {
            //initialize
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(m => m.GetEntities<Group>(null)).Returns(SetupRepo());

            IGroupService service = new GroupService(uow.Object);
            var controller = new GroupController(service);
            // end initialize 

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            var group = service.GetAll().Data[0];
            // test cases:
            // add a new role to a group

            string roles = "{ Ids: ['" + GuidHelper.NewSequentialGuid().ToString() + "'] }";

            JObject JRole = JObject.Parse(roles); 

            uow.Setup(m => m.GetById<Group>(group.Id, r => r.Roles)).Returns(group as Group);
            Assert.AreEqual((group as Group).Roles.Count, 0, "No roles to start with");
            IHttpActionResult result = controller.AddRolesToGroup(group.Id, JRole);
            var contentResult = (result as OkNegotiatedContentResult<IDataResponse>).Content as DataResponse<object>;

            Assert.AreEqual(contentResult.IsSuccessful, true,  "Service call was successful");
            Assert.AreEqual(contentResult.TotalResults, 1, "One role was added");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void GroupController_AddRolesToGroup_ExistingRole()
        {
            //initialize
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(m => m.GetEntities<Group>(null)).Returns(SetupRepo());

            var service = new ServiceBase<Group>(uow.Object);
            var controller = new GroupController(service);
            // end initialize 

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            var group = service.GetAll().Data[0];

            Role role = service.GetAll().Data.Cast<Group>().ToList()[1].Roles.ToList()[0];
            // get the Role from the Group from the service
            JObject JRole = JObject.FromObject(role);

            IHttpActionResult result = controller.AddRolesToGroup(group.Id, JRole);

        }

        private IQueryable<Group> SetupRepo()
        {
            List<Role> roleList = new List<Role>()
            {
                new Role { Name = "Read", Module = "Notes", Id = GuidHelper.NewSequentialGuid() },
                new Role { Name = "Read/Write", Module = "Settings", Id = GuidHelper.NewSequentialGuid() }
            };

            return new List<Group>()
            {
                new Group { Name = "Group 1", Id = GuidHelper.NewSequentialGuid(), Roles = new List<Role>() },
                new Group { Name = "Group 2", Id = GuidHelper.NewSequentialGuid(), Roles = roleList },
                new Group { Name = "Group 3", Id = GuidHelper.NewSequentialGuid(), Roles = new List<Role>() }
            }
            .AsQueryable<Group>();
        }

    }
}
