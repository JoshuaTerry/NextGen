using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Helpers;
using DDI.Data;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace DDI.Business.Tests.Helpers
{
    [TestClass]
    public class AbbreviationHelperTest
    {

        private const string TESTDESCR = "Business | Helpers";

        private UnitOfWorkNoDb _uow;

        [TestInitialize]
        public void Initialize()
        {

            _uow = new UnitOfWorkNoDb();
            var abbreviations = SetupAbbrevationRepo();
            _uow.CreateRepositoryForDataSource(abbreviations);
            AbbreviationHelper.SetAbbreviations(abbreviations);
        }

        private IQueryable<Abbreviation> SetupAbbrevationRepo()
        {
            var list = new List<Abbreviation>();
            list.Add(new Abbreviation { Word = "NORTH", AddressWord = "NORTH", NameWord = "NORTH", USPSAbbreviation = "N", Priority = 2 });
            list.Add(new Abbreviation { Word = "N", AddressWord = "NORTH", NameWord = "N.", USPSAbbreviation = "N", Priority = 2 });
            list.Add(new Abbreviation { Word = "SOUTH", AddressWord = "SOUTH", NameWord = "SOUTH", USPSAbbreviation = "S", Priority = 2 });
            list.Add(new Abbreviation { Word = "S", AddressWord = "SOUTH", NameWord = "S.", USPSAbbreviation = "S", Priority = 2 });
            list.Add(new Abbreviation { Word = "EAST", AddressWord = "EAST", NameWord = "EAST", USPSAbbreviation = "E", Priority = 2 });
            list.Add(new Abbreviation { Word = "E", AddressWord = "EAST", NameWord = "E.", USPSAbbreviation = "E", Priority = 2 });
            list.Add(new Abbreviation { Word = "WEST", AddressWord = "WEST", NameWord = "WEST", USPSAbbreviation = "W", Priority = 2 });
            list.Add(new Abbreviation { Word = "W", AddressWord = "WEST", NameWord = "W.", USPSAbbreviation = "W", Priority = 2 });

            list.Add(new Abbreviation { Word = "RIVER", AddressWord = "RIVER", NameWord = "RIVER", USPSAbbreviation = "RIV", Priority = 1, IsSuffix = true });
            list.Add(new Abbreviation { Word = "APT", AddressWord = "APT.", NameWord = "APT", USPSAbbreviation = "APT", Priority = 1, IsSecondary = true });
            list.Add(new Abbreviation { Word = "SUITE", AddressWord = "SUITE", NameWord = "", USPSAbbreviation = "STE", Priority = 0, IsSecondary = true });
            list.Add(new Abbreviation { Word = "STE", AddressWord = "SUITE", NameWord = "", USPSAbbreviation = "STE", Priority = 0, IsSecondary = true });
            list.Add(new Abbreviation { Word = "FLOOR", AddressWord = "FLOOR", NameWord = "FLOOR", USPSAbbreviation = "FL", Priority = 0, IsSecondary = true });
            list.Add(new Abbreviation { Word = "FL", AddressWord = "FLOOR", NameWord = "FLOOR", USPSAbbreviation = "FL", Priority = 0, IsSecondary = true });
            list.Add(new Abbreviation { Word = "BLDG", AddressWord = "BUILDING", NameWord = "BUILDING", USPSAbbreviation = "BLDG", Priority = 0, IsSecondary = true });
            list.Add(new Abbreviation { Word = "BUILDING", AddressWord = "BUILDING", NameWord = "BUILDING", USPSAbbreviation = "BLDG", Priority = 0, IsSecondary = true });
            list.Add(new Abbreviation { Word = "HC", AddressWord = "HC", NameWord = "", USPSAbbreviation = "HC", Priority = 1, IsCaps = true });
            list.Add(new Abbreviation { Word = "PO", AddressWord = "P. O.", NameWord = "", USPSAbbreviation = "PO", Priority = 1, IsCaps = true });


            list.Add(new Abbreviation { Word = "ROAD", AddressWord = "ROAD", NameWord = "ROAD", USPSAbbreviation = "RD", Priority = 0, IsSuffix = true });
            list.Add(new Abbreviation { Word = "RD", AddressWord = "ROAD", NameWord = "ROAD", USPSAbbreviation = "RD", Priority = 0, IsSuffix = true });
            list.Add(new Abbreviation { Word = "RR", AddressWord = "RR", NameWord = "", USPSAbbreviation = "RR", Priority = 1, IsCaps = true });
            list.Add(new Abbreviation { Word = "STREET", AddressWord = "STREET", NameWord = "STREET", USPSAbbreviation = "ST", Priority = 0, IsSuffix = true });
            list.Add(new Abbreviation { Word = "ST", AddressWord = "STREET", NameWord = "STREET", USPSAbbreviation = "ST", Priority = 0, IsSuffix = true });
            list.Add(new Abbreviation { Word = "AVENUE", AddressWord = "AVENUE", NameWord = "", USPSAbbreviation = "AVE", Priority = 0, IsSuffix = true });
            list.Add(new Abbreviation { Word = "AVE", AddressWord = "AVENUE", NameWord = "", USPSAbbreviation = "AVE", Priority = 0, IsSuffix = true });


            list.Add(new Abbreviation { Word = "&", AddressWord = "AND", NameWord = "AND", USPSAbbreviation = "&", Priority = 1 });
            list.Add(new Abbreviation { Word = "CH", AddressWord = "CHURCH", NameWord = "CHURCH", USPSAbbreviation = "CH", Priority = 1 });
            list.Add(new Abbreviation { Word = "JR", AddressWord = "", NameWord = "JR.", USPSAbbreviation = "JR", Priority = 0, IsSuffix = false });
            list.Add(new Abbreviation { Word = "INC", AddressWord = "", NameWord = "INC.", USPSAbbreviation = "INC", Priority = 0, IsSuffix = false });
            list.Add(new Abbreviation { Word = "SR", AddressWord = "", NameWord = "SR.", USPSAbbreviation = "SR", Priority = 0, IsSuffix = false });
            list.Add(new Abbreviation { Word = "FIRST", AddressWord = "FIRST", NameWord = "FIRST", USPSAbbreviation = "1ST", Priority = 2 });
            list.Add(new Abbreviation { Word = "CENTER", AddressWord = "CENTER", NameWord = "CENTER", USPSAbbreviation = "CTR", IsSuffix = true, Priority = 1 });
            list.Add(new Abbreviation { Word = "CTR", AddressWord = "CENTER", NameWord = "CENTER", USPSAbbreviation = "CTR", IsSuffix = true, Priority = 0 });
            list.Add(new Abbreviation { Word = "BRETHREN", AddressWord = "", NameWord = "BRETHREN", USPSAbbreviation = "BRETH" });
            list.Add(new Abbreviation { Word = "BRETH", AddressWord = "", NameWord = "BRETHREN", USPSAbbreviation = "BRETH" });

            return list.AsQueryable();
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void AbbreviationHelper_ExpandNameLine()
        {
            Assert.AreEqual("Saint Mark Church", AbbreviationHelper.ExpandNameLine("St Mark Ch", false), "Standard functionality test failed.");

            // Suffixes should be proceeded by comma
            foreach (var entry in "Jr,Sr,Inc".Split(','))
            {
                Assert.AreEqual($"John Doe, {entry}.", AbbreviationHelper.ExpandNameLine($"John Doe {entry}", false),
                       "Suffix/comma test failed.");
            }

            Assert.AreEqual("John Doe, III", AbbreviationHelper.ExpandNameLine("John Doe III", false), "Suffix/comma test (III) failed.");

            // AddPeriods option: Single character word gets trailing period.
            Assert.AreEqual("John Q. Public", AbbreviationHelper.ExpandNameLine("John Q Public", true), "Add periods (single char) test failed.");

            // AddPeriods option: Certain prefixes get a trailing period.
            foreach (var entry in "Mr,Mrs,Ms,Dr,Rev".Split(','))
            {
                Assert.AreEqual($"{entry}. John Doe", AbbreviationHelper.ExpandNameLine($"{entry} John Doe", true),
                       "Add periods for prefix test failed.");
            }

            // AddPeriods option: Certain suffixes get a trailing period.
            foreach (var entry in "Jr,Sr,Inc".Split(','))
            {
                Assert.AreEqual($"John Doe, {entry}.", AbbreviationHelper.ExpandNameLine($"John Doe {entry}", true),
                       "Add periods for suffix test failed.");
            }

            // Certain abbreviations are capitalized
            Assert.AreEqual("RR", AbbreviationHelper.ExpandNameLine("RR", false),
                   "Capitalization test failed.");


        }


        [TestMethod, TestCategory(TESTDESCR)]
        public void AbbreviationHelper_AbbreviateNameLine()
        {
            // Standard functionality
            Assert.AreEqual("1st Ctr", AbbreviationHelper.AbbreviateNameLine("First Center", false, false),
                "Standard functionality test failed.");

            // Abbreviate all option
            Assert.AreEqual("East St", AbbreviationHelper.AbbreviateNameLine("East Street", false, false),
                "abbreviateAll = false: East should not be abbreviated.");

            Assert.AreEqual("E St", AbbreviationHelper.AbbreviateNameLine("East Street", false, true),
                "abbreviateAll = true: East should be abbreviated.");

            // All caps option
            Assert.AreEqual("1ST CHURCH", AbbreviationHelper.AbbreviateNameLine("First Church", true, false),
                "All caps option test failed.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void AbbreviationHelper_AbbreviateNameLine_Length_Limited()
        {
            string test = "First Church of the Brethren";

            // Standard functionality
            Assert.AreEqual(test, AbbreviationHelper.AbbreviateNameLine(test, test.Length, false), 
                "Exact length test failed.");
            Assert.AreEqual("1st Church of the Brethren", AbbreviationHelper.AbbreviateNameLine(test, test.Length - 1, false),
                "Length - 1 test failed.");
            Assert.AreEqual("1st Church of the Breth", AbbreviationHelper.AbbreviateNameLine(test, test.Length - 8, false),
                "Length - 8 test failed.");

            // Last chance abbreviations
            Assert.AreEqual("Rock & Roll", AbbreviationHelper.AbbreviateNameLine("Rock and Roll", 8, false),
                "Last chance abbreviation for 'and' failed.");
            Assert.AreEqual("E Boston", AbbreviationHelper.AbbreviateNameLine("E. Boston", 8, false), 
                "Last chance period removal failed.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void AbbreviationHelper_ExpandAddressLine()
        {
            // Standard functionality
            Assert.AreEqual("123 West Main Avenue\nBuilding 9", AbbreviationHelper.ExpandAddressLine("123 W Main Ave\nBldg 9"), 
                "Standard functionality test failed.");

            // Quoted street letter name.
            Assert.AreEqual("123 West \"W\" Street", AbbreviationHelper.ExpandAddressLine("123 W \"W\" St"), 
                "Quoted street letter name test failed.");

            // Saint vs. St.  Expands to Saint only if at the beginning.
            Assert.AreEqual("123 St. Mark Street", AbbreviationHelper.ExpandAddressLine("123 St. Mark St."),
                "Saint vs. St. (with street number) test failed.");
            Assert.AreEqual("Saint Mark Street", AbbreviationHelper.ExpandAddressLine("St. Mark St."), 
                "Saint vs. St. (no street number) test failed.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void AbbreviationHelper_AbbreviateAddressLine()
        {
            // Standard functionality
            Assert.AreEqual("123 W Main Ave N", AbbreviationHelper.AbbreviateAddressLine("123 West Main Avenue North", false, false),
                "Standard functionality test failed.");

            // River is abbreviated only with abbreviate all option
            Assert.AreEqual("123 W River Rd", AbbreviationHelper.AbbreviateAddressLine("123 West River Road", false, false),
                "abbreviateAll = false: River should not be abbreviated.");
            Assert.AreEqual("123 W Riv Rd", AbbreviationHelper.AbbreviateAddressLine("123 West River Road", false, true), 
                "abbreviateAll = true: River should be abbreviated.");

            // All caps option
            Assert.AreEqual("123 W RIVER RD", AbbreviationHelper.AbbreviateAddressLine("123 West River Road", true, false), 
                "All caps option test failed.");

            // Directionals don't get abbreviated if they appear to be streets.
            Assert.AreEqual("123 N East St", AbbreviationHelper.AbbreviateAddressLine("123 N. East Street", false, false), 
                "Directional street name test failed.");

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void AbbreviationHelper_AbbreviateAddressLine_Length_Limited()
        {
            string test = "123 West Main Avenue North";

            // Standard functionality
            Assert.AreEqual(test, AbbreviationHelper.AbbreviateAddressLine(test, test.Length, false), 
                "Exact length test failed.");
            Assert.AreEqual("123 W Main Avenue North", AbbreviationHelper.AbbreviateAddressLine(test, test.Length - 1, false), 
                "Length - 1 test failed.");
            Assert.AreEqual("123 W Main Avenue N", AbbreviationHelper.AbbreviateAddressLine(test, test.Length - 6, false),
                "Length - 6 test failed.");
            Assert.AreEqual("123 W Main Ave N", AbbreviationHelper.AbbreviateAddressLine(test, test.Length - 10, false), 
                "Length - 10 test failed.");

            // Last chance abbreviations
            Assert.AreEqual("Rock & Roll", AbbreviationHelper.AbbreviateAddressLine("Rock and Roll", 8, false), 
                "Last chance abbreviation for 'and' failed.");
            Assert.AreEqual("E Boston", AbbreviationHelper.AbbreviateAddressLine("E. Boston", 8, false),
                "Last chance period removal failed.");

        }
    }
}
