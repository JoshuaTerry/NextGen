﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        public void Should_ReturnCorrectValueForSimpleType()
        {
            var target = new DynamicTransmogrifier();
            Assert.IsTrue(target.IsSimple(typeof(string)));
            Assert.IsTrue(target.IsSimple(typeof(int)));
            Assert.IsTrue(target.IsSimple(typeof(int?)));
            Assert.IsTrue(target.IsSimple(typeof(decimal)));
            Assert.IsFalse(target.IsSimple(typeof(DateTime)));
            Assert.IsFalse(target.IsSimple(typeof(DateTime?)));
            Assert.IsFalse(target.IsSimple(typeof(Constituent)));
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void When_ThereDataIsNotTransmogrfiable_Should_ReturnFullObject()
        {
            var urlHelperMock = new Mock<UrlHelper>();
            urlHelperMock.Setup(m => m.Link(RouteNames.Constituent, null)).Returns("TEST").Verifiable();
            var peytonIsTheGreatest = "Peyton is the greatest!!!";
            var totalResults = 18;
            var theGreatest = "The Greatest";
            var errorMessages = new List<string>
            {
                theGreatest
            };
            var verboseErrorMessages = new List<string>
            {
                peytonIsTheGreatest
            };
            DataResponse<string> initialResponse = new DataResponse<string>
            {
                Data = "Peyton",
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
            Assert.AreEqual("Peyton", ((DataResponse<dynamic>) results).Data);
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
            var result = target.ToDynamicObject(constituent, urlHelperMock.Object, "");
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
            var result = target.ToDynamicObject(constituent, urlHelperMock.Object, "");
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
            var result = target.ToDynamicObject(constituent, urlHelperMock.Object, null);
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
            var result = target.ToDynamicObject(constituent, urlHelperMock.Object, "FirstName,Links");
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
            var result = target.ToDynamicList(constituents, urlHelperMock.Object, "FirstName,LastName,Links");
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
            var result = target.ToDynamicObject(constituent, urlHelperMock.Object, "FirstName,ConstituentAddresses.Address.City,ConstituentAddresses.Address.PostalCode");
            Assert.IsTrue(result.ConstituentAddresses[0].Address.City == "Bham");
            Assert.IsTrue(result.ConstituentAddresses[0].Address.PostalCode == "12345");
            Assert.IsTrue(result.FirstName == "Jim");
            Assert.IsTrue(DoesFieldExist(result, "FirstName"));
            Assert.IsFalse(DoesFieldExist(result, "LastName"));
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
            var result = target.ToDynamicObject(constituent, urlHelperMock.Object, "FirstName,Links,ConstituentAddresses.Address.City,ConstituentAddresses.Address.PostalCode");
            Assert.IsNotNull(result.Links);
            Assert.IsNotNull(result.ConstituentAddresses[0].Links);
            Assert.IsNotNull(result.ConstituentAddresses[0].Address.Links);
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
