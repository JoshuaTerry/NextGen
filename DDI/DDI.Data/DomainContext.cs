using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using DDI.Shared;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CP;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Data
{
    public class DomainContext : DbContext
    {
        private const string DOMAIN_CONTEXT_CONNECTION_KEY = "DomainContext";
        #region Public Properties
         
        public static string ConstituentNumberSequence => "CRM_ConstituentNumber";

        #region Core Entities

        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<CustomField> CustomField { get; set; }
        public DbSet<CustomFieldData> CustomFieldData { get; set; }
        public DbSet<CustomFieldOption> CustomFieldOption { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<SectionPreference> SectionPreferences { get; set; }

        #endregion

        #region CRM Entities

        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<AlternateId> AlternateIds { get; set; }
        public DbSet<ClergyStatus> ClergyStatuses { get; set; }
        public DbSet<ClergyType> ClergyTypes { get; set; }
        public DbSet<Constituent> Constituents { get; set; }
        public DbSet<ConstituentAddress> ConstituentAddresses { get; set; }         
        public DbSet<ConstituentStatus> ConstituentStatuses { get; set; }
        public DbSet<ConstituentType> ConstituentTypes { get; set; }
        public DbSet<ContactInfo> ContactInfoes { get; set; }
        public DbSet<ContactCategory> ContactCategories { get; set; }
        public DbSet<ContactType> ContactTypes { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<Denomination> Denominations { get; set; }
        public DbSet<DoingBusinessAs> DoingBusinessAs { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<EducationLevel> EducationLevels { get; set; }         
        public DbSet<Ethnicity> Ethnicities { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<IncomeLevel> IncomeLevels { get; set; }
        public DbSet<MaritalStatus> MaritalStatuses { get; set; }
        public DbSet<Prefix> Prefixes { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<RegionArea> RegionAreas { get; set; }
        public DbSet<RegionLevel> RegionLevels { get; set; }        
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<RelationshipCategory> RelationshipCategories { get; set; }
        public DbSet<RelationshipType> RelationshipTypes { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagGroup> TagGroups { get; set; }

        #endregion

        #region CP Entities

        public DbSet<CardPaymentMethod> CardPaymentMethods { get; set; }
        public DbSet<EFTFormat> EFTFormats { get; set; }
        public DbSet<EFTPaymentMethod> EFTPaymentMethods { get; set; }
        public DbSet<PaymentMethodBase> PaymentMethods { get; set; }

        #endregion

        #endregion Public Properties


        #region Public Constructors

        public DomainContext()
            : base(ConnectionManager.Instance().Connections[DOMAIN_CONTEXT_CONNECTION_KEY])
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
