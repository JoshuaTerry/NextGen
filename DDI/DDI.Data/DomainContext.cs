using System.Data.Entity;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Client.Core;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using System;
using DDI.Shared.Models;

namespace DDI.Data
{
    public class DomainContext : DbContext
    {
        #region Public Properties

        public static string ConstituentNumberSequence => "CRM_ConstituentNumber";

        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<AddressType> AddressTypes { get; set; }

        public virtual DbSet<AlternateId> AlternateIds { get; set; }

        public virtual DbSet<ClergyStatus> ClergyStatuses { get; set; }

        public virtual DbSet<ClergyType> ClergyTypes { get; set; }

        public virtual DbSet<Configuration> Configurations { get; set; }

        public virtual DbSet<Constituent> Constituents { get; set; }

        public virtual DbSet<ConstituentAddress> ConstituentAddresses { get; set; }
         
        public virtual DbSet<ConstituentStatus> ConstituentStatuses { get; set; }

        public virtual DbSet<ConstituentType> ConstituentTypes { get; set; }

        public virtual DbSet<ContactInfo> ContactInfoes { get; set; }

        public virtual DbSet<ContactCategory> ContactCategories { get; set; }

        public virtual DbSet<ContactType> ContactTypes { get; set; }

        public virtual DbSet<CustomField> CustomField { get; set; }
        
        public virtual DbSet<CustomFieldData> CustomFieldData { get; set; }

        public virtual DbSet<CustomFieldOption> CustomFieldOption { get; set; }

        public virtual DbSet<Degree> Degrees { get; set; }

        public virtual DbSet<Denomination> Denominations { get; set; }

        public virtual DbSet<DoingBusinessAs> DoingBusinessAs { get; set; }

        public virtual DbSet<Education> Educations { get; set; }

        public virtual DbSet<EducationLevel> EducationLevels { get; set; }
         
        public virtual DbSet<Ethnicity> Ethnicities { get; set; }

        public virtual DbSet<Gender> Genders { get; set; }

        public virtual DbSet<IncomeLevel> IncomeLevels { get; set; }

        public virtual DbSet<Language> Languages { get; set; }

        public virtual DbSet<LogEntry> LogEntries { get; set; }

        public virtual DbSet<MaritalStatus> MaritalStatuses { get; set; }

        public virtual DbSet<PaymentPreference> PaymentPreferences { get; set; }

        public virtual DbSet<Prefix> Prefixes { get; set; }

        public virtual DbSet<Profession> Professions { get; set; }

        public virtual DbSet<Region> Regions { get; set; }

        public virtual DbSet<RegionArea> RegionAreas { get; set; }

        public virtual DbSet<RegionLevel> RegionLevels { get; set; }
        
        public virtual DbSet<Relationship> Relationships { get; set; }

        public virtual DbSet<RelationshipCategory> RelationshipCategories { get; set; }

        public virtual DbSet<RelationshipType> RelationshipTypes { get; set; }

        public virtual DbSet<School> Schools { get; set; }

        public virtual DbSet<SectionPreference> SectionPreferences { get; set; }

        public virtual DbSet<Tag> Tags { get; set; }

        public virtual DbSet<TagGroup> TagGroups { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public DomainContext()
            : base("name=DomainContext")
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
