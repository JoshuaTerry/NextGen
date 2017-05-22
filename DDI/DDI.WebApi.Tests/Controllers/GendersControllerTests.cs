using System.Collections.Generic;
using System.Linq;
using DDI.Services;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DDI.WebApi.Tests.Controllers
{
    [TestClass]
    public class GendersControllerTests
    {
        private const string TESTDESCR = "WebApi | Controllers";

        [TestInitialize]
        public void Initialize()
        {
            Factory.RegisterServiceFactory<ServiceFactory>();
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void GetAllGenders_ReturnsGenderCollection()
        {
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(m => m.GetEntities<Gender>(null)).Returns(SetupRepo());
            var service = Factory.CreateService<ServiceBase<Gender>>(uow.Object);
            var result = service.GetAll();

            Assert.IsTrue(result.Data.Count == 3);
        }
        private IQueryable<Gender> SetupRepo()
        {
            var list = new List<Gender>();
            list.Add(new Gender { IsMasculine=true, Name="Male" });
            list.Add(new Gender { IsMasculine = false, Name = "Female" });
            list.Add(new Gender { IsMasculine = null, Name = "Unspecified" });

            var response = list.AsQueryable<Gender>();
            return response;
        }
    }
}
