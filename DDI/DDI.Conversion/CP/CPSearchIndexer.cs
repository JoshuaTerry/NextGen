using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DDI.Business.CP;
using DDI.Search;
using DDI.Search.Models;
using DDI.Search.Statics;
using DDI.Shared;
using DDI.Shared.Models.Client.CP;

namespace DDI.Conversion.CP
{

    internal class CPSearchIndexer : ConversionBase
    {
        private ElasticRepository<MiscReceiptDocument> _elasticRepository;
        
        public enum ConversionMethod
        {
            IndexCP = 49999,
        }

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            RunConversion(ConversionMethod.IndexCP, () => BuildCPIndex());
        }

        private void BuildCPIndex()
        {
            NestClient nestClient = new NestClient();
            
            // Delete CP index.

            nestClient.DeleteIndex(IndexHelper.GetIndexName(IndexSuffixes.CP));
            string indexName = nestClient.CreateIndex(IndexSuffixes.CP);

            BuildMiscReceiptIndex(nestClient, indexName);

            
            // Finalize CP index.

            nestClient.CreateAlias(IndexSuffixes.CP, true);
        }

        private void BuildMiscReceiptIndex(NestClient nestClient, string indexName)
        {
            _elasticRepository = new ElasticRepository<MiscReceiptDocument>(nestClient);

            MiscReceiptLogic bl;

            Console.WriteLine("Starting misc. receipt indexing...");

            var query = new BatchUnitOfWork<MiscReceipt>(p => p.MiscReceiptLines,
                                                         p => p.FiscalYear.Ledger);
            query.OnNextBatch = (count, batch) =>
            {
                bl = query.UnitOfWork.GetBusinessLogic<MiscReceiptLogic>();
                MiscReceiptIndexLogic(batch.Select(p => bl.BuildSearchDocument(p) as MiscReceiptDocument).Where(p => p != null), count * query.BatchSize, indexName);
            };

            query.Process();

        }

        private void MiscReceiptIndexLogic(IEnumerable<MiscReceiptDocument> miscReceipts, int skip, string indexName)
        {
            var waitHandle = new CountdownEvent(1);

            _elasticRepository.BulkUpdate(miscReceipts, 100, p => Console.WriteLine($"{skip + p}: {DateTime.Now.ToString("HH:mm:ss")}"), e => Console.WriteLine(e.Message), () => waitHandle.Signal(), indexName);

            waitHandle.Wait();
        }

    }
 
}
