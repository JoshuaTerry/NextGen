using DDI.Business.CRM;
using DDI.Shared.Models.Client.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using DDI.Shared;
using DDI.Data;
using DDI.Business.Tests.CRM.DataSources;
using DDI.Business.Tests.Common.DataSources;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics.CRM;

namespace DDI.Business.Tests
{
    [TestClass]
    public class RegionLogicTests
    {
        private const string TESTDESCR = "Business | CRM";
        private UnitOfWorkNoDb _uow;
        private IList<RegionArea> _regionAreas;
        private IList<Country> _countries;
        private IList<State> _states;
        private IList<County> _counties;
        private IList<Region> _regions;

        [TestInitialize]
        public void Initialize()
        {
            _uow = new UnitOfWorkNoDb();

            _countries = CountryDataSource.GetDataSource(_uow);
            _states = StateDataSource.GetDataSource(_uow);
            _counties = CountyDataSource.GetDataSource(_uow);
            _regions = RegionDataSource.GetDataSource(_uow);
            _regionAreas = RegionAreaDataSource.GetDataSource(_uow);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void RegionLogic_LoadAllPropertiesForRegionArea()
        {
            var logic = _uow.GetBusinessLogic<RegionLogic>();

            var area = _regionAreas.FirstOrDefault(p => p.Region.Code == "HAML");
            logic.LoadAllPropertiesForRegionArea(area);
            Assert.AreEqual(AddressDefaults.DefaultCountryCode, area.Country?.ISOCode, "Region area has correct Country loaded.");
            Assert.AreEqual("IN", area.State?.StateCode, "Region area has correct State loaded.");
            Assert.AreEqual("Hamilton", area.County?.Description, "Region area has correct County loaded.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void RegionLogic_GetRegionsByAddress()
        {
            var logic = _uow.GetBusinessLogic<RegionLogic>();

            var result = logic.GetRegionsByAddress(_countries.FirstOrDefault(p => p.ISOCode == AddressDefaults.DefaultCountryCode).Id,
                _states.FirstOrDefault(p => p.StateCode == "OH").Id, null, "Columbus", "43701");

            Assert.AreEqual(1, result.Count, "Exactly one region for state of Ohio");
            CollectionAssert.Contains(result, _regions.FirstOrDefault(p => p.Code == "OH"), "Match region area by state (Ohio)");

            result = logic.GetRegionsByAddress(_countries.FirstOrDefault(p => p.ISOCode == AddressDefaults.DefaultCountryCode).Id,
            _states.FirstOrDefault(p => p.StateCode == "IN").Id, null, "Indianapolis", "46204");

            Assert.AreEqual(2, result.Count, "Exactly two regions for Indiana 46204");
            CollectionAssert.Contains(result, _regions.FirstOrDefault(p => p.Code == "IN"), "Match region area by state (Indiana)");
            CollectionAssert.Contains(result, _regions.FirstOrDefault(p => p.Code == "INDY"), "Match region area by zip (46204)");

            result = logic.GetRegionsByAddress(_countries.FirstOrDefault(p => p.ISOCode == AddressDefaults.DefaultCountryCode).Id,
            _states.FirstOrDefault(p => p.StateCode == "IN").Id, 
            _counties.FirstOrDefault(p => p.State.StateCode == "IN" && p.Description == "Hamilton").Id, "Fishers", "46038");

            Assert.AreEqual(2, result.Count, "Exactly two regions for Fishers");
            CollectionAssert.Contains(result, _regions.FirstOrDefault(p => p.Code == "IN"), "Match region area by state (Indiana)");
            CollectionAssert.Contains(result, _regions.FirstOrDefault(p => p.Code == "FISH"), "Match region area by city (Fishers)");

            result = logic.GetRegionsByAddress(_countries.FirstOrDefault(p => p.ISOCode == AddressDefaults.DefaultCountryCode).Id,
            _states.FirstOrDefault(p => p.StateCode == "IN").Id,
            _counties.FirstOrDefault(p => p.State.StateCode == "IN" && p.Description == "Hamilton").Id, "Noblesville", "46061");

            Assert.AreEqual(2, result.Count, "Exactly two regions for Hamilton Co.");
            CollectionAssert.Contains(result, _regions.FirstOrDefault(p => p.Code == "IN"), "Match region area by state (Indiana)");
            CollectionAssert.Contains(result, _regions.FirstOrDefault(p => p.Code == "HAML"), "Match region area by county (Hamilton)");

        }
    }
}
