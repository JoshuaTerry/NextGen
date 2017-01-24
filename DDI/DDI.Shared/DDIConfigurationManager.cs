using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared
{
    public class DDIConfigurationManager : IConfigurationManager
    {
        public NameValueCollection AppSettings => ConfigurationManager.AppSettings;

        public ConnectionStringSettingsCollection ConnectionStrings => ConfigurationManager.ConnectionStrings;
    }
}
