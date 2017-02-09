﻿using System;
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
            IndexHelper.Initialize(_indexName);

            _connectionSettings = new ConnectionSettings(_uri);
            _connectionSettings.MapDefaultTypeIndices(ms => new FluentDictionary<Type,string>(IndexHelper.IndexAliases));
            _client = new ElasticClient(_connectionSettings);
        }

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

            foreach (Type type in IndexHelper.GetTypesForIndexSuffix(indexSuffix))
            {
                mappings.Map(type, m => m.AutoMap());
            }

            string indexName = IndexHelper.GetIndexName(indexSuffix);

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
        public void CreateAlias(string indexSuffix, bool removeOldIndex = false)
        {
            string alias = IndexHelper.GetIndexAlias(indexSuffix);
            string newIndex = IndexHelper.GetIndexName(indexSuffix);

            // Build the alias descriptor.
            BulkAliasDescriptor descriptor = new BulkAliasDescriptor();
            string existingIndex = GetCurrentIndexForAlias(indexSuffix);
            if (!string.IsNullOrWhiteSpace(existingIndex))
            {
                descriptor.Remove(p => p.Alias(alias).Index(existingIndex));
            }
            
            descriptor.Add(p => p.Alias(alias).Index(newIndex));

            if (removeOldIndex && !string.IsNullOrWhiteSpace(existingIndex))
            {
                descriptor.Remove(p => p.Index(existingIndex));
            }
            
            // Execute the alias descriptor via the client.
            _client.Alias(d => descriptor);
        }

        /// <summary>
        /// Index a document.
        /// </summary>
        /// <param name="document">Document to index.</param>
        /// <param name="indexName">Specific index name.  If null, use the default index for document type.</param>
        public void IndexDocument<T>(T document, string indexName = null) where T : class
        {
            if (!string.IsNullOrWhiteSpace(indexName))
            {
                _client.Index(document, p => p.Index(indexName));

            }
            else
            {
                _client.Index(document);
            }
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

        #region Private Methods
        
        /// <summary>
        /// Given an index suffix, determine if an alias exists and return the current index name being used for the alias.
        /// </summary>
        private string GetCurrentIndexForAlias(string indexSuffix)
        {
            string alias = IndexHelper.GetIndexAlias(indexSuffix);

            var response = _client.GetAlias(p => p.AllIndices());
            return response.Indices.FirstOrDefault(p => p.Value.Any(a => a.Name == alias)).Key;
        }

        #endregion

    }
}
