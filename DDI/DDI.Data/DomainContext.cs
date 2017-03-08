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
using DDI.EFAudit.Filter;
using DDI.Shared.Models.Client.Audit;
using DDI.EFAudit.Contexts;
using DDI.EFAudit.History;
using DDI.EFAudit;
using System.Threading.Tasks;
using System.Threading;
using DDI.EFAudit.Logging;
using DDI.Shared.Models.Client.Security;

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
        public DbSet<Note> Notes { get; set; }
        public DbSet<NoteCategory> NoteCategories { get; set; }
        public DbSet<NoteContactMethod> NoteContactCodes { get; set; }
        public DbSet<NoteTopic> NoteTopics { get; set; }
        public DbSet<NoteCode> NoteCodes { get; set; }
        public DbSet<SectionPreference> SectionPreferences { get; set; }
        public DbSet<FileStorage> FileStorage { get; set; }

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
        public DbSet<ConstituentPicture> ConstituentPictures { get; set; }
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
        
        public DbSet<EFTFormat> EFTFormats { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        #endregion

        public DbSet<ChangeSet> ChangeSets { get; set; }
        public DbSet<ObjectChange> ObjectChanges { get; set; }
        public DbSet<PropertyChange> PropertyChanges { get; set; } 

        public readonly EFAuditModule<ChangeSet, User> Logger;
        public IAuditLogContext<ChangeSet, User> AuditLogContext
        {
            get { return new DomainContextAdapter(this); }
        }
        public HistoryExplorer<ChangeSet, User> HistoryExplorer
        {
            get { return new HistoryExplorer<ChangeSet, User>(AuditLogContext); }
        }

        public Action<DbContext> CustomSaveChangesLogic { get; set; }
        #endregion
          
        #region Public Constructors
        public DomainContext() : this(null, null)
        { }
        public DomainContext(Action<DbContext> customSaveChangesLogic = null, ILoggingFilterProvider filterProvider = null) : base(ConnectionManager.Instance().Connections[DOMAIN_CONTEXT_CONNECTION_KEY])
        {
            // Commented this line out for now.  See http://stackoverflow.com/questions/14064434/ef5-getting-this-error-message-model-compatibility-cannot-be-checked-because-th
            // Basically compatibility cannot be checked.
            //Database.SetInitializer<DomainContext>(new DomainContextInitializer());

            Logger = new EFAuditModule<ChangeSet, User>(new ChangeSetFactory(), AuditLogContext, filterProvider);
            CustomSaveChangesLogic = customSaveChangesLogic;
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
        public async Task<ISaveResult<ChangeSet>> SaveAsync(User author, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Logger.SaveChangesAsync(author, cancellationToken);
        }
        public override int SaveChanges()
        {
            if (CustomSaveChangesLogic != null)
                CustomSaveChangesLogic(this);

            return base.SaveChanges();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins"); ;
            modelBuilder.Entity<Role>().ToTable("Roles"); ;
            modelBuilder.Entity<UserRole>().ToTable("UserRoles"); ;
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims"); ;
            modelBuilder.Ignore<Claim>();
        }

        public ISaveResult<ChangeSet> Save(User author)
        {
            // NOTE: This will eventually circle back and call our overridden SaveChanges() later
            return Logger.SaveChanges(author);
        } 
        #endregion

    }
}
