using System;
using System.Collections.Generic;
using System.Web.Http.Routing;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DDI.WebApi.Tests.Helpers
{
    [TestClass]
    public class DynamicTransmogrifierTests
    {
        [TestMethod]
        public void Should_ReturnIdenticalDataResponseFieldsOtherThanData()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituents, null)).Returns("TEST").Verifiable();
            var peytonIsTheGreatest = "Peyton is the greatest!!!";
            var totalResults = 18;
            var theGreatest = "The Greatest";
            var constituent = new Constituent
            {
                FirstName = "Peyton"
            };
            var errorMessages = new List<string>
            {
                theGreatest
            };
            var verboseErrorMessages = new List<string>
            {
                peytonIsTheGreatest
            };
            DataResponse<Constituent> initialResponse = new DataResponse<Constituent>
            {
                Data = constituent,
                ErrorMessages = errorMessages,
                IsSuccessful = true,
                TotalResults = totalResults,
                VerboseErrorMessages = verboseErrorMessages
            };
            var target = new DynamicTransmogrifier();
            var results = target.ToDynamicResponse(initialResponse, urlHelperMock.Object);
            Assert.AreEqual(errorMessages, results.ErrorMessages);
            Assert.AreEqual(true, results.IsSuccessful);
            Assert.AreEqual(totalResults, results.TotalResults);
            Assert.AreEqual(verboseErrorMessages, results.VerboseErrorMessages);
        }

        [TestMethod]
        public void When_ThereIsNoFields_Should_ReturnFullObject()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituents, null)).Returns("TEST").Verifiable();
            Constituent constituent = new Constituent
            {
                FirstName = "Jim",
                LastName = "Bob",
            };
            var target = new DynamicTransmogrifier();
            var result = target.ToDynamicObject(constituent, urlHelperMock.Object, "", false);
            Assert.IsTrue(result.FirstName == "Jim");
            Assert.IsTrue(result.LastName == "Bob");
            Assert.IsTrue(result.MiddleName == null);
        }

        [TestMethod]
        public void When_ThereIsNoFieldsButLinks_Should_ReturnFullObjectWithLinks()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituents, null)).Returns("TEST").Verifiable();
            Constituent constituent = new Constituent
            {
                FirstName = "Jim",
                LastName = "Bob",
            };
            var target = new DynamicTransmogrifier();
            var result = target.ToDynamicObject(constituent, urlHelperMock.Object, "", true);
            Assert.IsTrue(result.FirstName == "Jim");
            Assert.IsTrue(DoesFieldExist(result, "FirstName"));
            Assert.IsTrue(DoesFieldExist(result, "LastName"));
            Assert.IsTrue(DoesFieldExist(result, "Links"));
        }

        [TestMethod]
        public void When_ThereIsNullFieldsButLinks_Should_ReturnFullObjectWithLinks()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituents, null)).Returns("TEST").Verifiable();
            Constituent constituent = new Constituent
            {
                FirstName = "Jim",
                LastName = "Bob",
            };
            var target = new DynamicTransmogrifier();
            var result = target.ToDynamicObject(constituent, urlHelperMock.Object, null, true);
            Assert.IsTrue(result.FirstName == "Jim");
            Assert.IsTrue(DoesFieldExist(result, "FirstName"));
            Assert.IsTrue(DoesFieldExist(result, "LastName"));
            Assert.IsTrue(DoesFieldExist(result, "Links"));
        }

        [TestMethod]
        public void When_ThereIsOneConstituent_Should_AddLinksToIt()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituents, null)).Returns("TEST").Verifiable();
            Constituent constituent = new Constituent
            {
                FirstName = "Jim",
                LastName = "Bob",
                MiddleName = "Jane"
            };
            var target = new DynamicTransmogrifier();
            var result = target.ToDynamicObject(constituent, urlHelperMock.Object, "FirstName", true);
            Assert.IsTrue(result.FirstName == "Jim");
            Assert.IsTrue(DoesFieldExist(result, "FirstName"));
            Assert.IsFalse(DoesFieldExist(result, "LastName"));
            Assert.IsFalse(DoesFieldExist(result, "MiddleName"));
            Assert.IsTrue(DoesFieldExist(result, "Links"));
        }

        [TestMethod]
        public void When_ThereIsAListOfConstituents_Should_AddLinksToIt()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituents, null)).Returns("TEST").Verifiable();
            List<Constituent> constituents = new List<Constituent> {
                new Constituent
                {
                    FirstName = "Jim",
                    LastName = "Bob",
                    MiddleName = "Jane"
                },
                new Constituent
                {
                    FirstName = "Sally",
                    LastName = "Fields",
                    MiddleName = "Jane"
                }
            };
            var target = new DynamicTransmogrifier();
            var result = target.ToDynamicList(constituents, urlHelperMock.Object, "FirstName,LastName", true);
            Assert.IsTrue(DoesFieldExist(result[1], "FirstName"));
            Assert.IsTrue(DoesFieldExist(result[1], "LastName"));
            Assert.IsFalse(DoesFieldExist(result[1], "MiddleName"));
            Assert.IsTrue(DoesFieldExist(result[1], "Links"));
        }

        [TestMethod]
        public void When_ThereIsASubProperty_Should_ReturnIt()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituents, null)).Returns("TEST").Verifiable();
            Constituent constituent = new Constituent
            {
                FirstName = "Jim",
                LastName = "Bob",
                ConstituentAddresses = new List<ConstituentAddress>
                {
                    new ConstituentAddress
                    {
                        Address = new Address
                        {
                            City = "Bham",
                            PostalCode = "12345"
                        }
                    }
                }
            };
            var target = new DynamicTransmogrifier();
            var result = target.ToDynamicObject(constituent, urlHelperMock.Object, "FirstName,ConstituentAddresses.Address.City,ConstituentAddresses.Address.PostalCode", false);
            Assert.IsTrue(result.ConstituentAddresses[0].Address.City == "Bham");
            Assert.IsTrue(result.ConstituentAddresses[0].Address.PostalCode == "12345");
            Assert.IsTrue(result.FirstName == "Jim");
            Assert.IsTrue(DoesFieldExist(result, "FirstName"));
            Assert.IsFalse(DoesFieldExist(result, "LastName"));
        }

        [TestMethod]
        public void When_ThereIsAreSubPropertiesAndLinksRequested_Should_ReturnLinksForEveryObject()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituents, It.IsAny<object>())).Returns("api/v1/constituents/").Verifiable();
            Constituent constituent = new Constituent
            {
                FirstName = "Jim",
                LastName = "Bob",
                Id = new Guid("06C72D87-0561-4E11-9F0E-794565D4A1F8"),
                ConstituentAddresses = new List<ConstituentAddress>
                {
                    new ConstituentAddress
                    {
                        Address = new Address
                        {
                            City = "Bham",
                            PostalCode = "12345"
                        }
                    }
                }
            };
            var target = new DynamicTransmogrifier();
            var result = target.ToDynamicObject(constituent, urlHelperMock.Object, "FirstName,ConstituentAddresses.Address.City,ConstituentAddresses.Address.PostalCode", true);
            Assert.IsNotNull(result.Links);
            Assert.IsNotNull(result.ConstituentAddresses[0].Links);
            Assert.IsNotNull(result.ConstituentAddresses[0].Address.Links);
        }

        [TestMethod]
        public void Should_ReturnLinksWithHATEOASActionsForEveryObjectWithAnAttribute()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituents, It.IsAny<object>())).Returns("api/v1/constituents/").Verifiable();
            Constituent constituent = new Constituent
            {
                FirstName = "Jim",
                LastName = "Bob",
                Id = new Guid("06C72D87-0561-4E11-9F0E-794565D4A1F8"),
                ConstituentAddresses = new List<ConstituentAddress>
                {
                    new ConstituentAddress
                    {
                        Address = new Address
                        {
                            City = "Bham",
                            PostalCode = "12345"
                        }
                    }
                }
            };
            var target = new DynamicTransmogrifier();
            var result = target.ToDynamicObject(constituent, urlHelperMock.Object, "FirstName,ConstituentAddresses.Address.City,ConstituentAddresses.Address.PostalCode", true);
            Assert.IsNotNull((result.Links[0] as HATEOASLink).Relationship = RouteRelationships.Self);
            Assert.IsNotNull((result.Links[0] as HATEOASLink).Method = RouteVerbs.Get);
            Assert.IsNotNull((result.Links[1] as HATEOASLink).Relationship = RouteRelationships.Update);
            Assert.IsNotNull((result.Links[1] as HATEOASLink).Method = RouteVerbs.Patch);
            Assert.IsNotNull((result.Links[2] as HATEOASLink).Relationship = RouteRelationships.Delete);
            Assert.IsNotNull((result.Links[2] as HATEOASLink).Method = RouteVerbs.Delete);
            Assert.IsTrue(result.ConstituentAddresses[0].Links.Count == 3);
            Assert.IsTrue(result.ConstituentAddresses[0].Address.Links.Count == 3);
        }

        private bool DoesFieldExist(object objectToCheck, string fieldname)
        {
            if (fieldname.Contains("."))
            {
                var property = fieldname.Substring(0, fieldname.IndexOf("."));
                var newArray = fieldname.Substring(property.Length + 1);
                var subObjectToCheck = ((IDictionary<string, object>) objectToCheck)[property];
                return DoesFieldExist(subObjectToCheck, newArray);
            }
            return ((IDictionary<string, object>) objectToCheck).ContainsKey(fieldname);
        }
    }
}
