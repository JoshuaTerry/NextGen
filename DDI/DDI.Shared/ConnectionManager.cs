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
        private static volatile ConnectionManager _instance;
        private static object _syncRoot = new object();
        private ReadOnlyDictionary<string, string> _connections;

        private ConnectionManager()
        {
            LoadConnections();
        }

        private void LoadConnections()
        {
            var connectionDictionary = new Dictionary<string, string>();
            var connectionStrings = ConfigurationManager.ConnectionStrings;

            foreach (ConnectionStringSettings kv in connectionStrings)
            {
                connectionDictionary.Add(kv.Name, GetDecryptedConnectionString(kv.ConnectionString));
            }

            Connections = new ReadOnlyDictionary<string, string>(connectionDictionary);
        }

        private string GetDecryptedConnectionString(string connection, string prefix = "Password=", string suffix = ";")
        {
            string encyrptedPassword = string.Empty;
            string decryptedConnection = string.Empty;

            if (!string.IsNullOrEmpty(prefix))
            {
                string connectionStart = connection.Substring(0, connection.IndexOf(prefix) + prefix.Length);
                string temp = connection.Replace(connectionStart, string.Empty);
                encyrptedPassword = string.IsNullOrEmpty(suffix) ? temp.Trim() : temp.Substring(0, temp.IndexOf(suffix)).Trim();
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
        public static ConnectionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new ConnectionManager();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
