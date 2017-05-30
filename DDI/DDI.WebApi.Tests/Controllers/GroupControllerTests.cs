using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.Security;
using DDI.WebApi.Controllers.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var uow = Factory.CreateUnitOfWork();
            uow.CreateRepositoryForDataSource(SetupRepo());

            GroupController controller = CreateController<GroupController>(uow);

            var result = controller.GetAll();

            var content = GetResponse(result);
            Assert.AreEqual(3, content.TotalResults, "Correct # of results returned.");

            var data = GetReponseData(content);
            Assert.AreEqual("Group 1", data[0].DisplayName);

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void GroupController_AddRolesToGroup()
        {

            // test cases:
            // add a new role to a group
            // add an existing role to a group
            // add an empty collection to a group (?)

        }


        private IQueryable<Group> SetupRepo()
        {
            var list = new List<Group>();
            list.Add(new Group { Name = "Group 1", Id = GuidHelper.NewSequentialGuid() });
            list.Add(new Group { Name = "Group 2", Id = GuidHelper.NewSequentialGuid() });
            list.Add(new Group { Name = "Group 3", Id = GuidHelper.NewSequentialGuid() });

            return list.AsQueryable();
        }
    }
}
