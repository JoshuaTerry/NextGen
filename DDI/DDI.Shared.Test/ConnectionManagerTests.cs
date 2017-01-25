using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDI.Shared.Test
{
    [TestClass]
    public class ConnectionManagerTests
    {
        private const string TEST_DESCRIPTION = "SHARED";
        [TestMethod] [TestCategory(TEST_DESCRIPTION)]
        public void Should_DecryptConnectionStringProperly()
        {
            var connectionName = "Client";
            var configurationManager = new FakeConnectionManager();
            var password = "NewPresidentTomorrow";
            var encryptedPassword = "qARMfaRGIqgKB4lQwxsVbAf9G5MstYK4MkDp64Hx8zc=";
            var connectionString = "Data Source=tcp:10.10.10.1;Initial Catalog=DDI_DEV;User Id=Andrew_Luck;Password={0};MultipleActiveResultSets=True";
            var connectionStringEncrypted = String.Format(connectionString, encryptedPassword);
            configurationManager.AddConnectionString(connectionName, connectionStringEncrypted);
            var result = ConnectionManager.Instance(configurationManager).Connections[connectionName];

            var connectionStringDecrypted = String.Format(connectionString, password);
            Assert.AreEqual(connectionStringDecrypted, result);
        }
    }
}
