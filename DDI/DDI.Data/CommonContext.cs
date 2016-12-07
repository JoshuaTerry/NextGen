using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DDI.Data.Models.Common;

namespace DDI.Data
{
	public class CommonContext : DbContext
	{
        #region Public Properties

        public DbSet<Abbreviation> AbbrevWords { get; set; }
        public DbSet<Thesaurus> ThesaurusWords { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<County> Counties { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<CityName> CityNames { get; set; }
        public DbSet<Zip> Zips { get; set; }
        public DbSet<ZipBranch> ZipBranches { get; set; }
        public DbSet<ZipStreet> ZipStreets { get; set; }
        public DbSet<ZipPlus4> ZipPlus4s { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public CommonContext()
			: base("name=CommonContext")
		{
		}

		#endregion Public Constructors
	}
}
