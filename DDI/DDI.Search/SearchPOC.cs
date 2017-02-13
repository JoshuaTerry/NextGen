using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Search.Models;

namespace DDI.Search
{
    public static class SearchPOC
    {

        /// <summary>
        /// This class will go away with DC-367 (POC Cleanup).
        /// For now it demonstrates search functionality and can be called from DDI.Conversion.Program.cs.
        /// </summary>
        public static void PerformSearch()
        {
            var repo = new ElasticRepository<ConstituentDocument>();
            var query = repo.CreateQuery();
            
            var addrQuery = new ElasticQuery<ConstituentDocument>();
            addrQuery.Must.Match(p => p.Addresses[0].City, "Indianapolis");
            addrQuery.Must.Match(p => p.Addresses[0].StreetAddress, "Silver");

            query.Must.Nested(p => p.Addresses, addrQuery);


            //query.Must.BeInList(p => p.AlternateIds, "S1233,S3091");
            //query.MustNot.BeInList(p => p.AlternateIds, "DSD66");

            string body = repo.GetQueryJsonBody(query);
            Uri uri = repo.GetSearchUri();
                       
            var results = repo.DocumentSearch(query, 50, 0);
            if (results.TotalCount == 0)
            {
                Console.WriteLine("Not found.");
            }
            foreach (var item in results.Documents)
            {
                Console.WriteLine($"{item.ConstituentNumber}: {item.Name}, {item.PrimaryAddress}");
            }

            Console.ReadLine();

        }
    }
}
