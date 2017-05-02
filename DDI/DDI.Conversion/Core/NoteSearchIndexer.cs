using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DDI.Business.Core;
using DDI.Data;
using DDI.Search;
using DDI.Search.Models;
using DDI.Search.Statics;
using DDI.Shared.Models.Client.Core;

namespace DDI.Conversion.Core
{

    internal class NoteSearchIndexer : ConversionBase
    {
        private ElasticRepository<NoteDocument> _elasticRepository;
        
        public enum ConversionMethod
        {
            IndexNotes = 990099,
        }

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            RunConversion(ConversionMethod.IndexNotes, () => BuildNoteIndex());
        }

        private void BuildNoteIndex()
        {
            NestClient nestClient = new NestClient();
            
            nestClient.DeleteIndex(IndexHelper.GetIndexName(IndexSuffixes.CORE));
            string indexName = nestClient.CreateIndex(IndexSuffixes.CORE);

            _elasticRepository = new ElasticRepository<NoteDocument>(nestClient);
            
            NoteLogic bl;

            Console.WriteLine("Starting note indexing...");

            var query = new BatchUnitOfWork<Note>();

            query.OnNextBatch = (count, batch) =>
            {
                bl = query.UnitOfWork.GetBusinessLogic<NoteLogic>();
                    IndexLogic(batch.Select(p => bl.BuildSearchDocument(p) as NoteDocument).Where(p => p != null), count * query.BatchSize, indexName);
            };

            query.Process();
            
            nestClient.CreateAlias(IndexSuffixes.CORE, true);
        }

        private void IndexLogic(IEnumerable<NoteDocument> notes, int skip, string indexName)
        {
            var waitHandle = new CountdownEvent(1);

            _elasticRepository.BulkUpdate(notes, 100, p => Console.WriteLine($"{skip + p}: {DateTime.Now.ToString("HH:mm:ss")}"), e => Console.WriteLine(e.Message), () => waitHandle.Signal(), indexName);

            waitHandle.Wait();
        }

    }
 
}
