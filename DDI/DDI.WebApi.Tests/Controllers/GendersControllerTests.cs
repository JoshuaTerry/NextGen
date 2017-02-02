using System.Collections.Generic;
using System.Linq;
using DDI.Data;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DDI.Shared;

namespace DDI.WebApi.Tests.Controllers
{
    [TestClass]
    public class GendersControllerTests
    {
        private const string TESTDESCR = "WebApi | Controllers";

        [TestMethod, TestCategory(TESTDESCR)]
        public void GetAllGenders_ReturnsGenderCollection()
        {
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IRepository<Gender>>();  
            repo.Setup(r => r.GetEntities(null)).Returns(SetupRepo());
            uow.Setup(m => m.GetRepository<Gender>()).Returns(repo.Object);
            var service = new ServiceBase<Gender>(uow.Object);
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
