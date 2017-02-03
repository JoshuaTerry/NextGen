using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Common;
using DDI.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DDI.Shared.Models.Common;
using DDI.Shared;
using DDI.Shared.Enums.Common;
using DDI.Business.Tests.Common.DataSources;
using DDI.Shared.Statics.CRM;

namespace DDI.Business.Tests.Common
{
    [TestClass]
    public class ZipLookupTest : TestBase
    {
        private ZipLookup _zipLookup;
        private UnitOfWorkNoDb _uow;
        private IList<Country> _countries;
        private IList<State> _states;
        private Country _us;

        [TestInitialize]
        public void Initialize()
        {
            _uow = new UnitOfWorkNoDb();
            AbbreviationDataSource.GetDataSource(_uow);
            _countries = CountryDataSource.GetDataSource(_uow);
            _states = StateDataSource.GetDataSource(_uow);
            CountyDataSource.GetDataSource(_uow);

            _us = _countries.FirstOrDefault(p => p.ISOCode == AddressDefaults.DefaultCountryCode);

            _zipLookup = new ZipLookup(_uow);
            _zipLookup.Initialize();
        }


        private const string TESTDESCR = "Business | Common";

        [TestMethod, TestCategory(TESTDESCR)]
        public void ZipLookup_AbbreviateWords()
        {
            // This method assumes words are capitalized.
            Assert.AreEqual(_zipLookup.AbbreviateWords("NORTH RIVER ROAD"), "N RIV RD", "Basic functionality test.");
            // Periods should be omitted, other non-letters should be included.
            Assert.AreEqual(_zipLookup.AbbreviateWords("N. A-B ROAD"), "N A-B RD", "Period should be omitted.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void ZipLookup_SplitNumber()
        {

            CollectionAssert.AreEquivalent(_zipLookup.SplitNumber("123ABC456"), new string[] { "123", "ABC", "456" }, "Basic functionality.");
            CollectionAssert.AreEquivalent(_zipLookup.SplitNumber("3 1/2"), new string[] { "3", " ", "1/2" }, "Fractions shouldn't be split.");
        }

        /// <summary>
        /// Test WordSplit: Split an address into words delimited by space or other delimiters.
        /// </summary>
        [TestMethod, TestCategory(TESTDESCR)]
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
        [TestMethod, TestCategory(TESTDESCR)]
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

            Assert.IsTrue(_zipLookup.CompareAddressNumber("FIVE", "1", "9", EvenOddType.Any),
                "Written number test #1  failed.");

            Assert.IsTrue(_zipLookup.CompareAddressNumber("5", "FIVE", "FIVE", EvenOddType.Any),
                "Written number test #2 failed.");
        
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

        [TestMethod, TestCategory(TESTDESCR)]
        public void ZipLookup_Zip4Lookup()
        {
            CityDataSource.GetDataSource(_uow);
            ZipDataSource.GetDataSource(_uow);
            ZipBranchDataSource.GetDataSource(_uow);
            ZipStreetDataSource.GetDataSource(_uow);
            ZipPlus4DataSource.GetDataSource(_uow);
            State indiana = _states.FirstOrDefault(p => p.StateCode == "IN" && p.CountryId == _us.Id);

            string resultAddress;

            // Standard ZIP+4 lookup for DDI Address
            ZipLookupInfo zip4Lookup = new ZipLookupInfo()
            {
                AddressLine1 = "101 W Ohio St",
                AddressLine2 = "Suite 1650",
                PostalCode = "46204"
            };
            _zipLookup.Zip4Lookup(zip4Lookup, out resultAddress);

            Assert.AreEqual("46204-4201", zip4Lookup.PostalCode, "Zip+4 for 101 W. Ohio Suite 1650");
            Assert.IsNotNull(zip4Lookup.County);
            Assert.AreEqual("Marion", zip4Lookup.County.Description, "Zip+4 lookup returned Marion county");
            Assert.AreEqual("Indianapolis", zip4Lookup.City);

            // Zip+4 Lookup for a valid address using City/State instead of ZIP code.
            zip4Lookup = new ZipLookupInfo()
            {
                AddressLine1 = "102 W Ohio St",
                City = "Indianapolis",
                State = indiana
            };
            _zipLookup.Zip4Lookup(zip4Lookup, out resultAddress);
            Assert.AreEqual("46204-2153", zip4Lookup.PostalCode, "Zip for 102 W. Ohio via City/State");

            // Zip+4 lookup where ZIP code is incorrect, but it can be corrected.
            zip4Lookup = new ZipLookupInfo()
            {
                AddressLine1 = "102 W Ohio St",
                PostalCode = "46229",
                State = indiana
            };
            _zipLookup.Zip4Lookup(zip4Lookup, out resultAddress);
            Assert.AreEqual("46204-2153", zip4Lookup.PostalCode, "Corrected Zip for 102 W. Ohio via City/State");

            // Lookup on an invalid street address.  Should return county and city.
            zip4Lookup = new ZipLookupInfo()
            {
                AddressLine1 = "10223 W Ohio St",
                PostalCode = "46204"
            };
            _zipLookup.Zip4Lookup(zip4Lookup, out resultAddress);
            Assert.AreEqual("46204", zip4Lookup.PostalCode, "Zip for invalid street not modified.");
            Assert.IsNotNull(zip4Lookup.County);
            Assert.AreEqual("Marion", zip4Lookup.County.Description, "Zip+4 lookup returned Marion county");
            Assert.AreEqual("Indianapolis", zip4Lookup.City);

            // Lookup on City/State alone where ZIP code is unique and can be returned.
            zip4Lookup = new ZipLookupInfo()
            {
                AddressLine1 = "",
                City = "Beech Grove",
                State = indiana
            };

            _zipLookup.Zip4Lookup(zip4Lookup, out resultAddress);
            Assert.AreEqual("46107", zip4Lookup.PostalCode, "Singleton Zip Beech Grove");
            Assert.IsNotNull(zip4Lookup.County);
            Assert.AreEqual("Marion", zip4Lookup.County.Description, "Zip+4 lookup returned Marion county");

            // ZIP code on ambiguous street number address with double suffix
            zip4Lookup = new ZipLookupInfo()
            {
                AddressLine1 = "15380 Ten Point Drive",
                PostalCode = "46060"
            };
            _zipLookup.Zip4Lookup(zip4Lookup, out resultAddress);
            Assert.AreEqual("46060-7994", zip4Lookup.PostalCode, "Ten Point Drive");

            // ZIP code lookup with Apartment in range
            zip4Lookup = new ZipLookupInfo()
            {
                AddressLine1 = "347 S 8th St Apt 3",
                PostalCode = "46060"
            };
            _zipLookup.Zip4Lookup(zip4Lookup, out resultAddress);
            Assert.AreEqual("46060-2700", zip4Lookup.PostalCode, "Valid apt #");

            // ZIP code lookup with Apartment out of range
            zip4Lookup = new ZipLookupInfo()
            {
                AddressLine1 = "347 S 8th St Apt 5",
                PostalCode = "46060"
            };
            _zipLookup.Zip4Lookup(zip4Lookup, out resultAddress);
            Assert.AreEqual("46060-2714", zip4Lookup.PostalCode, "Invalid apt #");

            // ZIP code lookup with alpha Apartment
            zip4Lookup = new ZipLookupInfo()
            {
                AddressLine1 = "304 S 8th St Apt E",
                PostalCode = "46060"
            };
            _zipLookup.Zip4Lookup(zip4Lookup, out resultAddress);
            Assert.AreEqual("46060-2710", zip4Lookup.PostalCode, "Alpha apartment");

            // ZIP code lookup with fractional street number
            zip4Lookup = new ZipLookupInfo()
            {
                AddressLine1 = "998 1/2 South 8th Street",
                PostalCode = "46060"
            };
            _zipLookup.Zip4Lookup(zip4Lookup, out resultAddress);
            Assert.AreEqual("46060-3417", zip4Lookup.PostalCode, "Fractional street number");

            // Non-deliverable address
            zip4Lookup = new ZipLookupInfo()
            {
                AddressLine1 = "1005 S. 8th St.",
                PostalCode = "46060"
            };
            _zipLookup.Zip4Lookup(zip4Lookup, out resultAddress);
            Assert.AreEqual("46060", zip4Lookup.PostalCode, "Non-deliverable address");

            // PO Box
            zip4Lookup = new ZipLookupInfo()
            {
                AddressLine1 = "PO Box 2015",
                PostalCode = "46061"
            };
            _zipLookup.Zip4Lookup(zip4Lookup, out resultAddress);
            Assert.AreEqual("46061-2015", zip4Lookup.PostalCode, "PO Box");

            // Written street number
            zip4Lookup = new ZipLookupInfo()
            {
                AddressLine1 = "One West Ohio St.",
                PostalCode = "46204"
            };
            _zipLookup.Zip4Lookup(zip4Lookup, out resultAddress);
            Assert.AreEqual("46204-1916", zip4Lookup.PostalCode, "Written street number");
            
        }
    }
}
