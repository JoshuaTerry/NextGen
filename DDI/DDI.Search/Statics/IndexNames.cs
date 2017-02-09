using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Search.Models;
using DDI.Shared.Models;

namespace DDI.Search.Statics
{
    public static class IndexNames
    {
        public const string CRM = "crm";

        private static string _indexBaseName;
        private static Dictionary<Type, string> _indexSuffixes;
        private static Dictionary<string, string> _indexVersions;
        private static Dictionary<Type, string> _indexNames;
        private static Dictionary<Type, string> _indexAliases;

        public static Dictionary<Type, string> IndexAliases => _indexAliases;

        static IndexNames()
        {
            // Mapping of types to their index suffixes
            _indexSuffixes = new Dictionary<Type, string>()
            {
                { typeof(ConstituentDocument), CRM },
                { typeof(AddressDocument), CRM },
                { typeof(ContactInfoDocument), CRM },
            };

            // Mapping of index suffixes to their most recent versions.  These need to be updated any time the index needs to be rebuilt because the 
            // underyling documents have changed.
            _indexVersions = new Dictionary<string, string>
            {
                { CRM, "1" }
            };

        }

        private static void BuildIndexAliases()
        {
            _indexAliases = new Dictionary<Type, string>();
            foreach (var kvp in _indexSuffixes)
            {
                _indexAliases.Add(kvp.Key, FormatIndexAlias(_indexBaseName, kvp.Value));
            }
        }

        private static void BuildIndexNames()
        {
            _indexNames = new Dictionary<Type, string>();
            foreach (var kvp in _indexSuffixes)
            {
                _indexNames.Add(kvp.Key, FormatIndexName(_indexBaseName, kvp.Value, _indexVersions[kvp.Value]));
            }
        }

        public static void Initialize(string indexBaseName)
        {
            _indexBaseName = indexBaseName;

            if (_indexNames == null)
            {
                BuildIndexNames();
            }

            if (_indexAliases == null)
            {
                BuildIndexAliases();
            }
        }

        public static string GetIndexName<T>() where T : ISearchDocument
        {
            return _indexNames[typeof(T)];
        }

        public static string GetIndexName(string indexSuffix)
        {
            return FormatIndexName(_indexBaseName, indexSuffix, _indexVersions[indexSuffix]);
        }


        public static string GetIndexAlias<T>() where T : ISearchDocument
        {
            return _indexAliases[typeof(T)];
        }

        public static string GetIndexAlias(string indexSuffix)
        {
            return FormatIndexAlias(_indexBaseName, indexSuffix);
        }

        private static string FormatIndexName(string baseName, string indexSuffix, string version)
        {
            return $"{baseName}_{indexSuffix}_{version}";
        }

        private static string FormatIndexAlias(string baseName, string indexSuffix)
        {
            return $"{baseName}_{indexSuffix}";
        }

        public static IEnumerable<Type> GetTypesForIndexSuffix(string indexSuffix)
        {
            return _indexSuffixes.Where(p => p.Value == indexSuffix).Select(p => p.Key);
        }


    }
}
