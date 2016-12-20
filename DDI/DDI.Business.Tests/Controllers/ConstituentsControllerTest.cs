using DDI.Business.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Http;

namespace DDI.Business.Tests.Controllers
{
    [TestClass]
    public class ConstituentsControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            ConstituentsController controller = new ConstituentsController();

            // Act
            IHttpActionResult result = controller.GetConstituents();

            // Assert
            
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            ConstituentsController controller = new ConstituentsController();

            // Act
            IHttpActionResult result = controller.GetConstituentById(new Guid());

            // Assert
            
        }

        [TestMethod]
        public void Post()
        {
            // Arrange
            ConstituentsController controller = new ConstituentsController();

            // Act
            //controller.Post("value");

            // Assert
        }

        [TestMethod]
        public void Put()
        {
            // Arrange
            ConstituentsController controller = new ConstituentsController();

            // Act
            //controller.Put(new Guid(), "value");

            // Assert
        }

        [TestMethod]
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
