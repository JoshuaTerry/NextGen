using System;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;
using System.Web.Routing;
using DDI.Services.Search;
using DDI.Shared;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DDI.WebApi.Tests.Helpers
{
    [TestClass]
    public class PaginationTests
    {
        private const string TESTDESCR = "WebApi | Helpers";

        [TestMethod, TestCategory(TESTDESCR)]
        public void Should_CreatePaginationHeaderInfo()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituent,It.IsAny<IPageable>())).Returns("TEST").Verifiable();
            var search = new ConstituentSearch
            {
                Name = "Danger",
                Limit = 15,
                Offset = 3
            };
            var target = new Pagination();
            var result = target.CreatePaginationHeader(urlHelperMock.Object, search, 100, RouteNames.Constituent);
            urlHelperMock.Verify();
            Assert.AreEqual(15, result.PageSize);
            Assert.AreEqual(4, result.CurrentPage);
            Assert.AreEqual(7, result.TotalPages);
            Assert.AreEqual(100, result.TotalCount);
        }
    }
}
