using DDI.Shared.Models.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System;
using System.Data.Entity.Validation;
using System.Collections.Generic;
using DDI.Shared.Models;
using DDI.Shared;

namespace DDI.Data
{
    public class CommonContext : DbContext
	{
        private const string _commonContextConnectionKey = "CommonContext";
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
            : base(ConnectionManager.Instance.Connections[_commonContextConnectionKey])			 
		{
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        #endregion Public Constructors

        #region Method Overrides 
        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            EntityBase entity = entityEntry.Entity as EntityBase;
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
