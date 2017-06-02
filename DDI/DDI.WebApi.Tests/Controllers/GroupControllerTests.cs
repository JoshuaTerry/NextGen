﻿using DDI.Services;
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
        public void GroupController_AddUserToGroup()
        {
            throw new NotImplementedException();
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void GroupController_RemoveUserFromGroup()
        {
            throw new NotImplementedException();
        }

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

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            var group = service.GetAll().Data[0];

            Role role = service.GetAll().Data.Cast<Group>().ToList()[1].Roles.ToList()[0];
            string roles = "{ Ids: ['" + role.Id.ToString() + "'] }";
            JObject JRole = JObject.Parse(roles); 

            uow.Setup(m => m.GetById<Group>(group.Id, r => r.Roles)).Returns(group as Group);
            uow.Setup(m => m.GetById<Group>(group.Id)).Returns(group as Group);
            uow.Setup(m => m.GetById<Role>(role.Id)).Returns(role);
            Assert.AreEqual((group as Group).Roles.Count, 0, "No roles to start with");

            IHttpActionResult result = controller.AddRolesToGroup(group.Id, JRole);
            var contentResult = (result as OkNegotiatedContentResult<IDataResponse>).Content as DataResponse<object>;

            Assert.AreEqual(contentResult.IsSuccessful, true,  "Service call was successful");
            Assert.AreEqual((contentResult.Data as Group).Roles.Count, 1, "One role was added");
            Assert.AreEqual((contentResult.Data as Group).Roles.Count, 1, "One role was added");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void GropuController_AddRolesToGroup_DuplicateRole()
        {
            //initialize
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(m => m.GetEntities<Group>(null)).Returns(SetupRepo());

            IGroupService service = new GroupService(uow.Object);
            var controller = new GroupController(service);
            // end initialize 

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            var group = service.GetAll().Data[1];

            Role role = service.GetAll().Data.Cast<Group>().ToList()[1].Roles.ToList()[0];
            // get the Role from the Group from the service

            string roles = "{ Ids: ['" + role.Id.ToString() + "'] }";
            JObject JRole = JObject.Parse(roles);

            uow.Setup(m => m.GetById<Group>(group.Id, r => r.Roles)).Returns(group as Group);
            uow.Setup(m => m.GetById<Role>(role.Id)).Returns(role as Role);
            uow.Setup(m => m.GetById<Group>(group.Id)).Returns(group as Group);
            IHttpActionResult result = controller.AddRolesToGroup(group.Id, JRole);
            //Try to add the existing Role to the Group
            var contentResult = (result as OkNegotiatedContentResult<IDataResponse>).Content as DataResponse<object>;

            Assert.AreEqual(2, (contentResult.Data as Group).Roles.Count,  "Duplicate Role was not added as new role");
            Assert.IsNotNull((contentResult.Data as Group).Roles.ToList().Find(r => r.Name == "Read") , "Role was not changed");

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void GroupController_RemoveRolesFromGroup_RoleIsInGroup()
        {
            //initialize
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(m => m.GetEntities<Group>(null)).Returns(SetupRepo());

            IGroupService service = new GroupService(uow.Object);
            var controller = new GroupController(service);
            // end initialize 

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            Group group = service.GetAll().Data[0] as Group;
            Role role = service.GetAll().Data.Cast<Group>().ToList()[1].Roles.ToList()[0];

            uow.Setup(m => m.GetById<Group>(group.Id, g => g.Roles)).Returns(group);
            uow.Setup(m => m.GetById<Group>(group.Id)).Returns(group);
            Assert.AreEqual(2, group.Roles.Count, "2 Roles in Group at first");

            IHttpActionResult result = controller.RemoveRolesFromGroup(group.Id, role.Id);
            var contentResult = (result as OkNegotiatedContentResult<IDataResponse>).Content as DataResponse<object>;

            Assert.AreEqual(1, (contentResult.Data as Group).Roles.Count, "Role was removed from Group");
            //Assert.AreEqual("Read", (contentResult.Data as Group).Roles[0].Nam, "Role was removed from Group");

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void GroupController_RemoveRolesFromGroup_RoleIsNotInGroup()
        {
            //initialize
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(m => m.GetEntities<Group>(null)).Returns(SetupRepo());

            IGroupService service = new GroupService(uow.Object);
            var controller = new GroupController(service);
            // end initialize 

            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            Group group = service.GetAll().Data[2] as Group;
            Role role = service.GetAll().Data.Cast<Group>().ToList()[1].Roles.ToList()[0];

            uow.Setup(m => m.GetById<Group>(group.Id, g => g.Roles)).Returns(group);
            uow.Setup(m => m.GetById<Group>(group.Id)).Returns(group);

            IHttpActionResult result = controller.RemoveRolesFromGroup(group.Id, role.Id);
            var contentResult = (result as OkNegotiatedContentResult<IDataResponse>).Content as DataResponse<object>;

            Assert.AreEqual(0, (contentResult.Data as Group).Roles.Count, "Group collection is unchanged");
            //Assert.IsTrue(user.Roles.Empty, "Role removed from a group is removed from user in that group")
        }

        private IQueryable<Group> SetupRepo()
        {

            User user = new User()
            {
                Email = "JohnDoe@DDI.org",
                FullName = "John Doe"
            };

            List<Role> roleList = new List<Role>()
            {
                new Role { Name = "Read", Module = "Notes", Id = GuidHelper.NewSequentialGuid() },
                new Role { Name = "Read/Write", Module = "Settings", Id = GuidHelper.NewSequentialGuid() }
            };

            return new List<Group>()
            {
                new Group { Name = "Group 1", Id = GuidHelper.NewSequentialGuid(), Roles = new List<Role>() },
                new Group { Name = "Group 2", Id = GuidHelper.NewSequentialGuid(), Roles = roleList },
                new Group { Name = "Group 3", Id = GuidHelper.NewSequentialGuid(), Roles = new List<Role>(), Users = new List<User> { user } }

            }
            .AsQueryable<Group>();
        }

    }
}
