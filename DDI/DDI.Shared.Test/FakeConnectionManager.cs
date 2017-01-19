using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Test
{
    public class FakeConnectionManager : IConfigurationManager
    {
        private ConnectionStringSettingsCollection _connectionStrings = new ConnectionStringSettingsCollection();
        private NameValueCollection _appSettings = new NameValueCollection();
        public NameValueCollection AppSettings => _appSettings;

        public ConnectionStringSettingsCollection ConnectionStrings => _connectionStrings;

        public void AddConnectionString(string name, string connectionString)
        {
            _connectionStrings.Add(new ConnectionStringSettings(name, connectionString));
        }

        public void AddAppSetting(string name, string value)
        {
            _appSettings.Add(name, value);
        }
    }
}
