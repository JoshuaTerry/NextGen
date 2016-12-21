using DDI.Data;
using DDI.Data.Models.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DDI.WebApi.Tests.Services
{
    [TestClass]
    public class ConstituentServiceTests
    {
        [TestMethod]
        public void Test_ConvertToType()
        {
            var repo = new Mock<IRepository<Constituent>>();             
            //var service = new ConstituentService(repo.Object);   
            //var result = service.ConvertToType<Constituent>("GenderId");
            
           // Assert.IsTrue(result == typeof(Guid?));
        }
    }
}
