using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DDI.Business.CRM;
using DDI.Data;
using DDI.Search;
using DDI.Search.Models;
using DDI.Search.Statics;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Conversion.CRM
{

    internal class ConstituentSearchIndexer : ConversionBase
    {
        private ElasticRepository<ConstituentDocument> _elasticRepository;
        
        public enum ConversionMethod
        {
            IndexConstituents = 200800,
        }

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            RunConversion(ConversionMethod.IndexConstituents, () => BuildConstituentIndex());
        }

        private void BuildConstituentIndex()
        {
            NestClient nestClient = new NestClient();
            
            nestClient.DeleteIndex(IndexHelper.GetIndexName(IndexSuffixes.CRM));
            string indexName = nestClient.CreateIndex(IndexSuffixes.CRM);

            _elasticRepository = new ElasticRepository<ConstituentDocument>(nestClient);
            
            ConstituentLogic bl;

            Console.WriteLine("Starting constituent indexing...");

            var query = new BatchUnitOfWork<Constituent>(p => p.ConstituentAddresses.First().Address,
                                                         p => p.ContactInfo,
                                                         p => p.ContactInfo.First().ContactType.ContactCategory,
                                                         p => p.AlternateIds,
                                                         p => p.ConstituentType);

            query.OnNextBatch = (count, batch) =>
            {
                bl = query.UnitOfWork.GetBusinessLogic<ConstituentLogic>();
                    IndexLogic(batch.Select(p => bl.BuildSearchDocument(p) as ConstituentDocument).Where(p => p != null), count * query.BatchSize, indexName);
            };

            query.Process();
            
            nestClient.CreateAlias(IndexSuffixes.CRM, true);
        }

        private void IndexLogic(IEnumerable<ConstituentDocument> constituents, int skip, string indexName)
        {
            var waitHandle = new CountdownEvent(1);

            _elasticRepository.BulkUpdate(constituents, 100, p => Console.WriteLine($"{skip + p}: {DateTime.Now.ToString("HH:mm:ss")}"), e => Console.WriteLine(e.Message), () => waitHandle.Signal(), indexName);

            waitHandle.Wait();
        }

    }
 
}
