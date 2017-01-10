using System;
using System.Collections.Generic;
using DDI.Data.Extensions;
using DDI.Data.Models.Client.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Data.Tests.Extensions
{
    [TestClass]
    public class EntityBaseExtensionsTests
    {
        [TestMethod]
        public void When_ThereIsNoFields_Should_ReturnFullObject()
        {
            Constituent constituent = new Constituent
            {
                FirstName = "Jim",
                LastName = "Bob"
            };
            Func<Constituent> funcToExecute = (() => constituent);
            var result = funcToExecute();
            var resultWithLinks = result.ToPartialObject("", false);
        }

        [TestMethod]
        public void When_ThereIsOneConstituent_Should_AddLinksToIt()
        {
            Constituent constituent = new Constituent
            {
                FirstName = "Jim",
                LastName = "Bob"
            };
            Func<Constituent> funcToExecute = (() => constituent);
            var result = funcToExecute();
            var resultWithLinks = result.ToPartialObject("FirstName",true);
        }

        [TestMethod]
        public void When_ThereIsAListOfConstituents_Should_AddLinksToIt()
        {
            List<Constituent> constituents = new List<Constituent> { 
                new Constituent
                {
                    FirstName = "Jim",
                    LastName = "Bob"
                },
                new Constituent
                {
                    FirstName = "Sally",
                    LastName = "Fields"
                }
            };
            Func<List<Constituent>> funcToExecute = (() => constituents);
            var result = funcToExecute();
            var resultWithLinks = result.ToPartialObject("FirstName,LastName", true);
        }
    }
}
