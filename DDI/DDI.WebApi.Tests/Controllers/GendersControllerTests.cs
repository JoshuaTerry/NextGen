using DDI.Services;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.WebApi.Controllers.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace DDI.WebApi.Tests.Controllers
{
    [TestClass]
    public class GendersControllerTests
    {
        private const string TESTDESCR = "WebApi | Controllers";
         
        [TestMethod, TestCategory(TESTDESCR)]
        public void GetAllGenders_ReturnAll()
        {
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(m => m.GetEntities<Gender>(null)).Returns(SetupRepo());

            var service = new ServiceBase<Gender>(uow.Object);
            var controller = new GendersController(service);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult result = controller.GetAll();
            var contentResult = result as OkNegotiatedContentResult<IDataResponse>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(((contentResult.Content as DataResponse<object>).Data as List<System.Dynamic.ExpandoObject>).Count, 3);
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
