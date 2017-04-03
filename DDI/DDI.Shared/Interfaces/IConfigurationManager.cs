using System.Collections.Specialized;
using System.Configuration;

namespace DDI.Shared
{
    public interface IConfigurationManager
    {
        NameValueCollection AppSettings { get; }
        ConnectionStringSettingsCollection ConnectionStrings { get; }
    }
}