using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Conversion.CRM
{
    using SearchConstituent = DDI.Shared.Models.Search.Constituent;

    internal class ConstituentSearchIndexer : ConversionBase
    {
        private ElasticRepository<SearchConstituent> _elasticRepository;
        private List<Guid> ids;

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
            _elasticRepository = new ElasticRepository<SearchConstituent>();

            /*
            Console.WriteLine("Building list of Ids to index");
            using (var uow = new UnitOfWorkEF())
            {
                ids = uow.GetEntities<Constituent>()
                         .OrderBy(p => p.ConstituentNumber)
                         .Select(p => p.Id)
                         .ToList();
            }
            */

            Console.WriteLine("Starting constituent indexing...");

            int take = 1000;
            int skip = 86100;

            while (true)
            {
                using (var bl = new Business.CRM.ConstituentLogic())
                {
                    var documentsToIndex =
                        
                        bl.UnitOfWork.GetEntities<Constituent>(p => p.ConstituentAddresses.First().Address, p => p.ContactInfo, p => p.ContactInfo.First().ContactType.ContactCategory, p => p.AlternateIds)
                        .OrderBy(p => p.ConstituentNumber)                        
                        //ids
                        .Skip(skip)
                        .Take(take)
                        //.Select(id => bl.UnitOfWork.GetById<Constituent>(id, p => p.ConstituentAddresses.First().Address, p => p.ContactInfo, p => p.ContactInfo.First().ContactType.ContactCategory, p => p.AlternateIds))
                        .AsEnumerable()
                        .Select(p => bl.BuildSearchDocument(p) as SearchConstituent);

                    if (documentsToIndex.Count() == 0)
                    {
                        break;
                    }

                    IndexLogic(documentsToIndex, skip);

                    skip += take;
                }
            }            
        }

        private void IndexLogic(IEnumerable<SearchConstituent> constituents, int skip)
        {
            var waitHandle = new CountdownEvent(1);

            _elasticRepository.BulkUpdate(constituents, 100, p => Console.WriteLine($"{skip + p}: {DateTime.Now.ToString("HH:mm:ss")}"), e => Console.WriteLine(e.Message), () => waitHandle.Signal());

            waitHandle.Wait();
        }

    }
 
}
