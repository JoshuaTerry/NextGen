using System;
using DDI.Services.Extensions;
using DDI.Services.Search;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Services.Extensions.Test
{
    [TestClass]
    public class PageableSearchExtensionsTest
    {
        private const string TESTDESCR = "Services | Extensions";

        [TestMethod, TestCategory(TESTDESCR)]
        public void Should_ReturnPreviousPageLink()
        {
            ConstituentSearch search = new ConstituentSearch
            {
                Name = "Trick",
                Limit = 15,
                Offset = 2
            };
            var result = search.ForPreviousPage();
            Assert.AreEqual(1, result.Offset.Value);
            Assert.AreEqual("Trick", result.Name);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void When_GettingPreviousPageOnFirstPage_Should_ReturnNull()
        {
            ConstituentSearch search = new ConstituentSearch
            {
                Name = "Trick",
                Limit = 15,
                Offset = 0
            };
            var result = search.ForPreviousPage();
            Assert.IsNull(result);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void Should_ReturnNextPageLink()
        {
            ConstituentSearch search = new ConstituentSearch
            {
                Name = "Trick",
                Limit = 15,
                Offset = 2
            };
            var result = search.ForNextPage();
            Assert.AreEqual(3, result.Offset.Value);
            Assert.AreEqual("Trick", result.Name);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void When_GettingNextPageOnLastPage_Should_ReturnNull()
        {
            ConstituentSearch search = new ConstituentSearch
            {
                Name = "Trick",
                Limit = 15,
                Offset = 2
            };
            var result = search.ForNextPage(2);
            Assert.IsNull(result);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void When_GettingNextPagePastLastPage_Should_ReturnNull()
        {
            ConstituentSearch search = new ConstituentSearch
            {
                Name = "Trick",
                Limit = 15,
                Offset = 3
            };
            var result = search.ForNextPage(2);
            Assert.IsNull(result);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void Should_ReturnFirstPageLink()
        {
            ConstituentSearch search = new ConstituentSearch
            {
                Name = "Trick",
                Limit = 15,
                Offset = 2
            };
            var result = search.ForFirstPage();
            Assert.AreEqual(0, result.Offset.Value);
            Assert.AreEqual("Trick", result.Name);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void Should_ReturnLastPageLink()
        {
            ConstituentSearch search = new ConstituentSearch
            {
                Name = "Trick",
                Limit = 15,
                Offset = 2
            };
            var result = search.ForLastPage(5);
            Assert.AreEqual(4, result.Offset.Value);
            Assert.AreEqual("Trick", result.Name);
        }
    }
}
