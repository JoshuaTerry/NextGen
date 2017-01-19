using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DDI.Services;
using Moq;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;

namespace DDI.WebApi.Tests.Controllers
{
    [TestClass]
    public class RegionsServiceTest
    {
        private const string TESTDESCR = "WebApi | Service";

        [TestMethod, TestCategory(TESTDESCR)]
        public void GetRegions_ReturnsRegionsByLevel()
        {
            var repo = new Mock<IRepository<Region>>();
            repo.Setup(r => r.Entities).Returns(SetupRepositoryRegions());
            var service = new RegionService(repo.Object);
            var result = service.GetByLevel(1, null);

            Assert.IsTrue(result.Data.Count == 1);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void GetRegions_ReturnsRegionsByLevelAndId()
        {
            var repo = new Mock<IRepository<Region>>();
            repo.Setup(r => r.Entities).Returns(SetupRepositoryRegions());
            var service = new RegionService(repo.Object);
            var result = service.GetByLevel(2, new Guid("337E30A4-2162-4500-BD37-83947B22BA89"));

            Assert.IsTrue(result.Data.Count > 1);
            Assert.IsTrue(result.Data.All(a => a.Level == 2 && a.ParentRegionId == new Guid("337E30A4-2162-4500-BD37-83947B22BA89")));
        }

        private IQueryable<Region> SetupRepositoryRegions()
        {
            var regions = new List<Region>();
            regions.Add(new Region { Id = new Guid("337E30A4-2162-4500-BD37-83947B22BA89"), Level = 1, Code = "US", Name = "United States", ParentRegionId = null });
            regions.Add(new Region { Id = new Guid("BB8AC800-89D2-4C4F-B21F-8C001759540F"), Level = 2, Code = "98", Name = "Puerto Rico", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("D7DACEA9-495A-40FD-A565-9001255A4911"), Level = 2, Code = "9", Name = "Capital Area", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("5EA17DE7-FACD-40EE-A859-91CA3742ADBF"), Level = 2, Code = "63", Name = "Pacific Southwest", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("4B1450E4-8DF9-4880-AA4C-9327951F31C9"), Level = 2, Code = "17", Name = "Kansas", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("09C97628-91DB-45B1-B90E-966436C2B9F9"), Level = 2, Code = "39", Name = "Pennsylvania", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("AEEB0EED-2062-4413-BE4D-B04944FBF0A6"), Level = 2, Code = "26", Name = "Mid-America", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("BDF3867B-B50A-4BE1-89A9-B107D8146ADD"), Level = 2, Code = "46", Name = "Utah", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("27FDDF8F-F855-4137-A8E4-C676C32E1E4E"), Level = 2, Code = "23", Name = "Michigan", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("F9605C49-99E1-4FBB-843E-C735EF136C4C"), Level = 2, Code = "14", Name = "Illinois & Wisconsin", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("AE05055C-5603-476C-A897-CA5ACC435578"), Level = 2, Code = "42", Name = "South Carolina", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("8E0D4E4B-9077-462A-83A5-D03F1730DECF"), Level = 2, Code = "70", Name = "Kansas City, Greater", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("1CE0C18A-155A-4926-9AA5-D3308D26D397"), Level = 2, Code = "50", Name = "Northwest", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("F6C44D10-1968-4B9A-ACCE-D8832CA96AA6"), Level = 2, Code = "71", Name = "Great River", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("044DA6C4-978D-4A10-8A53-D9A8A98C4C2F"), Level = 2, Code = "5", Name = "California, Northern - Nevada", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("B1213CC0-92C8-4A10-95DD-E2F85B7E66CC"), Level = 2, Code = "10", Name = "Florida", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("684536D7-DABB-44F5-AA26-E44E3C560BDA"), Level = 2, Code = "16", Name = "Upper Midwest", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("70F9346C-9A30-4B97-8D73-F1B17BA3910B"), Level = 2, Code = "1", Name = "Alabama-Northwest Florida", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("20526AE7-9757-4BCD-BC5B-FD29ACB19603"), Level = 2, Code = "38", Name = "Oregon", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("E124C64E-4C43-4CB0-8C49-FD78FF853C57"), Level = 2, Code = "43", Name = "General Miscellaneous", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("BB98694A-F711-424A-9C75-068D2698675E"), Level = 2, Code = "34", Name = "North Carolina", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("9BB4E8AB-015B-417D-BD44-0ED0DDCBCAE5"), Level = 2, Code = "51", Name = "West Virginia", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("5F0D600B-AB3F-4EFF-A05C-1415C5E9BCB4"), Level = 2, Code = "15", Name = "Indiana", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("5E9138C7-2B91-41DC-BF61-181949A02F0D"), Level = 2, Code = "36", Name = "Ohio", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("C764E7B3-DAF6-488E-93A3-18A09F8BC9E3"), Level = 2, Code = "37", Name = "Oklahoma", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("A547E2B5-2D15-48CC-AD69-1BD2E769B6CA"), Level = 2, Code = "27", Name = "Montana", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("17D99988-55CA-489E-ABAD-2C2D85E6FC5C"), Level = 2, Code = "18", Name = "Kentucky", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("6B8606BA-9ADE-4AC6-A171-3432A59E7727"), Level = 2, Code = "44", Name = "Tennessee", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("5CDD3D3A-B404-46EE-8706-39D094122CDC"), Level = 2, Code = "4", Name = "Arkansas", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("1B1C5743-1757-4FA2-83FB-3F5F4349D79B"), Level = 2, Code = "3", Name = "Arizona", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("98543108-C2AE-4413-B7C0-40C821940E65"), Level = 2, Code = "11", Name = "Georgia", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("406E3354-B2FB-4509-B50D-47F35EB6C350"), Level = 2, Code = "13", Name = "Idaho-South", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("84F27B98-033E-41BE-9F25-4AE67442C488"), Level = 2, Code = "6", Name = "Central Rocky Mountain", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("630B5AA3-4432-4141-BF42-50B52B49E531"), Level = 2, Code = "48", Name = "Virginia", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("2D9268A2-647F-4C13-B08C-5252584B8F48"), Level = 2, Code = "33", Name = "Northeastern", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("4A5FEEB9-CD2F-485A-876F-58FEBC05EC5A"), Level = 2, Code = "54", Name = "Canada", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("F1FE35FF-346D-4BC5-AA20-68B303BC7DDF"), Level = 2, Code = "25", Name = "Mississippi", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("BC469A25-117C-4A98-8415-6A687BE4EC00"), Level = 2, Code = "19", Name = "Louisiana", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("93A956FE-E5CC-497F-864B-6E9B7007E239"), Level = 2, Code = "97", Name = "Other (Foreign)", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("EDCC8B66-C41B-484E-AB55-791E38DF0227"), Level = 2, Code = "45", Name = "Southwest", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            regions.Add(new Region { Id = new Guid("14733402-1260-43F7-8100-822F76DD9F81"), Level = 2, Code = "28", Name = "Nebraska", ParentRegionId = new Guid("337E30A4-2162-4500-BD37-83947B22BA89") });
            
            var response = regions.AsQueryable<Region>();
            return response;
        }
    }
}
