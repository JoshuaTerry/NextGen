using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Common;
using DDI.Data;
using DDI.Data.Enums.Common;
using DDI.Data.Models.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DDI.Business.Tests.Common
{
    [TestClass]
    public class ZipLookupTest
    {
        private ZipLookup _zipLookup;
        private UnitOfWorkNoDb _uow;

        [TestInitialize]
        public void Initialize()
        {
            _uow = new UnitOfWorkNoDb();
            var abbrRepo = new Mock<IRepository<Abbreviation>>();
            abbrRepo.Setup(r => r.Entities).Returns(SetupAbbrevationRepo());
            _uow.SetRepository<Abbreviation>(abbrRepo.Object);
            
            _zipLookup = new ZipLookup(_uow);
            _zipLookup.Initialize();
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
            list.Add(new Abbreviation { Word = "HC", AddressWord = "HC", NameWord = "", USPSAbbreviation = "HC", Priority = 1, IsCaps = true });
            list.Add(new Abbreviation { Word = "PO", AddressWord = "P. O.", NameWord = "", USPSAbbreviation = "PO", Priority = 1, IsCaps = true });


            list.Add(new Abbreviation { Word = "ROAD", AddressWord = "ROAD", NameWord = "ROAD", USPSAbbreviation = "RD", Priority = 0, IsSuffix = true });
            list.Add(new Abbreviation { Word = "RD", AddressWord = "ROAD", NameWord = "ROAD", USPSAbbreviation = "RD", Priority = 0, IsSuffix = true });
            list.Add(new Abbreviation { Word = "STREET", AddressWord = "STREET", NameWord = "STREET", USPSAbbreviation = "ST", Priority = 0, IsSuffix = true });
            list.Add(new Abbreviation { Word = "ST", AddressWord = "STREET", NameWord = "STREET", USPSAbbreviation = "ST", Priority = 0, IsSuffix = true });

            return list.AsQueryable();
        }

        private const string TESTDESCR = "Business | Common";

        [TestMethod,TestCategory(TESTDESCR)]
        public void ZipLookup_AbbreviateWords()
        {
            // This method assumes words are capitalized.
            Assert.AreEqual(_zipLookup.AbbreviateWords("NORTH RIVER ROAD"), "N RIV RD", "Basic functionality test.");
            // Periods should be omitted, other non-letters should be included.
            Assert.AreEqual(_zipLookup.AbbreviateWords("N. A-B ROAD"), "N A-B RD", "Period should be omitted.");
        }

        [TestMethod,TestCategory(TESTDESCR)]
        public void ZipLookup_SplitNumber()
        {

            CollectionAssert.AreEquivalent(_zipLookup.SplitNumber("123ABC456"), new string[] { "123", "ABC", "456" }, "Basic functionality.");            
            CollectionAssert.AreEquivalent(_zipLookup.SplitNumber("3 1/2"), new string[] { "3", " ", "1/2" }, "Fractions shouldn't be split.");
        }

        /// <summary>
        /// Test WordSplit: Split an address into words delimited by space or other delimiters.
        /// </summary>
        [TestMethod,TestCategory(TESTDESCR)]
        public void ZipLookup_WordSplit()
        {
            string[] rslt = new string[] { "A1C", "D2F" };
            CollectionAssert.AreEquivalent(_zipLookup.WordSplit("A1C D2F"), rslt, "Basic functionality.");
            CollectionAssert.AreEquivalent(_zipLookup.WordSplit("A1C-D2F"), rslt, "'-' Delimiter test failed.");
            CollectionAssert.AreEquivalent(_zipLookup.WordSplit("A1C.D2F"), rslt, "'.' Delimiter test failed.");
            CollectionAssert.AreEquivalent(_zipLookup.WordSplit("A1C,D2F"), rslt, "',' Delimiter test failed.");
            CollectionAssert.AreEquivalent(_zipLookup.WordSplit("A1C'D2F"), rslt, "quote Delimiter test failed.");
            CollectionAssert.AreEquivalent(_zipLookup.WordSplit("A1C\"D2F"), rslt, "double quote Delimiter test failed.");
            CollectionAssert.AreEquivalent(_zipLookup.WordSplit("A1C. D2F"), rslt, "Multiple delimiter test failed.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ZipLookup_WordCombine()
        {
            ZipLookup_WordCombine_Test("P O Box 1234", "PO Box 1234");
            ZipLookup_WordCombine_Test("R R 5", "RR 5");
            ZipLookup_WordCombine_Test("RURAL RT 5", "RR 5");
            ZipLookup_WordCombine_Test("H C 6", "HC 6");
            ZipLookup_WordCombine_Test("HWY CONTRACT 6", "HC 6");
            ZipLookup_WordCombine_Test("HIGHWAY C 6", "HC 6");
            ZipLookup_WordCombine_Test("RT 7", "RR 7");
            ZipLookup_WordCombine_Test("100 MAIN ST #5", "100 MAIN ST APT 5");
            ZipLookup_WordCombine_Test("100 MAIN ST APARTMENT 5", "100 MAIN ST APT 5");
            ZipLookup_WordCombine_Test("100 MAIN ST APT #5", "100 MAIN ST APT 5");
        }

        private void ZipLookup_WordCombine_Test(string text1, string text2)
        {
            List<string> list = text1.Split(' ').ToList();
            List<string> rsltList = text2.Split(' ').ToList();

            _zipLookup.WordCombine(list);

            CollectionAssert.AreEquivalent(list, rsltList);
        }

        /// <summary>
        /// Test CompareAddressNumber: Address number comparison for Zip+4 lookup
        /// </summary>
        [TestMethod,TestCategory(TESTDESCR)]
        public void ZipLookup_CompareAddressNumber()
        {
            Assert.IsTrue(_zipLookup.CompareAddressNumber("555", "500", "600", EvenOddType.Any), 
                "Within range test failed.");

            Assert.IsFalse(_zipLookup.CompareAddressNumber("7", "500", "600", EvenOddType.Any), 
                "Less than test failed.");

            Assert.IsFalse(_zipLookup.CompareAddressNumber("4000", "500", "600", EvenOddType.Any), 
                "Greater than test failed.");

            Assert.IsTrue(_zipLookup.CompareAddressNumber("555", "500", "600", EvenOddType.Odd), 
                "Odd vs. Odd test failed.");

            Assert.IsFalse(_zipLookup.CompareAddressNumber("555", "500", "600", EvenOddType.Even), 
                "Odd vs. Even test failed.");

            Assert.IsTrue(_zipLookup.CompareAddressNumber("562", "500", "600", EvenOddType.Even), 
                "Even vs. Even test failed.");

            Assert.IsFalse(_zipLookup.CompareAddressNumber("562", "500", "600", EvenOddType.Odd), 
                "Even vs. Odd test failed.");

            Assert.IsTrue(_zipLookup.CompareAddressNumber("5C", "5A", "5D", EvenOddType.Any), 
                "Alpha suffix within range test failed.");

            Assert.IsFalse(_zipLookup.CompareAddressNumber("5C", "5D", "5G", EvenOddType.Any), 
                "Alpha suffix less than test failed.");

            Assert.IsTrue(_zipLookup.CompareAddressNumber("5DD", "5D", "5G", EvenOddType.Any), 
                "Alpha suffix padding test failed.");

            Assert.IsFalse(_zipLookup.CompareAddressNumber("5H", "5D", "5G", EvenOddType.Any), 
                "Alpha suffix greater than test failed.");

            Assert.IsTrue(_zipLookup.CompareAddressNumber("150D", "120D", "169D", EvenOddType.Any), 
                "Numeric prefix within range test failed.");

            Assert.IsFalse(_zipLookup.CompareAddressNumber("119D", "120D", "169D", EvenOddType.Any), 
                "Numeric prefix less than test failed.");

            Assert.IsFalse(_zipLookup.CompareAddressNumber("170D", "120D", "169D", EvenOddType.Any), 
                "Numeric prefix greater than test failed.");

            Assert.IsFalse(_zipLookup.CompareAddressNumber("130D", "120D", "169", EvenOddType.Any), 
                "Format mismatch test failed.");

        }

        /// <summary>
        /// Test SplitAddress: Split a street address into an Address object.
        /// </summary>
        [TestMethod, TestCategory(TESTDESCR)]
        public void ZipLookup_SplitAddress()
        {
            ZipLookup.Address rslt;
            string descr;

            rslt = _zipLookup.SplitAddress("100 E Main St North");
            descr = "Basic functionality test failed.";
            
            Assert.AreEqual(rslt.Prefix, "E", descr);
            Assert.AreEqual(rslt.Street, "MAIN", descr);
            Assert.AreEqual(rslt.Suffix, "ST", descr);
            Assert.AreEqual(rslt.Suffix2, "N", descr);
            Assert.AreEqual(rslt.StreetNum, "100", descr);

            rslt = _zipLookup.SplitAddress("100 E Main St Apt #471 C");
            descr = "Secondary address test failed.";

            Assert.AreEqual(rslt.SecondaryAbbr, "APT", descr);
            Assert.AreEqual(rslt.SecondaryNum, "471C", descr);

            rslt = _zipLookup.SplitAddress("100 E Main St Suite 5 1/2");
            descr = "Secondary address with fraction test failed.";

            Assert.AreEqual(rslt.SecondaryAbbr, "STE", descr);
            Assert.AreEqual(rslt.SecondaryNum, "5 1/2", descr);

            rslt = _zipLookup.SplitAddress("100 E Main St 9th Floor");
            descr = "Secondary address with ordinal secondary test failed.";

            Assert.AreEqual(rslt.SecondaryAbbr, "FL", descr);
            Assert.AreEqual(rslt.SecondaryNum, "9", descr);

            rslt = _zipLookup.SplitAddress("HC 1 Box 72A");
            descr = "RR with Box test failed.";

            Assert.AreEqual(rslt.Street, "HC 1", descr);
            Assert.AreEqual(rslt.StreetNum, "72A", descr);

            rslt = _zipLookup.SplitAddress("PO Box 40234");
            descr = "PO Box test failed.";

            Assert.AreEqual(rslt.Street, "PO BOX", descr);
            Assert.AreEqual(rslt.StreetNum, "40234", descr);

            rslt = _zipLookup.SplitAddress("Box C");
            descr = "Box test failed.";

            Assert.AreEqual(rslt.Street, "PO BOX", descr);
            Assert.AreEqual(rslt.StreetNum, "C", descr);

            rslt = _zipLookup.SplitAddress("One West Main");
            descr = "Street number 'One' test failed.";

            Assert.AreEqual(rslt.Street, "MAIN", descr);
            Assert.AreEqual(rslt.StreetNum, "ONE", descr);
            Assert.AreEqual(rslt.Prefix, "W", descr);

            rslt = _zipLookup.SplitAddress("A9A Main St");
            descr = "Non-numeric street number test failed.";

            Assert.AreEqual(rslt.Street, "MAIN", descr);
            Assert.AreEqual(rslt.StreetNum, "A9A", descr);
            Assert.AreEqual(rslt.Suffix, "ST", descr);

            rslt = _zipLookup.SplitAddress("125 E 700 S");
            descr = "Grid style test failed.";

            Assert.AreEqual(rslt.StreetNum, "125", descr);
            Assert.AreEqual(rslt.Prefix, "E", descr);
            Assert.AreEqual(rslt.Street, "700", descr);
            Assert.AreEqual(rslt.Suffix2, "S", descr);

        }
    }
}
