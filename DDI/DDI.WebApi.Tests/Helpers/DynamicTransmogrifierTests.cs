using System;
using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.WebApi.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.WebApi.Tests.Helpers
{
    [TestClass]
    public class DynamicTransmogrifierTests
    {
        [TestMethod]
        public void Should_ReturnFieldsOtherThanDataTheSame()
        {
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
            var results = target.ToDynamicResponse(initialResponse);
            Assert.AreEqual(errorMessages, results.ErrorMessages);
            Assert.AreEqual(true, results.IsSuccessful);
            Assert.AreEqual(totalResults, results.TotalResults);
            Assert.AreEqual(verboseErrorMessages, results.VerboseErrorMessages);
        }

        [TestMethod]
        public void When_ThereIsNoFields_Should_ReturnFullObject()
        {
            Constituent constituent = new Constituent
            {
                FirstName = "Jim",
                LastName = "Bob",
            };
            var target = new DynamicTransmogrifier();
            var result = target.ToDynamicObject(constituent,"", false);
            Assert.IsTrue(result.FirstName == "Jim");
            Assert.IsTrue(result.LastName == "Bob");
            Assert.IsTrue(result.MiddleName == null);
        }

        [TestMethod]
        public void When_ThereIsNoFieldsButLinks_Should_ReturnFullObjectWithLinks()
        {
            Constituent constituent = new Constituent
            {
                FirstName = "Jim",
                LastName = "Bob",
            };
            var target = new DynamicTransmogrifier();
            var result = target.ToDynamicObject(constituent, "", true);
            Assert.IsTrue(result.FirstName == "Jim");
            Assert.IsTrue(DoesFieldExist(result, "FirstName"));
            Assert.IsTrue(DoesFieldExist(result, "LastName"));
            Assert.IsTrue(DoesFieldExist(result, "Links"));
        }

        [TestMethod]
        public void When_ThereIsNullFieldsButLinks_Should_ReturnFullObjectWithLinks()
        {
            Constituent constituent = new Constituent
            {
                FirstName = "Jim",
                LastName = "Bob",
            };
            var target = new DynamicTransmogrifier();
            var result = target.ToDynamicObject(constituent, null, true);
            Assert.IsTrue(result.FirstName == "Jim");
            Assert.IsTrue(DoesFieldExist(result, "FirstName"));
            Assert.IsTrue(DoesFieldExist(result, "LastName"));
            Assert.IsTrue(DoesFieldExist(result, "Links"));
        }

        [TestMethod]
        public void When_ThereIsOneConstituent_Should_AddLinksToIt()
        {
            Constituent constituent = new Constituent
            {
                FirstName = "Jim",
                LastName = "Bob",
                MiddleName = "Jane"
            };
            var target = new DynamicTransmogrifier();
            var result = target.ToDynamicObject(constituent, "FirstName", true);
            Assert.IsTrue(result.FirstName == "Jim");
            Assert.IsTrue(DoesFieldExist(result, "FirstName"));
            Assert.IsFalse(DoesFieldExist(result, "LastName"));
            Assert.IsFalse(DoesFieldExist(result, "MiddleName"));
            Assert.IsTrue(DoesFieldExist(result, "Links"));
        }

        [TestMethod]
        public void When_ThereIsAListOfConstituents_Should_AddLinksToIt()
        {
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
            var result = target.ToDynamicList(constituents, "FirstName,LastName", true);
            Assert.IsTrue(DoesFieldExist(result[1], "FirstName"));
            Assert.IsTrue(DoesFieldExist(result[1], "LastName"));
            Assert.IsFalse(DoesFieldExist(result[1], "MiddleName"));
            Assert.IsTrue(DoesFieldExist(result[1], "Links"));
        }

        [TestMethod]
        public void When_ThereIsASubProperty_Should_ReturnIt()
        {
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
            var result = target.ToDynamicObject(constituent, "FirstName,ConstituentAddresses.Address.City,ConstituentAddresses.Address.PostalCode", false);
            Assert.IsTrue(result.ConstituentAddresses[0].Address.City == "Bham");
            Assert.IsTrue(result.ConstituentAddresses[0].Address.PostalCode == "12345");
            Assert.IsTrue(result.FirstName == "Jim");
            Assert.IsTrue(DoesFieldExist(result, "FirstName"));
            Assert.IsFalse(DoesFieldExist(result, "LastName"));
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
