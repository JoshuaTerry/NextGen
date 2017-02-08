using System;
using System.Collections.Generic;
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
            query.MustBeInList(p => p.AlternateIds, "S1233");
            query.MustNotBeInList(p => p.AlternateIds, "DSD66+XXYYZ");
            var results = repo.DocumentSearch(query, 10, 0);
            if (results.TotalCount == 0)
            {
                Console.WriteLine("Not found.");
            }
            foreach (var item in results.Documents)
            {
                Console.WriteLine(item.Name);
            }

            Console.ReadLine();

        }
    }
}
