using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Models;
using Nest;

namespace DDI.Search
{
    /// <summary>
    /// Strongly typed Elasticsearch search results.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DocumentSearchResult<T> : IDocumentSearchResult<T> where T : class, ISearchDocument
    {
        public ElasticClient ElasticClient { get; set; }
        public ISearchRequest Request { get; set; }
        public IEnumerable<T> Documents { get; set; }
        public int TotalCount { get; set; }

        /// <summary>
        /// Get a specific page (0 based) of results.
        /// </summary>
        public void GetPage(int offset)
        {
            Request.From = offset * Request.Size;
            var response = ElasticClient.Search<T>(Request);

            Documents = response.Documents;
            TotalCount = (int)response.Total;
        }
    }
}
