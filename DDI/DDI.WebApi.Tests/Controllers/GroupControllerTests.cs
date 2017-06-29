using DDI.Services;
using DDI.Services.General;
using DDI.Services.Security;
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
        private User user;

        [TestInitialize]
        public override void Initialize() => base.Initialize();


        [TestMethod, TestCategory(TESTDESCR)]
        public void GroupController_GetAll()
        {
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(m => m.GetEntities<Group>(null)).Returns(SetupRepo());

            var service = new ServiceBase<Group>(uow.Object);
            var controller = new GroupController(service);

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult result = controller.GetAll();
            var contentResult = result as OkNegotiatedContentResult<IDataResponse>;

            var data = ((contentResult.Content as DataResponse<object>).Data as List<ICanTransmogrify>).Cast<Group>().ToList();
            Assert.IsNotNull(contentResult, "IDataResponse is returned");
            Assert.IsNotNull(contentResult.Content, "IDataResponse is returned with content");
            Assert.AreEqual(data.Count, 3, "Content is the correct size");
            Assert.AreEqual(data[1].DisplayName, "Group 2", "Content is accurate");

        }       

        private IQueryable<Group> SetupRepo()
        {

            user = new User()
            {
                Id = GuidHelper.NewSequentialGuid(),
                Email = "JohnDoe@DDI.org",
                FullName = "John Doe",
            };

            List<Role> roleList = new List<Role>()
            {
                new Role { Name = "Read", Module = "Notes", Id = GuidHelper.NewSequentialGuid()  },
                new Role { Name = "Read/Write", Module = "Settings", Id = GuidHelper.NewSequentialGuid() }
            };

            user.Roles.Add(new UserRole() { UserId = user.Id, RoleId = roleList[0].Id });
            roleList[0].Users.Add(new UserRole { RoleId = roleList[0].Id, UserId = user.Id});

            user.Roles.Add(new UserRole() { UserId = user.Id, RoleId = roleList[1].Id });
            roleList[1].Users.Add(new UserRole { RoleId = roleList[1].Id, UserId = user.Id });

            return new List<Group>()
            {
                new Group { Name = "Group 1", Id = GuidHelper.NewSequentialGuid(), Roles = new List<Role>(), Users = new List<User>() { user } },
                new Group { Name = "Group 2", Id = GuidHelper.NewSequentialGuid(), Roles = roleList },
                new Group { Name = "Group 3", Id = GuidHelper.NewSequentialGuid(), Roles = roleList, Users = new List<User> { user } }

            }
            .AsQueryable<Group>();
        }

    }
}
