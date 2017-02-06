using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Elasticsearch.Net;
using System.Configuration;
using DDI.Shared.Models.Search;

namespace DDI.Data
{
    public class ElasticRepository<T> where T : class
    {
        private ElasticClient _client;
        private Uri _uri;
        private ConnectionSettings _connectionSettings;
        private string _indexName = "demo";


        public ElasticRepository()
        {
            _uri = new Uri(ConfigurationManager.AppSettings["ElasticsearchUrl"]);
            _connectionSettings = new ConnectionSettings(_uri);
            _connectionSettings.DefaultIndex(_indexName);
            //_connectionSettings.MapPropertiesFor<Constituent>(p => { });
            /*
            var descriptor = new CreateIndexDescriptor(_indexName)
                .Mappings(ms => ms
                .Map<Constituent>(m => m.AutoMap()));            
            */
            _client = new ElasticClient(_connectionSettings);
            
        }
        
        public void Update(T document)
        {
            var indexResp = _client.Index(document);            
        }

        public void BulkUpdate(IEnumerable<T> documents, int pageSize, Action<long> onNext = null, Action<Exception> onError = null, Action onCompleted = null)
        {
            var bulkAll = _client.BulkAll(documents, b => b.BackOffRetries(2).BackOffTime("30s").MaxDegreeOfParallelism(4).Size(pageSize));
            bulkAll.Subscribe(new BulkAllObserver(
                onNext: n => onNext?.Invoke(n.Page * pageSize),
                onError: onError,
                onCompleted: onCompleted
                ));
        }
    }

}
