using System.Collections.Specialized;
using System.Configuration;

namespace DDI.Shared
{
    public class DDIConfigurationManager : IConfigurationManager
    {
        public NameValueCollection AppSettings => ConfigurationManager.AppSettings;

        public ConnectionStringSettingsCollection ConnectionStrings => ConfigurationManager.ConnectionStrings;
    }
}
