using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Models;
using Nest;
using System.Web;
using DDI.Shared.Extensions;

namespace DDI.Search
{
    /// <summary>
    /// ElasticSearch strongly typed repository
    /// </summary>
    /// <typeparam name="T">Document type</typeparam>
    public class ElasticRepository<T> where T : class, ISearchDocument
    {
        #region Private Fields

        private const int DEFAULT_SEARCH_LIMIT = 15;

        private NestClient _client;

        #endregion

        #region Constructors

        public ElasticRepository() : this(new NestClient()) { }

        public ElasticRepository(NestClient client)
        {
            _client = client;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The NestClient for the repository.
        /// </summary>
        public NestClient Client => _client;

        #endregion

        #region Public Methods

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
        /// <param name="indexName">Index name.  If null, use the default index for the document type.</param>
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
        /// <param name="limit">Number of results to return per page</param>
        /// <param name="offset">Page number (zero-based)</param>
        /// <returns></returns>
        public IDocumentSearchResult<T> DocumentSearch(ElasticQuery<T> query, int limit, int offset)
        {
            ISearchRequest request = query.BuildSearchRequest();

            if (limit <= 0)
            {
                limit = DEFAULT_SEARCH_LIMIT;
            }

            request.Size = limit;
            request.From = offset * limit;

            var response = _client.ElasticClient.Search<T>(request);

            var result = new DocumentSearchResult<T>();
            result.ElasticClient = _client.ElasticClient;
            result.Request = request;
            result.Documents = response.Documents;
            result.TotalCount = (int)response.Total;

            return result;            
        }

        /// <summary>
        /// Get the Json body for an ElasticQuery.
        /// </summary>
        public string GetQueryJsonBody(ElasticQuery<T> query)
        {
            var ms = new MemoryStream();

            Client.ElasticClient.Serializer.Serialize(query.GetQuery(), ms);
            return Encoding.UTF8.GetString(ms.ToArray()).Replace("\r\n", string.Empty).Replace("  ", " ");
        }

        /// <summary>
        /// Get the URI for a search command.
        /// </summary>
        public Uri GetSearchUri()
        {
            string index = IndexHelper.GetIndexAlias<T>();
            string document = typeof(T).GetAttribute<ElasticsearchTypeAttribute>()?.Name
                    ??
                    typeof(T).Name.ToLower();

            return new Uri(Client.Uri, $"{index}/{document}/_search");
        }

        #endregion

    }


}
