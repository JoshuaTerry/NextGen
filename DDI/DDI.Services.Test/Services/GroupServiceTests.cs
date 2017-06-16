using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using DDI.Shared;
using DDI.Shared.Models.Client.Security;
using DDI.Services.General;
using Newtonsoft.Json.Linq;

namespace DDI.Services.Test.Services
{
    [TestClass]
    public class GroupServiceTests
    {
        private const string TESTDESCR = "Services | Services";

        [TestMethod, TestCategory(TESTDESCR)]
        public void GroupService_ChangeRoles()
        {
            var uow = new Mock<IUnitOfWork>();
            Guid groupId = Guid.Parse("D7ABB652-B6D7-47AF-A945-B25DA5FD6922");
            uow.Setup(m => m.GetById<Group>(groupId, g => g.Roles, g => g.Users)).Returns(GetGroup());
            uow.Setup(m => m.GetReference(GetGroup(), g => g.Roles)).Returns(GetGroup().Roles);

            var roles = CreateRoles();

            uow.Setup(m => m.GetById<Role>(roles[0].Id)).Returns(roles[0]);
            uow.Setup(m => m.GetById<Role>(roles[1].Id)).Returns(roles[1]);
            uow.Setup(m => m.GetById<Role>(roles[2].Id)).Returns(roles[2]);

            //var service = new Mock<GroupService>(uow.Object);
            //service.Setup(s => s.GetRoleListFromJObject(It.IsAny<JObject>())).Returns(JObjectRoles());
            var service = new GroupService(uow.Object);
            string roleid = "{ Ids: ['82CD5D29-34A6-419A-B93D-CCAF5CB910F6'] }";
            JObject jRole = JObject.Parse(roleid);

            var response = service.UpdateGroupRoles(groupId, jRole);

            Assert.AreEqual(response.Data.Roles.Count(), 1);
            Assert.AreEqual(response.Data.Roles.ToList()[0].Id, Guid.Parse("82cd5d29-34a6-419a-b93d-ccaf5cb910f6"));
            Assert.AreEqual(response.Data.Users.ToList()[0].Roles.ToList()[0].RoleId, Guid.Parse("82cd5d29-34a6-419a-b93d-ccaf5cb910f6"));
        }

         
        private List<Role> JObjectRoles()
        {
            var roles = CreateRoles();
            roles.RemoveAt(2);
            roles.RemoveAt(0);

            return roles;
        }
        private Group GetGroup()
        {
            var roles = CreateRoles();
            var users = CreateUsers(roles); 
            var group = CreateGroup(users);

            users[2].Groups = new List<Group>();
            users[2].Groups.Add(group);
            return group;
        }
        private List<Role> CreateRoles()
        {
            var list = new List<Role>();
            list.Add(new Role { Id = Guid.Parse("985786DA-EF78-4281-937E-581023BB0274") });
            list.Add(new Role { Id = Guid.Parse("82CD5D29-34A6-419A-B93D-CCAF5CB910F6") });
            list.Add(new Role { Id = Guid.Parse("E8E2A595-7313-4897-8036-2CE3906B9455") });
            return list;
        }

        private List<User> CreateUsers(List<Role> roles)
        { 
            var list = new List<User>();
            list.Add(CreateUser(Guid.Parse("32DD86D5-CAAB-42FC-944A-A80B2B0660B4"), roles));
            roles.RemoveAt(0);
            list.Add(CreateUser(Guid.Parse("3D09D94E-6C73-4BB0-AE44-95696A3423F4"), roles));
            roles.RemoveAt(0);
            list.Add(CreateUser(Guid.Parse("A03EDE89-047C-4466-A8B6-EE8EC8960E1E"), roles));
            return list;
        }

        private User CreateUser(Guid id, List<Role> roles)
        {
            var user = new User();
            user.Id = id;
            user.UserName = "Test " + DateTime.Now.Ticks.ToString();
            roles.ForEach(r => user.Roles.Add(new UserRole() { Id = Guid.NewGuid(), UserId = user.Id, RoleId = r.Id }));
            return user;
        }

        private Group CreateGroup(List<User> users)
        {
            var roles = CreateRoles(); 
            roles.RemoveAt(0);
            roles.RemoveAt(0);
            var group = new Group();
            group.Id = Guid.Parse("D7ABB652-B6D7-47AF-A945-B25DA5FD6922");
            //Group should have this Role Id "E8E2A595-7313-4897-8036-2CE3906B9455"
            roles.ForEach(r => group.Roles.Add(r));
            group.Users.Add(users[2]);

            return group;
        }
    }
}
