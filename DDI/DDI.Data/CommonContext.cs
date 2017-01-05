using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data.Models;
using DDI.Data.Models.Common;

namespace DDI.Data
{
	public class CommonContext : DbContext
	{
        #region Public Properties

        public DbSet<Abbreviation> Abbreviations { get; set; }
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
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        #endregion Public Constructors

        #region Method Overrides 
        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            BaseEntity entity = entityEntry.Entity as BaseEntity;
            if (entity != null)
            {
                //Ensure new entities have an ID
                if (entityEntry.State == EntityState.Added && entity.Id == default(Guid))
                {
                    entity.AssignPrimaryKey();
                }
            }

            return base.ValidateEntity(entityEntry, items);
        }
        #endregion
    }


}
