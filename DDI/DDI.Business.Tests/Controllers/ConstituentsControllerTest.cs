using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DDI.Business;
using DDI.Business.Controllers;

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
