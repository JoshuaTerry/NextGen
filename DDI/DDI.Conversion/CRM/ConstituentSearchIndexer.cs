﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DDI.Business.CRM;
using DDI.Data;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Search;

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
            _elasticRepository = new ElasticRepository<ConstituentDocument>();
            ConstituentLogic bl;

            Console.WriteLine("Starting constituent indexing...");

            var query = new BatchUnitOfWork<Constituent>(p => p.ConstituentAddresses.First().Address, 
                                                         p => p.ContactInfo, 
                                                         p => p.ContactInfo.First().ContactType.ContactCategory, 
                                                         p => p.AlternateIds,
                                                         p => p.ConstituentType)
                .Where(p => p.ConstituentNumber == 40036);
            query.OnNextBatch = (count, batch) =>
            {
                bl = query.UnitOfWork.GetBusinessLogic<ConstituentLogic>();
                IndexLogic(batch.Select(p => bl.BuildSearchDocument(p) as ConstituentDocument), count * query.BatchSize);
            };

            query.Process();
        }

        private void IndexLogic(IEnumerable<ConstituentDocument> constituents, int skip)
        {
            var waitHandle = new CountdownEvent(1);

            _elasticRepository.BulkUpdate(constituents, 100, p => Console.WriteLine($"{skip + p}: {DateTime.Now.ToString("HH:mm:ss")}"), e => Console.WriteLine(e.Message), () => waitHandle.Signal());

            waitHandle.Wait();
        }

    }
 
}
