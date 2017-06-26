using DDI.Services;
using DDI.Services.General;
using DDI.Services.Security;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Client.Security;
using DDI.Shared.Models.Common;
using DDI.WebApi.Controllers.Core;
using DDI.WebApi.Controllers.CRM;
using DDI.WebApi.Controllers.General;
using DDI.WebApi.Models.BindingModels;
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
    public class UserControllerTests : TestBase
    {
        private const string TESTDESCR = "WebApi | Controllers";
        private User user;

        [TestInitialize]
        public override void Initialize() => base.Initialize();


        [TestMethod, TestCategory(TESTDESCR)]
        public void UserController_GetAll() //NEW//
        {
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(m => m.GetEntities<User>(null)).Returns(SetupRepo());

            var service = Factory.CreateService<IService<UserService>>() as UserService;
            var controller = new UsersController(service);

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult result = controller.Get();
            var contentResult = result as OkNegotiatedContentResult<IDataResponse>;

            var data = ((contentResult.Content as DataResponse<object>).Data as List<ICanTransmogrify>).Cast<User>().ToList();
            Assert.IsNotNull(contentResult, "IDataResponse is returned");
            Assert.IsNotNull(contentResult.Content, "IDataResponse is returned with content");
            Assert.AreEqual(data.Count, 3, "Content is the correct size");
            Assert.AreEqual(data[0].FullName, "John Doe", "Content is accurate");

        }


        [TestMethod, TestCategory(TESTDESCR)]
        public void UserController_EditUser()
        {
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(m => m.GetEntities<User>(null)).Returns(SetupRepo());
            
            UserService service = new UserService(uow.Object);
            var controller = new UsersController(service);

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            User user = service.GetAll().Data.Cast<User>().ToList()[0];
            

            //change default business unit
            BusinessUnit businessUnit = service.GetAll().Data.Cast<User>().ToList()[2].DefaultBusinessUnit;

            //change list of business units
            List<BusinessUnit> businessUnits = service.GetAll().Data.Cast<User>().ToList()[2].BusinessUnits.ToList();

            //change group
            Group group = service.GetAll().Data.Cast<User>().ToList()[1].Groups.ToList()[0];

            //change constituent
            Constituent constituent = service.GetAll().Data.Cast<User>().ToList()[1].Constituent;

            string item = "{ FullName: '" + user.FullName
                + "', UserName: '" + user.UserName
                + "', Email: '" + user.Email
                + "', DefaultBusinessUnitId: '" + businessUnit.Id
                + "', ConstituentId: '" + constituent.Id
                + "', GroupId: '" + group.Id
                + "', BusinessUnitIds: ['" + businessUnits[0].Id + "','" + businessUnits[1].Id + "']"
                + ", IsActive: '" + true + "' }";
            JObject jUser = JObject.Parse(item);

            uow.Setup(m => m.GetById<User>(user.Id, u => u.Groups, u => u.Roles)).Returns(user as User);
            uow.Setup(m => m.GetById<Group>(group.Id, u => u.Users, u => u.Roles)).Returns(group as Group);
            uow.Setup(m => m.GetById<User>(user.Id, r => r.BusinessUnits)).Returns(user as User);
            uow.Setup(m => m.GetById<BusinessUnit>(businessUnit.Id)).Returns(businessUnit as BusinessUnit);

            IHttpActionResult result = controller.Patch(user.Id, jUser);
            var contentResult = (result as OkNegotiatedContentResult<IDataResponse>).Content as DataResponse<object>;

            Assert.AreEqual(contentResult.IsSuccessful, true, "Service call was successful");
            Assert.AreEqual((contentResult.Data as User).Roles.Count, 1, "User now has one role");
            Assert.AreEqual((contentResult.Data as User).BusinessUnits.Count, 2, "User now has two business units");
        }


        private IQueryable<User> SetupRepo()
        {
            List<BusinessUnit> businessUnits = new List<BusinessUnit>()
            {
                new BusinessUnit() { Name = "ABC Unit", Code = "ABC", Id = GuidHelper.NewSequentialGuid() },
                new BusinessUnit() { Name = "DEF Unit", Code = "DEF", Id = GuidHelper.NewSequentialGuid() },
                new BusinessUnit() { Name = "GHI Unit", Code = "GHI", Id = GuidHelper.NewSequentialGuid() }
            };

            List<Constituent> constituents = new List<Constituent>()
            {
                new Constituent() { Name = "John Doe", Id = GuidHelper.NewSequentialGuid() },
                new Constituent() { Name = "Jack Sprat", Id = GuidHelper.NewSequentialGuid() },
                new Constituent() { Name = "Vanilla Ice", Id = GuidHelper.NewSequentialGuid() }
            };

            List<Role> roleList = new List<Role>()
            {
                new Role { Name = "Read", Module = "Notes", Id = GuidHelper.NewSequentialGuid()  },
                new Role { Name = "Read/Write", Module = "Settings", Id = GuidHelper.NewSequentialGuid() }
            };

            List<Group> groups = new List<Group>()
            {
                new Group() { Name = "Test Group", Id = GuidHelper.NewSequentialGuid(), Roles = new List<Role> { roleList[0], roleList[1] } },
                new Group() { Name = "Second Group", Id = GuidHelper.NewSequentialGuid(), Roles = new List<Role> { roleList[0] } },
                new Group() { Name = "3rd Group", Id = GuidHelper.NewSequentialGuid(), Roles = new List<Role> { roleList[1] } }
            };

            List<User> users = new List<User>()
            {
                new User { Id = GuidHelper.NewSequentialGuid(), FullName = "John Doe", Email = "JDoe@DDI.org", DefaultBusinessUnit = businessUnits[0], BusinessUnits = new List<BusinessUnit> { businessUnits[0], businessUnits[1], businessUnits[2] }, Constituent = constituents[0], Groups = new List<Group> { groups[0] } },
                new User { Id = GuidHelper.NewSequentialGuid(), FullName = "Jack Sprat", Email = "jsprat@DDI.org", DefaultBusinessUnit = businessUnits[1], BusinessUnits = new List<BusinessUnit> { businessUnits[1] }, Constituent = constituents[1], Groups = new List<Group> { groups[1] } },
                new User { Id = GuidHelper.NewSequentialGuid(), FullName = "Vanilla Ice", Email = "vice@DDI.org", DefaultBusinessUnit = businessUnits[2], BusinessUnits = new List<BusinessUnit> { businessUnits[2], businessUnits[1] }, Constituent = constituents[2], Groups = new List<Group> { groups[2] } }
            };

            users[0].Roles.Add(new UserRole() { UserId = users[0].Id, RoleId = roleList[0].Id, Id = GuidHelper.NewSequentialGuid()});
            roleList[0].Users.Add(new UserRole { RoleId = roleList[0].Id, UserId = users[0].Id });

            users[0].Roles.Add(new UserRole() { UserId = users[0].Id, RoleId = roleList[1].Id });
            roleList[1].Users.Add(new UserRole { RoleId = roleList[1].Id, UserId = users[0].Id });

            groups[0].Users = new List<User>();
            groups[0].Users.Add(users[0]);
            groups[1].Users = new List<User>();
            groups[1].Users.Add(users[1]);
            groups[2].Users = new List<User>();
            groups[2].Users.Add(users[2]);
            
            return users.AsQueryable<User>();
        }

    }
}
