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
    public class ConstituentControllerTest
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
            IHttpActionResult result = controller.GetConstituentById(5);

            // Assert
            
        }

        [TestMethod]
        public void Post()
        {
            // Arrange
            ConstituentsController controller = new ConstituentsController();

            // Act
            controller.Post("value");

            // Assert
        }

        [TestMethod]
        public void Put()
        {
            // Arrange
            ConstituentsController controller = new ConstituentsController();

            // Act
            controller.Put(5, "value");

            // Assert
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            ConstituentsController controller = new ConstituentsController();

            // Act
            controller.Delete(5);

            // Assert
        }
    }
}
