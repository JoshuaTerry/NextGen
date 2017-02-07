using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace DDI.Search
{
    /// <summary>
    /// ElasticSearch repository
    /// </summary>
    /// <typeparam name="T">Document type</typeparam>
    public class ElasticRepository<T> where T : class
    {
        private ElasticClient _client;
        private Uri _uri;
        private ConnectionSettings _connectionSettings;
        private string _indexName = "demo"; // TODO:  This will need to be based on the "client code" once this has been established.

        public ElasticRepository()
        {
            var configManager = new Shared.DDIConfigurationManager();
            _uri = new Uri(configManager.AppSettings["ElasticsearchUrl"]);

            _connectionSettings = new ConnectionSettings(_uri);
            _connectionSettings.DefaultIndex(_indexName);
            _client = new ElasticClient(_connectionSettings);
        }

        /// <summary>
        /// Update a document in the Elasticsearch database.
        /// </summary>
        /// <param name="document"></param>
        public void Update(T document)
        {
            var indexResp = _client.Index(document);            
        }
        
        /// <summary>
        /// Update a set of documents in the Elasticsearch database.
        /// </summary>
        /// <param name="documents">Set of documents to be updated.</param>
        /// <param name="pageSize">Number of documents to submit to Elasticsearch at a time.</param>
        /// <param name="onNext">Action to perform for each page.</param>
        /// <param name="onError">Action to perform if an exception is thrown.</param>
        /// <param name="onCompleted">Action to perform when all documents have been submitted for update.</param>
        public void BulkUpdate(IEnumerable<T> documents, int pageSize, Action<long> onNext = null, Action<Exception> onError = null, Action onCompleted = null)
        {
            var bulkAll = _client.BulkAll(documents, b => b.BackOffRetries(2).BackOffTime("30s").MaxDegreeOfParallelism(4).Size(pageSize)); // Using recommended values.
            bulkAll.Subscribe(new BulkAllObserver(
                onNext: n => onNext?.Invoke(n.Page * pageSize),
                onError: onError,
                onCompleted: onCompleted
                ));
        }
    }

}
