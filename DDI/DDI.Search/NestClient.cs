using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Search.Models;
using DDI.Search.Statics;
using Nest;

namespace DDI.Search
{
    /// <summary>
    /// Nest (Elasticsearch via Nest) client.
    /// </summary>
    public class NestClient
    {
        #region Private Fields

        private ElasticClient _client;
        private Uri _uri;
        private ConnectionSettings _connectionSettings;
        private string _indexName = "demo"; // TODO:  This will need to be based on the "client code" once this has been established.

        #endregion

        #region Public Properties

        /// <summary>
        /// Nest ElasticClient
        /// </summary>
        public ElasticClient ElasticClient
        {
            get { return _client; }
            set { _client = value; }
        }

        #endregion

        #region Constructors

        public NestClient()
        {
            var configManager = new Shared.DDIConfigurationManager();
            _uri = new Uri(configManager.AppSettings["ElasticsearchUrl"]);
            IndexNames.Initialize(_indexName);

            _connectionSettings = new ConnectionSettings(_uri);
            _connectionSettings.MapDefaultTypeIndices(ms => new FluentDictionary<Type,string>(IndexNames.IndexAliases));
            _client = new ElasticClient(_connectionSettings);
        }

        #endregion

        #region Private Methods



        #endregion


        #region Public Methods

        /// <summary>
        /// Delete a specific.
        /// </summary>
        public void DeleteIndex(string indexName)
        {
            if (_client.IndexExists(new IndexExistsRequest(indexName)).Exists)
            {
                _client.DeleteIndex(indexName);
            }
        }

        /// <summary>
        /// Create the current version of an index, returning that index's name.
        /// </summary>
        public string CreateIndex(string indexSuffix)
        {
            // Build the mappings for this index suffix.
            var mappings = new MappingsDescriptor();

            foreach (Type type in IndexNames.GetTypesForIndexSuffix(indexSuffix))
            {
                mappings.Map(type, m => m.AutoMap());
            }

            string indexName = IndexNames.GetIndexName(indexSuffix);

            // Create an index descriptor
            var descriptor = new CreateIndexDescriptor(indexName)
                .Mappings(ms => mappings);

            // Finally, create the index.
            _client.CreateIndex(descriptor);

            return indexName;
        }

        /// <summary>
        /// Delete then create the alias for an index suffix.
        /// </summary>
        /// <param name="indexSuffix"></param>
        public void CreateAlias(string indexSuffix)
        {
            string alias = IndexNames.GetIndexAlias(indexSuffix);
            string index = IndexNames.GetIndexName(indexSuffix);

            _client.Alias(r => r.Remove(rs => rs.Alias(alias)).Add(aa => aa.Alias(alias).Index(index)));
        }

        public string GetCurrentIndexForAlias(string indexSuffix)
        {
            string alias = IndexNames.GetIndexAlias(indexSuffix);
            var response = _client.GetAlias(p => p.AllIndices());

            return string.Empty;         
        }

        /// <summary>
        /// Index a document.
        /// </summary>
        /// <param name="document">Document to index.</param>
        public void IndexDocument<T>(T document) where T : class
        {
            _client.Index(document);
        }

        /// <summary>
        /// Index a document using specified index name.
        /// </summary>
        /// <param name="document">Document to index.</param>
        /// <param name="indexName">Index name.</param>
        public void IndexDocument<T>(T document, string indexName) where T : class
        {
            _client.Index(document, p => p.Index(indexName));
        }

        /// <summary>
        /// Index a batch of documents.
        /// </summary>
        /// <param name="documents">Set of documents to be updated.</param>
        /// <param name="pageSize">Number of documents to submit to Elasticsearch at a time.</param>
        /// <param name="onNext">Action to perform for each page.</param>
        /// <param name="onError">Action to perform if an exception is thrown.</param>
        /// <param name="onCompleted">Action to perform when all documents have been submitted for update.</param>
        /// <param name="indexName">Index name.</param>
        public void BulkIndexDocuments<T>(IEnumerable<T> documents, int pageSize, Action<long> onNext = null, Action<Exception> onError = null, Action onCompleted = null, string indexName = null) where T : class
        {
            BulkAllDescriptor<T> descriptor = new BulkAllDescriptor<T>(documents).BackOffRetries(2).BackOffTime("30s").MaxDegreeOfParallelism(4).Size(pageSize);
            if (!string.IsNullOrWhiteSpace(indexName))
            {
                descriptor.Index(indexName);
            }

            _client.BulkAll(descriptor) // Using recommended values.
                   .Subscribe(new BulkAllObserver(
                        onNext: n => onNext?.Invoke(n.Page * pageSize),
                        onError: onError,
                        onCompleted: onCompleted
                        ));
        }

        #endregion

    }
}
