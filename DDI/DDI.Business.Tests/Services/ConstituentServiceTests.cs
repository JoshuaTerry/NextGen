using DDI.Business.Services;
using DDI.Data;
using DDI.Data.Models.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DDI.Business.Tests.Services
{
    [TestClass]
    public class ConstituentServiceTests
    {
        [TestMethod]
        public void Test_ConvertToType()
        {
            var repo = new Mock<IRepository<Constituent>>();             
            var service = new ConstituentService();   
            var result = service.ConvertToType("GenderId", typeof(Constituent));
            
            Assert.IsTrue(result == typeof(Guid?));
        }
    }
}
