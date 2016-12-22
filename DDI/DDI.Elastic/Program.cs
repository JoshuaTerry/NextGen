using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Elastic
{
	class Program
	{
		static void Main(string[] args)
		{
			var indexer = new DataIndexer("http://localhost:9200/");

			var user = new User() { Username = "pevans", Name = "Paul Evans", Email = "pevans@ddi.org" };
			Console.WriteLine(indexer.IndexObject(user, user.Username));
			Console.WriteLine(indexer.GetIndex("user"));
		}
	}
}
