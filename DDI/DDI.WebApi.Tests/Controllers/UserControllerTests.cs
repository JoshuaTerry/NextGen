using DDI.Services.Security;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Client.Security;
using DDI.WebApi.Controllers.General;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace DDI.WebApi.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests : TestBase
    {
        private const string TESTDESCR = "WebApi | Controllers";

        [TestInitialize]
        public override void Initialize() => base.Initialize();
            
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
