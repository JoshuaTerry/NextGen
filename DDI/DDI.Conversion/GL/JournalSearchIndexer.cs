using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using DDI.Business.GL;
using DDI.Data;
using DDI.Search;
using DDI.Search.Models;
using DDI.Search.Statics;
using DDI.Shared.Models.Client.GL;

namespace DDI.Conversion.GL
{

    internal class JournalSearchIndexer : ConversionBase
    {
        private ElasticRepository<JournalDocument> _elasticRepository;
        
        public enum ConversionMethod
        {
            IndexJournals = 70499,
        }

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            RunConversion(ConversionMethod.IndexJournals, () => BuildJournalIndex());
        }

        private void BuildJournalIndex()
        {
            NestClient nestClient = new NestClient();
            
            nestClient.DeleteIndex(IndexHelper.GetIndexName(IndexSuffixes.GL));
            string indexName = nestClient.CreateIndex(IndexSuffixes.GL);

            _elasticRepository = new ElasticRepository<JournalDocument>(nestClient);
            
            JournalLogic bl;

            Console.WriteLine("Starting journal indexing...");

            var query = new BatchUnitOfWork<Journal>(p => p.JournalLines,
                                                     p => p.FiscalYear.Ledger,
                                                     p => p.JournalLines);
            query.OnNextBatch = (count, batch) =>
            {
                bl = query.UnitOfWork.GetBusinessLogic<JournalLogic>();
                    IndexLogic(batch.Select(p => bl.BuildSearchDocument(p) as JournalDocument).Where(p => p != null), count * query.BatchSize, indexName);
            };

            query.Process();
            
            nestClient.CreateAlias(IndexSuffixes.GL, true);
        }

        private void IndexLogic(IEnumerable<JournalDocument> journals, int skip, string indexName)
        {
            var waitHandle = new CountdownEvent(1);

            _elasticRepository.BulkUpdate(journals, 100, p => Console.WriteLine($"{skip + p}: {DateTime.Now.ToString("HH:mm:ss")}"), e => Console.WriteLine(e.Message), () => waitHandle.Signal(), indexName);

            waitHandle.Wait();
        }

    }
 
}
