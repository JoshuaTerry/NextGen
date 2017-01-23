using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Routing;
using DDI.Shared;
using DDI.Shared.Attributes;
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
        private const string TESTDESCR = "WebApi | Helpers";

        [TestMethod, TestCategory(TESTDESCR)]
        public void Should_ReturnIdenticalDataResponseFieldsOtherThanData()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituent, null)).Returns("TEST").Verifiable();
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

        [TestMethod, TestCategory(TESTDESCR)]
        public void When_ThereIsNoFields_Should_ReturnFullObject()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituent, null)).Returns("TEST").Verifiable();
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

        [TestMethod, TestCategory(TESTDESCR)]
        public void When_ThereIsNoFieldsButLinks_Should_ReturnFullObjectWithLinks()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituent, null)).Returns("TEST").Verifiable();
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

        [TestMethod, TestCategory(TESTDESCR)]
        public void When_ThereIsNullFieldsButLinks_Should_ReturnFullObjectWithLinks()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituent, null)).Returns("TEST").Verifiable();
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

        [TestMethod, TestCategory(TESTDESCR)]
        public void When_ThereIsOneConstituent_Should_AddLinksToIt()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituent, null)).Returns("TEST").Verifiable();
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

        [TestMethod, TestCategory(TESTDESCR)]
        public void When_ThereIsAListOfConstituents_Should_AddLinksToIt()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituent, null)).Returns("TEST").Verifiable();
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

        [TestMethod, TestCategory(TESTDESCR)]
        public void When_ThereIsASubProperty_Should_ReturnIt()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituent, null)).Returns("TEST").Verifiable();
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
                            PostalCode = "12345",
                            Id = new Guid("736D341E-B392-4D79-83B5-46D5E5A92581")
                        },
                        Id = new Guid("21A2A412-5620-48A8-80D8-9D10BC95E160")
                    }
                },
                Id = new Guid("44EEB5B8-C2AC-4CCD-B0EE-296866DB8374")
            };
            var target = new DynamicTransmogrifier();
            var result = target.ToDynamicObject(constituent, urlHelperMock.Object, "FirstName,ConstituentAddresses.Address.City,ConstituentAddresses.Address.PostalCode", false);
            Assert.IsTrue(result.ConstituentAddresses[0].Address.City == "Bham");
            Assert.IsTrue(result.ConstituentAddresses[0].Address.PostalCode == "12345");
            Assert.IsTrue(result.FirstName == "Jim");
            Assert.IsTrue(DoesFieldExist(result, "FirstName"));
            Assert.IsFalse(DoesFieldExist(result, "LastName"));
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void When_ReturningEverythingAndHateoasLinks_Should_ReturnSubProperties()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituent, null)).Returns("TEST").Verifiable();
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
                            PostalCode = "12345",
                            Id = new Guid("736D341E-B392-4D79-83B5-46D5E5A92581")
                        },
                        Id = new Guid("21A2A412-5620-48A8-80D8-9D10BC95E160")
                    }
                },
                Id = new Guid("44EEB5B8-C2AC-4CCD-B0EE-296866DB8374")
            };
            var target = new DynamicTransmogrifier();
            var result = target.ToDynamicObject(constituent, urlHelperMock.Object, null, true);
            Assert.IsTrue(result.ConstituentAddresses[0].Address.City == "Bham");
            Assert.IsTrue(result.ConstituentAddresses[0].Address.PostalCode == "12345");
            Assert.IsTrue(result.FirstName == "Jim");
            Assert.IsTrue(result.LastName == "Bob");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void When_ReturningEverythingAndHateoasLinks_Should_AvoidInfiniteLoops()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituent, null)).Returns("TEST").Verifiable();
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
                            PostalCode = "12345",
                            Id = new Guid("736D341E-B392-4D79-83B5-46D5E5A92581")
                        },
                        Id = new Guid("21A2A412-5620-48A8-80D8-9D10BC95E160")
                    }
                },
                Id = new Guid("44EEB5B8-C2AC-4CCD-B0EE-296866DB8374")
                
            };
            constituent.ConstituentAddresses.First().Address.ConstituentAddresses = new List<ConstituentAddress> { constituent.ConstituentAddresses.First() };
            constituent.ConstituentAddresses.First().Constituent = constituent;
            var target = new DynamicTransmogrifier();
            var result = target.ToDynamicObject(constituent, urlHelperMock.Object, null, true);
            Assert.IsTrue(result.ConstituentAddresses[0].Address.City == "Bham");
            Assert.IsTrue(result.ConstituentAddresses[0].Address.PostalCode == "12345");
            Assert.IsTrue(result.FirstName == "Jim");
            Assert.IsTrue(result.LastName == "Bob");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void When_ReturningEverythingAndNoHateoas_Should_ReturnSubProperties()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituent, null)).Returns("TEST").Verifiable();
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
                            PostalCode = "12345",
                            Id = new Guid("736D341E-B392-4D79-83B5-46D5E5A92581")
                        },
                        Id = new Guid("21A2A412-5620-48A8-80D8-9D10BC95E160")
                    }
                },
                Id = new Guid("44EEB5B8-C2AC-4CCD-B0EE-296866DB8374")
            };
            var target = new DynamicTransmogrifier();
            var result = target.ToDynamicObject(constituent, urlHelperMock.Object, null, false);
            Assert.IsTrue(result.ConstituentAddresses[0].Address.City == "Bham");
            Assert.IsTrue(result.ConstituentAddresses[0].Address.PostalCode == "12345");
            Assert.IsTrue(result.FirstName == "Jim");
            Assert.IsTrue(result.LastName == "Bob");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void When_ThereIsAreSubPropertiesAndLinksRequested_Should_ReturnLinksForEveryObject()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituent, It.IsAny<object>())).Returns("api/v1/constituents/").Verifiable();
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
                            PostalCode = "12345",
                            Id = new Guid("736D341E-B392-4D79-83B5-46D5E5A92581")
                        },
                        Id = new Guid("21A2A412-5620-48A8-80D8-9D10BC95E160")
                    }
                },
            };
            var target = new DynamicTransmogrifier();
            var result = target.ToDynamicObject(constituent, urlHelperMock.Object, "FirstName,ConstituentAddresses.Address.City,ConstituentAddresses.Address.PostalCode", true);
            Assert.IsNotNull(result.Links);
            Assert.IsNotNull(result.ConstituentAddresses[0].Links);
            Assert.IsNotNull(result.ConstituentAddresses[0].Address.Links);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void Should_ReturnLinksWithHateoasActionsForEveryObjectWithAnAttribute()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituent, It.IsAny<object>())).Returns("api/v1/constituents/").Verifiable();
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
                            PostalCode = "12345",
                            Id = new Guid("736D341E-B392-4D79-83B5-46D5E5A92581")
                        },
                        Id = new Guid("21A2A412-5620-48A8-80D8-9D10BC95E160")
                    }
                },
            };
            var target = new DynamicTransmogrifier();
            var result = target.ToDynamicObject(constituent, urlHelperMock.Object, "FirstName,ConstituentAddresses.Address.City,ConstituentAddresses.Address.PostalCode", true);
            List<HateoasLink> links = result.Links as List<HateoasLink>;
            Assert.IsNotNull(links.Single(a => a.Method == RouteVerbs.Get && a.Relationship == RouteRelationships.Self));
            Assert.IsNotNull(links.Single(a => a.Method == RouteVerbs.Get && a.Relationship == (RouteRelationships.Get + RouteNames.ConstituentAddress)));
            Assert.IsTrue(links.Single(a => a.Method == RouteVerbs.Patch).Relationship == RouteRelationships.Update + RouteNames.Constituent);
            Assert.IsTrue(links.Single(a => a.Method == RouteVerbs.Delete).Relationship == RouteRelationships.Delete + RouteNames.Constituent);
            Assert.IsTrue(links.Single(a => a.Method == RouteVerbs.Post).Relationship == RouteRelationships.New + RouteNames.ConstituentAddress);
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
