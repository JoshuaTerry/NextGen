using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Business.Helpers;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace DDI.Business.Tests.Helpers
{
    [TestClass]
    public class CodeEntityHelperTest
    {

        private const string TESTDESCR = "Business | Helpers";

        private IUnitOfWork _uow;
        private List<Tag> tags;

        [TestInitialize]
        public void Initialize()
        {
            tags = new List<Tag>();
            tags.Add(new Tag() { Code = "CH", Name = "Church", Id = GuidHelper.NewSequentialGuid(), IsActive = true });
            tags.Add(new Tag() { Code = "IN", Name = "Individual", Id = GuidHelper.NewSequentialGuid(), IsActive = true });
            tags.Add(new Tag() { Code = "XM", Name = "No Mail", Id = GuidHelper.NewSequentialGuid(), IsActive = true });

            Factory.ConfigureForTesting();

            _uow = Factory.CreateUnitOfWork();
            _uow.CreateRepositoryForDataSource(tags.AsQueryable());

        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void CodeEntityHelper_ConvertToGuid()
        {
            Guid idTag0 = tags[0].Id;
            Assert.AreEqual(idTag0, CodeEntityHelper.ConvertToGuid<Tag>(_uow, tags[0].Code), "ConvertToGuid, valid code.");
            Assert.AreEqual(idTag0, CodeEntityHelper.ConvertToGuid<Tag>(_uow, tags[0].Id.ToString()), "ConvertToGuid, Guid string.");
            Assert.IsNull(CodeEntityHelper.ConvertToGuid<Tag>(_uow, "ZZ"), "ConvertToGuid, invalid code.");
            Assert.IsNull(CodeEntityHelper.ConvertToGuid<Tag>(_uow, ""), "ConvertToGuid, empty string.");
            Assert.IsNull(CodeEntityHelper.ConvertToGuid<Tag>(_uow, null), "ConvertToGuid, null string.");
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void CodeEntityHelper_ConvertToGuid_Where()
        {
            Guid idTag0 = tags[0].Id;

            Assert.AreEqual(idTag0, CodeEntityHelper.ConvertToGuid<Tag>(_uow, tags[0].Code, p => p.IsActive == true), "ConvertToGuid, matching where predicate.");
            Assert.IsNull(CodeEntityHelper.ConvertToGuid<Tag>(_uow, tags[0].Code, p => p.IsActive == false), "ConvertToGuid, non-matching where predicate.");
        }


        [TestMethod, TestCategory(TESTDESCR)]
        public void CodeEntityHelper_ConvertCodeListToGuidList()
        {
            Guid idTag0 = tags[0].Id;
            Guid idTag1 = tags[1].Id;
            Guid idTag2 = tags[2].Id;

            string codeList = $"{tags[0].Code}+{tags[1].Code},{tags[2].Code}";
            string expectedResult = $"{idTag0}+{idTag1},{idTag2}";

            Assert.AreEqual(expectedResult, CodeEntityHelper.ConvertCodeListToGuidList<Tag>(_uow, codeList), $"ConvertToGuidList for {codeList}");

            codeList = codeList.Replace('+', '&').Replace(',', '|');
            expectedResult = expectedResult.Replace('+', '&').Replace(',', '|');

            Assert.AreEqual(expectedResult, CodeEntityHelper.ConvertCodeListToGuidList<Tag>(_uow, codeList), $"ConvertToGuidList for {codeList}");

            codeList = codeList.Replace('|', ' ');
            expectedResult = expectedResult.Replace('|', ' ');

            Assert.AreEqual(expectedResult, CodeEntityHelper.ConvertCodeListToGuidList<Tag>(_uow, codeList), $"ConvertToGuidList for {codeList}");

            Assert.IsNull(CodeEntityHelper.ConvertCodeListToGuidList<Tag>(_uow, ""), $"ConvertToGuidList for empty string");
            Assert.IsNull(CodeEntityHelper.ConvertCodeListToGuidList<Tag>(_uow, null), $"ConvertToGuidList for null string");

        }

    }
}
