using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Shared.Test.Helpers
{
    [TestClass]
    public class PathHelperTests
    {
        private const string TESTDESCR = "Shared | Helpers";

        [TestMethod, TestCategory(TESTDESCR)]
        public void PathHelper_NameFor()
        {
            var result = PathHelper.NameFor<Constituent>(c => c.ConstituentAddresses, true);
            Assert.AreEqual("ConstituentAddresses", result);
            result = PathHelper.NameFor<Constituent>(c => c.Language.Code, true);
            Assert.AreEqual("Language.Code", result);
            result = PathHelper.NameFor<Constituent>(c => c.ConstituentAddresses.First().Address, true);
            Assert.AreEqual("ConstituentAddresses.Address", result);
        }

        [TestMethod, TestCategory(TESTDESCR)]
        public void PathHelper_FieldListBuilder()
        {
            string fields = new PathHelper.FieldListBuilder<Constituent>()
                .Include(p => p.FirstName)
                .IncludeReference<ConstituentAddress>(p => p.ConstituentAddresses)
                .Exclude(p => p.Address);

            Assert.AreEqual($"FirstName,ConstituentAddresses,ConstituentAddresses.{PathHelper.FieldExcludePrefix}Address", fields);
        }

    }
}
