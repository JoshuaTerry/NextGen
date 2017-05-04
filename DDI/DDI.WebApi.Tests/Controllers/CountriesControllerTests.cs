using DDI.Services;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Common;
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
    public class CountriesControllerTests
    {
        private const string TESTDESCR = "WebApi | Controllers";

        [TestMethod, TestCategory(TESTDESCR)]
        public void GetAllCountriesnAll()
        {
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(m => m.GetEntities<Country>(null)).Returns(SetupRepo());

            var service = new ServiceBase<Country>(uow.Object);
            var controller = new CountriesController(service);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            IHttpActionResult result = controller.GetAll();
            var contentResult = result as OkNegotiatedContentResult<IDataResponse>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(((contentResult.Content as DataResponse<object>).Data as List<DDI.Shared.Models.ICanTransmogrify>).Count, 3);
        }
        private IQueryable<Country> SetupRepo()
        {
            var list = new List<Country>();
            list.Add(new Country { CountryCode = "US", Description = "Merica" }  );
            list.Add(new Country { CountryCode = "CA", Description = "Canada" });
            list.Add(new Country { CountryCode = "Mx", Description = "Mexico" });

            var response = list.AsQueryable<Country>();
            return response;
        }
    }
}
