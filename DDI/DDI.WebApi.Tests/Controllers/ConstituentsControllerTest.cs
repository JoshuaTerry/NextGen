using System;
using System.Web.Http;
using DDI.WebApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.WebApi.Tests.Controllers
{
    [TestClass]
    public class ConstituentsControllerTest
    {
        private const string TESTDESCR = "WebApi | Controllers";

        [TestMethod, TestCategory(TESTDESCR)]
        [Ignore]
        public void Get()
        {
            // Arrange
            ConstituentsController controller = new ConstituentsController();

            // Act
            //IHttpActionResult result = controller.GetConstituents();

            // Assert
            
        }

        [TestMethod, TestCategory(TESTDESCR)]
        [Ignore]
        public void GetById()
        {
            // Arrange
            ConstituentsController controller = new ConstituentsController();

            // Act
            //IHttpActionResult result = controller.GetConstituentById(new Guid());

            // Assert
            
        }

        [TestMethod, TestCategory(TESTDESCR)]
        [Ignore]
        public void Post()
        {
            // Arrange
            ConstituentsController controller = new ConstituentsController();

            // Act
            //controller.Post("value");

            // Assert
        }

        [TestMethod, TestCategory(TESTDESCR)]
        [Ignore]
        public void Put()
        {
            // Arrange
            ConstituentsController controller = new ConstituentsController();

            // Act
            //controller.Put(new Guid(), "value");

            // Assert
        }

        [TestMethod, TestCategory(TESTDESCR)]
        [Ignore]
        public void Delete()
        {
            // Arrange
            ConstituentsController controller = new ConstituentsController();

            // Act
            controller.Delete(new Guid());

            // Assert
        }
    }
}
