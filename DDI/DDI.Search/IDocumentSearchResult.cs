using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Models;
using Nest;

namespace DDI.Search
{
    public interface IDocumentSearchResult<T> where T : class, ISearchDocument
    {
        ElasticClient ElasticClient { get; set; }
        ISearchRequest Request { get; set; }
        IEnumerable<T> Documents { get; set; }
        int TotalCount { get; set; }

        /// <summary>
        /// Get a specific page (0 based) of results.
        /// </summary>
        void GetPage(int offset);
    }
}
