using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDI.Data;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DDI.Shared; 

namespace DDI.WebApi.Tests.Services
{
    [TestClass]
    public class ConstituentServiceTests
    {
        private const string TESTDESCR = "WebApi | Services";

        [TestMethod, TestCategory(TESTDESCR)]
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
                Id = new Guid("E69FC3E8-C5CC-45B0-BA58-BF340A3706BC"),
            });

            return constituents.AsQueryable();
        }
    }
}
