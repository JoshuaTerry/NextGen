using DDI.Shared;
using DDI.Shared.Models;
using DDI.Shared.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using DDI.Data.Conventions;

namespace DDI.Data
{
    [DbConfigurationType(typeof(CustomDbConfiguration))]
    public class CommonContext : DbContext
	{
        private const string COMMON_CONTEXT_CONNECTION_KEY = "CommonContext";
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
            : base(ConnectionManager.Instance().Connections[COMMON_CONTEXT_CONNECTION_KEY])			 
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
                if (entityEntry.State == System.Data.Entity.EntityState.Added && entity.Id == default(Guid))
                {
                    entity.AssignPrimaryKey();
                }
            }

            return base.ValidateEntity(entityEntry, items);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new DecimalPrecisionAttributeConvention());
            modelBuilder.Conventions.Add(new StringLengthRequiredConvention());
        }

        #endregion
    } 
}
