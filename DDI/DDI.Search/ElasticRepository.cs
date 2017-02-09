using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Models;
using Nest;

namespace DDI.Search
{
    /// <summary>
    /// ElasticSearch strongly typed repository
    /// </summary>
    /// <typeparam name="T">Document type</typeparam>
    public class ElasticRepository<T> where T : class, ISearchDocument
    {

        private NestClient _client;

        public ElasticRepository() : this(new NestClient()) { }

        public ElasticRepository(NestClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Update a document in the Elasticsearch database.
        /// </summary>
        /// <param name="document"></param>
        public void Update(T document)
        {
            _client.IndexDocument(document);
        }

        /// <summary>
        /// Update a set of documents in the Elasticsearch database.
        /// </summary>
        /// <param name="documents">Set of documents to be updated.</param>
        /// <param name="pageSize">Number of documents to submit to Elasticsearch at a time.</param>
        /// <param name="onNext">Action to perform for each page.</param>
        /// <param name="onError">Action to perform if an exception is thrown.</param>
        /// <param name="onCompleted">Action to perform when all documents have been submitted for update.</param>
        /// <param name="indexName">Index name.</param>
        public void BulkUpdate(IEnumerable<T> documents, int pageSize, Action<long> onNext = null, Action<Exception> onError = null, Action onCompleted = null, string indexName = null)
        {
            _client.BulkIndexDocuments(documents, pageSize, onNext, onError, onCompleted, indexName); 
        }

        /// <summary>
        /// Create a new strongly typed ElasticQuery.
        /// </summary>
        /// <returns></returns>
        public ElasticQuery<T> CreateQuery()
        {
            return new ElasticQuery<T>();
        }

        /// <summary>
        /// Perform a document search, returning strongly typed search results.
        /// </summary>
        /// <param name="query">The ElasticQuery  </param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public DocumentSearchResult<T> DocumentSearch(ElasticQuery<T> query, int pageSize, int page)
        {
            ISearchRequest request = query.BuildSearchRequest();
            request.Size = pageSize;
            request.From = page * pageSize;
            var response = _client.ElasticClient.Search<T>(request);

            var result = new DocumentSearchResult<T>();
            result.ElasticClient = _client.ElasticClient;
            result.Request = request;
            result.Documents = response.Documents;
            result.TotalCount = (int)response.Total;

            return result;            
        }
    }


}
