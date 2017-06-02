using System.Collections.Generic;
using System.Linq;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using DDI.WebApi.Controllers.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DDI.WebApi.Tests.Controllers
{
    [TestClass]
    public class GendersControllerTests : TestBase
    {
        private const string TESTDESCR = "WebApi | Controllers";

        [TestInitialize]
        public override void Initialize() => base.Initialize();

        [TestMethod, TestCategory(TESTDESCR)]
        public void GendersController_GetAll()
        {
            var uow = Factory.CreateUnitOfWork();
            uow.CreateRepositoryForDataSource(SetupRepo());

            GendersController controller = CreateController<GendersController>(uow);

            var result = controller.GetAll();

            var content = GetResponse(result);
            Assert.AreEqual(3, content.TotalResults, "Correct # of results returned.");

            var data = GetReponseData(content);
            Assert.AreEqual("Female", data[0].DisplayName);
            Assert.AreEqual("Male", data[1].DisplayName);
            Assert.AreEqual("Unspecified", data[2].DisplayName);

            result = controller.GetAll("", new PageableSearch(null, null, OrderByProperties.DisplayName), "Name,IsMasculine");
            data = GetResponseData(result);

            Assert.AreEqual("Female", data[0].Name);
            Assert.AreEqual(false, data[0].IsMasculine);
            Assert.AreEqual("Male", data[1].Name);
            Assert.AreEqual(true, data[1].IsMasculine);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void GendersController_GetById()
        {
            var uow = Factory.CreateUnitOfWork();
            var genders = SetupRepo();
            uow.CreateRepositoryForDataSource(genders);

            GendersController controller = CreateController<GendersController>(uow);

            var result = controller.GetById(genders.FirstOrDefault(p => p.Name == "Male").Id);

            var data = GetResponseEntity<Gender>(result);
            Assert.AreEqual("Male", data.DisplayName);

            result = controller.GetById(genders.FirstOrDefault(p => p.Name == "Female").Id, "Name");
            var dynamicData = GetResponseEntity<dynamic>(result);
            Assert.AreEqual("Female", dynamicData.Name);            
        }

        private IQueryable<Gender> SetupRepo()
        {
            var list = new List<Gender>();
            list.Add(new Gender { IsMasculine=true, Name="Male", Id = GuidHelper.NewSequentialGuid() });
            list.Add(new Gender { IsMasculine = false, Name = "Female", Id = GuidHelper.NewSequentialGuid() });
            list.Add(new Gender { IsMasculine = null, Name = "Unspecified", Id = GuidHelper.NewSequentialGuid() });

            return list.AsQueryable();
        }
    }
}
