using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Search.Models;
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

            _connectionSettings = new ConnectionSettings(_uri);
            _connectionSettings.DefaultIndex(_indexName);
            _client = new ElasticClient(_connectionSettings);            
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Delete the default index.
        /// </summary>
        public void DeleteIndex()
        {
            if (_client.IndexExists(new IndexExistsRequest(Indices.Index(_indexName))).Exists)
            {
                _client.DeleteIndex(Indices.Index(_indexName));
            }
        }

        /// <summary>
        /// Create the default index.  
        /// </summary>
        public void CreateIndex()
        {
            // TODO:  This may need to be revisited during DC-367 (POC Cleanup) to have separate indexes per module (or something to that effect).
            //        Having to rebuild everything each time we add or update documents would be too disruptive.
            //        Unfortunately we have to let Nest create indexes via AutoMap() or Elasticsearch will use default
            //        mappings which are not suitable.  (In other words, the attributes in the search document model classes would be ignored.)
            var descriptor = new CreateIndexDescriptor(_indexName)
                .Mappings(ms => ms
                .Map<ConstituentDocument>(m => m.AutoMap())
                .Map<AddressDocument>(m => m.AutoMap())
                .Map<ContactInfoDocument>(m => m.AutoMap())
                );

            _client.CreateIndex(descriptor);
        }

        /// <summary>
        /// Index a document.
        /// </summary>
        /// <param name="document"></param>
        public void IndexDocument<T>(T document) where T : class
        {
            _client.Index(document);
        }

        /// <summary>
        /// Index a batch of documents.
        /// </summary>
        /// <param name="documents">Set of documents to be updated.</param>
        /// <param name="pageSize">Number of documents to submit to Elasticsearch at a time.</param>
        /// <param name="onNext">Action to perform for each page.</param>
        /// <param name="onError">Action to perform if an exception is thrown.</param>
        /// <param name="onCompleted">Action to perform when all documents have been submitted for update.</param>
        public void BulkIndexDocuments<T>(IEnumerable<T> documents, int pageSize, Action<long> onNext = null, Action<Exception> onError = null, Action onCompleted = null) where T : class
        {
            _client.BulkAll(documents, b => b.BackOffRetries(2).BackOffTime("30s").MaxDegreeOfParallelism(4).Size(pageSize)) // Using recommended values.
                   .Subscribe(new BulkAllObserver(
                        onNext: n => onNext?.Invoke(n.Page * pageSize),
                        onError: onError,
                        onCompleted: onCompleted
                        ));
        }

        #endregion

    }
}
