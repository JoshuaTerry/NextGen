using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDI.Data;
using DDI.Data.Models.Client;
using DDI.WebApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DDI.WebApi.Tests.Services
{
    [TestClass]
    public class ConstituentServiceTests
    {
        [TestMethod]
        public void Test_GetConstituentByConstituentNum()
        {
            var repo = new Mock<IRepository<Constituent>>();
            var unitOfWork = new UnitOfWorkNoDb();
            unitOfWork.SetRepository(repo.Object);
            repo.Setup(r => r.Entities).Returns(SetupRepositoryConstituents());

            var service = new ConstituentService(unitOfWork);
            var result = service.GetConstituentByConstituentNum(12345);

            Assert.AreEqual(result.Data, repo.Object.Entities.FirstOrDefault(p => p.ConstituentNumber == 12345));              
            //var result = service.ConvertToType<Constituent>("GenderId");

            // Assert.IsTrue(result == typeof(Guid?));
        }

        private IQueryable<Constituent> SetupRepositoryConstituents()
        {
            var constituents = new List<Constituent>();
            constituents.Add(new Constituent {
                ConstituentNumber = 12345,
                FormattedName = "Mr. John Doe",
                Id = new Guid(),
            });

            return constituents.AsQueryable();
        }
    }
}
