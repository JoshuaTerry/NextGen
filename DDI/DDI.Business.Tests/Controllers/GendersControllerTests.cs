using DDI.Business.Services;
using DDI.Data;
using DDI.Data.Models.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace DDI.Business.Tests.Controllers
{
    [TestClass]
    public class GendersControllerTests
    {
        [TestMethod]
        public void GetAllGenders_ReturnsGenderCollection()
        {
            var repo = new Mock<IRepository<Gender>>(); ;
            repo.Setup(r => r.Entities).Returns(SetupRepo());
            var service = new GenericServiceBase<Gender>(repo.Object);
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
