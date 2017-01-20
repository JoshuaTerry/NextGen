using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared
{
    public sealed class ConnectionManager
    {
        private static ConnectionManager _instance;
        private static object _syncRoot = new object();
        private ReadOnlyDictionary<string, string> _connections;
        private static IConfigurationManager _configurationManager;

        private ConnectionManager(IConfigurationManager configurationManager = null)
        {
            if (configurationManager == null)
            {
                configurationManager = new DDIConfigurationManager();
            }
            _configurationManager = configurationManager;
            LoadConnections();
        }

        private void LoadConnections()
        {
            var connectionDictionary = new Dictionary<string, string>();
            var connectionStrings = _configurationManager.ConnectionStrings;

            foreach (ConnectionStringSettings connectionStringSetting in connectionStrings)
            {
                connectionDictionary.Add(connectionStringSetting.Name, GetDecryptedConnectionString(connectionStringSetting.ConnectionString));
            }

            Connections = new ReadOnlyDictionary<string, string>(connectionDictionary);
        }

        private string GetDecryptedConnectionString(string connection, string prefix = "Password=", string suffix = ";")
        {
            string decryptedConnection = string.Empty;

            if (!string.IsNullOrEmpty(prefix))
            {
                string connectionStart = connection.Substring(0, connection.IndexOf(prefix) + prefix.Length);
                string temp = connection.Replace(connectionStart, string.Empty);
                string encyrptedPassword = string.IsNullOrEmpty(suffix) ? temp.Trim() : temp.Substring(0, temp.IndexOf(suffix)).Trim();
                string connectionEnd = temp.Replace(encyrptedPassword, string.Empty);

                string decryptedPassword = Encryption.Decrypt(encyrptedPassword);
                decryptedConnection = string.Join(string.Empty, connectionStart, decryptedPassword, connectionEnd);
            }

            return decryptedConnection;
        }

        public ReadOnlyDictionary<string, string> Connections
        {
            get { return _connections; }
            private set { _connections = value; }
        }
        public static ConnectionManager Instance(IConfigurationManager configurationManager = null)
        {
            if (_instance == null)
            {
                lock (_syncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new ConnectionManager(configurationManager);
                    }
                }
            }
            return _instance;
        }
    }
}
