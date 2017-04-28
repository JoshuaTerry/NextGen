using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Search.Models;
using DDI.Search.Statics;
using DDI.Shared.Models;

namespace DDI.Search
{
    /// <summary>
    /// Helper class for defining and managing Elasticsearch indexes
    /// </summary>
    public static class IndexHelper
    {
        #region Private Fields

        private static string _indexBaseName;
        private static Dictionary<Type, string> _indexSuffixes;
        private static Dictionary<string, string> _indexVersions;
        private static Dictionary<Type, string> _indexNames;
        private static Dictionary<Type, string> _indexAliases;

        #endregion
        
        #region Public Properties

        /// <summary>
        /// Dictionary of index search document types and their index aliases.
        /// </summary>
        public static Dictionary<Type, string> IndexAliases => _indexAliases;

        #endregion

        #region Constructors

        static IndexHelper()
        {
            // Mapping of types to their index suffixes.  These need to be updated if new search document types are created.
            _indexSuffixes = new Dictionary<Type, string>()
            {
                { typeof(ConstituentDocument), IndexSuffixes.CRM },
                { typeof(AddressDocument), IndexSuffixes.CRM },
                { typeof(ContactInfoDocument), IndexSuffixes.CRM },
                { typeof(JournalDocument), IndexSuffixes.GL },
                { typeof(NoteDocument), IndexSuffixes.CORE }
            };

            // Mapping of index suffixes to their most recent versions.  
            // These need to be updated any time the index needs to be rebuilt because the underyling documents have changed,
            // or if new search document types are added.
            _indexVersions = new Dictionary<string, string>
            {
                { IndexSuffixes.CRM, "2.4" },
                { IndexSuffixes.GL, "1.0" },
                { IndexSuffixes.CORE, "1.0" }
            };
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///  Initialize static dictionaries.  This is called by the NestClient class to ensure the overhead applies only when actually using Elasticsearch.
        /// </summary>
        /// <param name="indexBaseName">Index base name (i.e. the client code)</param>
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

        /// <summary>
        /// Get a the current index name for a search document type.
        /// </summary>
        public static string GetIndexName<T>() where T : ISearchDocument
        {
            return _indexNames[typeof(T)];
        }

        /// <summary>
        ///  Get the currenet index name for a index suffix.
        /// </summary>
        public static string GetIndexName(string indexSuffix)
        {
            return FormatIndexName(_indexBaseName, indexSuffix, _indexVersions[indexSuffix]);
        }


        /// <summary>
        /// Get the index alias name to use for a document type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetIndexAlias<T>() where T : ISearchDocument
        {
            return _indexAliases[typeof(T)];
        }

        /// <summary>
        /// Get the index alias name that corresponds to a index suffix.
        /// </summary>
        public static string GetIndexAlias(string indexSuffix)
        {
            return FormatIndexAlias(_indexBaseName, indexSuffix);
        }


        /// <summary>
        /// Get the set of search document types that are indexed via the specified index suffix.
        /// </summary>
        public static IEnumerable<Type> GetTypesForIndexSuffix(string indexSuffix)
        {
            return _indexSuffixes.Where(p => p.Value == indexSuffix).Select(p => p.Key);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Build the dictionary of index aliases for each search document type.
        /// </summary>
        private static void BuildIndexAliases()
        {
            _indexAliases = new Dictionary<Type, string>();

            foreach (var kvp in _indexSuffixes)
            {
                _indexAliases.Add(kvp.Key, FormatIndexAlias(_indexBaseName, kvp.Value));
            }
        }

        /// <summary>
        /// Build the dictionary of index names for each search document type.
        /// </summary>
        private static void BuildIndexNames()
        {
            _indexNames = new Dictionary<Type, string>();
            foreach (var kvp in _indexSuffixes)
            {
                _indexNames.Add(kvp.Key, FormatIndexName(_indexBaseName, kvp.Value, _indexVersions[kvp.Value]));
            }
        }
        /// <summary>
        /// Format an index name given its parts.
        /// </summary>
        private static string FormatIndexName(string baseName, string indexSuffix, string version)
        {
            return $"{baseName}_{indexSuffix}_{version}";
        }

        /// <summary>
        /// Format an index alias name given its parts.
        /// </summary>
        private static string FormatIndexAlias(string baseName, string indexSuffix)
        {
            return $"{baseName}_{indexSuffix}";
        }



        #endregion

    }
}
