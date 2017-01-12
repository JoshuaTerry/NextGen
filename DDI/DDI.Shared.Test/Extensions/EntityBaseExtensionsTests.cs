using System;
using System.Collections.Generic;
using DDI.Shared.Extensions;
using DDI.Shared.Models.Client.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Shared.Tests.Extensions
{
    [TestClass]
    public class EntityBaseExtensionsTests
    {
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

        [TestMethod]
        public void When_ThereIsNoFields_Should_ReturnFullObject()
        {
            Constituent constituent = new Constituent
            {
                FirstName = "Jim",
                LastName = "Bob",
            };
            Func<Constituent> funcToExecute = (() => constituent);
            var result = funcToExecute();
            var resultWithLinks = result.ToPartialObject("", false);
            Assert.IsTrue(resultWithLinks.FirstName == "Jim");
            Assert.IsTrue(resultWithLinks.LastName == "Bob");
            Assert.IsTrue(resultWithLinks.MiddleName == null);
        }

        [TestMethod]
        public void When_ThereIsNoFieldsButLinks_Should_ReturnFullObjectWithLinks()
        {
            Constituent constituent = new Constituent
            {
                FirstName = "Jim",
                LastName = "Bob",
            };
            Func<Constituent> funcToExecute = (() => constituent);
            var result = funcToExecute();
            var resultWithLinks = result.ToPartialObject("", true);
            Assert.IsTrue(resultWithLinks.FirstName == "Jim");
            Assert.IsTrue(DoesFieldExist(resultWithLinks, "FirstName"));
            Assert.IsTrue(DoesFieldExist(resultWithLinks, "LastName"));
            Assert.IsTrue(DoesFieldExist(resultWithLinks, "Links"));
        }

        [TestMethod]
        public void When_ThereIsNullFieldsButLinks_Should_ReturnFullObjectWithLinks()
        {
            Constituent constituent = new Constituent
            {
                FirstName = "Jim",
                LastName = "Bob",
            };
            Func<Constituent> funcToExecute = (() => constituent);
            var result = funcToExecute();
            var resultWithLinks = result.ToPartialObject(null, true);
            Assert.IsTrue(resultWithLinks.FirstName == "Jim");
            Assert.IsTrue(DoesFieldExist(resultWithLinks, "FirstName"));
            Assert.IsTrue(DoesFieldExist(resultWithLinks, "LastName"));
            Assert.IsTrue(DoesFieldExist(resultWithLinks, "Links"));
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
            Func<Constituent> funcToExecute = (() => constituent);
            var result = funcToExecute();
            var resultWithLinks = result.ToPartialObject("FirstName",true);
            Assert.IsTrue(resultWithLinks.FirstName == "Jim");
            Assert.IsTrue(DoesFieldExist(resultWithLinks, "FirstName"));
            Assert.IsFalse(DoesFieldExist(resultWithLinks, "LastName"));
            Assert.IsFalse(DoesFieldExist(resultWithLinks, "MiddleName"));
            Assert.IsTrue(DoesFieldExist(resultWithLinks, "Links"));
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
            Func<List<Constituent>> funcToExecute = (() => constituents);
            var result = funcToExecute();
            var resultWithLinks = result.ToPartialObject("FirstName,LastName", true);
            Assert.IsTrue(DoesFieldExist(resultWithLinks[1], "FirstName"));
            Assert.IsTrue(DoesFieldExist(resultWithLinks[1], "LastName"));
            Assert.IsFalse(DoesFieldExist(resultWithLinks[1], "MiddleName"));
            Assert.IsTrue(DoesFieldExist(resultWithLinks[1], "Links"));
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
            Func<Constituent> funcToExecute = (() => constituent);
            var result = funcToExecute();
            var resultWithLinks = result.ToPartialObject("FirstName,ConstituentAddresses.Address.City,ConstituentAddresses.Address.PostalCode", false);
            Assert.IsTrue(resultWithLinks.ConstituentAddresses[0].Address.City == "Bham");
            Assert.IsTrue(resultWithLinks.ConstituentAddresses[0].Address.PostalCode == "12345");
            Assert.IsTrue(resultWithLinks.FirstName == "Jim");
            Assert.IsTrue(DoesFieldExist(resultWithLinks, "FirstName"));
            Assert.IsFalse(DoesFieldExist(resultWithLinks, "LastName"));
        }
    }
}
