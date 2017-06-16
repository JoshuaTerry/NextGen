using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using DDI.Data.Conventions;
using DDI.EFAudit;
using DDI.EFAudit.Contexts;
using DDI.EFAudit.Filter;
using DDI.EFAudit.History;
using DDI.EFAudit.Logging;
using DDI.Logger;
using DDI.Shared;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.Audit;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.Security;

namespace DDI.Data
{
    [DbConfigurationType(typeof(CustomDbConfiguration))]
    public class DomainContext : DbContext
    {
        private const string DOMAIN_CONTEXT_CONNECTION_KEY = "DomainContext";
        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(DomainContext));
        #region Public Properties

        #region Core Entities

        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<CustomField> CustomField { get; set; }
        public DbSet<CustomFieldData> CustomFieldData { get; set; }
        public DbSet<CustomFieldOption> CustomFieldOption { get; set; }
        public DbSet<EntityMapping> EntityMapping { get; set; }
        public DbSet<SavedEntityMapping> SavedEntityMapping { get; set; }
        public DbSet<SavedEntityMappingField> SavedEntityMappingField { get; set; }
        public DbSet<Language> Languages { get; set; } 
        public DbSet<Note> Notes { get; set; }
        public DbSet<NoteCategory> NoteCategories { get; set; }
        public DbSet<NoteCode> NoteCodes { get; set; }
        public DbSet<NoteContactMethod> NoteContactMethods { get; set; }
        public DbSet<NoteTopic> NoteTopics { get; set; }
        public DbSet<SectionPreference> SectionPreferences { get; set; }
        public DbSet<FileStorage> FileStorage { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<EntityApproval> EntityApprovals { get; set; }
        public DbSet<EntityNumber> EntityNumbers { get; set; }
        public DbSet<EntityTransaction> EntityTransactions { get; set; }

        #endregion

        #region Internal Entities for tables not used in this solution.

        private DbSet<Models.TransactionXref> TransactionXrefs { get; set; }

        #endregion

        #region CRM Entities

        public DbSet<Shared.Models.Client.CRM.Address> CRM_Addresses { get; set; }
        public DbSet<Shared.Models.Client.CRM.AddressType> CRM_AddressTypes { get; set; }
        public DbSet<Shared.Models.Client.CRM.AlternateId> CRM_AlternateIds { get; set; }
        public DbSet<Shared.Models.Client.CRM.ClergyStatus> CRM_ClergyStatuses { get; set; }
        public DbSet<Shared.Models.Client.CRM.ClergyType> CRM_ClergyTypes { get; set; }
        public DbSet<Shared.Models.Client.CRM.Constituent> CRM_Constituents { get; set; }
        public DbSet<Shared.Models.Client.CRM.ConstituentAddress> CRM_ConstituentAddresses { get; set; }         
        public DbSet<Shared.Models.Client.CRM.ConstituentStatus> CRM_ConstituentStatuses { get; set; }
        public DbSet<Shared.Models.Client.CRM.ConstituentType> CRM_ConstituentTypes { get; set; }
        public DbSet<Shared.Models.Client.CRM.ConstituentPicture> CRM_ConstituentPictures { get; set; }
        public DbSet<Shared.Models.Client.CRM.ContactInfo> CRM_ContactInfoes { get; set; }
        public DbSet<Shared.Models.Client.CRM.ContactCategory> CRM_ContactCategories { get; set; }
        public DbSet<Shared.Models.Client.CRM.ContactType> CRM_ContactTypes { get; set; }
        public DbSet<Shared.Models.Client.CRM.Degree> CRM_Degrees { get; set; }
        public DbSet<Shared.Models.Client.CRM.Denomination> CRM_Denominations { get; set; }
        public DbSet<Shared.Models.Client.CRM.DoingBusinessAs> CRM_DoingBusinessAs { get; set; }
        public DbSet<Shared.Models.Client.CRM.Education> CRM_Educations { get; set; }
        public DbSet<Shared.Models.Client.CRM.EducationLevel> CRM_EducationLevels { get; set; }         
        public DbSet<Shared.Models.Client.CRM.Ethnicity> CRM_Ethnicities { get; set; }
        public DbSet<Shared.Models.Client.CRM.Gender> CRM_Genders { get; set; }
        public DbSet<Shared.Models.Client.CRM.IncomeLevel> CRM_IncomeLevels { get; set; }
        public DbSet<Shared.Models.Client.CRM.MaritalStatus> CRM_MaritalStatuses { get; set; }
        public DbSet<Shared.Models.Client.CRM.Prefix> CRM_Prefixes { get; set; }
        public DbSet<Shared.Models.Client.CRM.Profession> CRM_Professions { get; set; }
        public DbSet<Shared.Models.Client.CRM.Region> CRM_Regions { get; set; }
        public DbSet<Shared.Models.Client.CRM.RegionArea> CRM_RegionAreas { get; set; }
        public DbSet<Shared.Models.Client.CRM.RegionLevel> CRM_RegionLevels { get; set; }        
        public DbSet<Shared.Models.Client.CRM.Relationship> CRM_Relationships { get; set; }
        public DbSet<Shared.Models.Client.CRM.RelationshipCategory> CRM_RelationshipCategories { get; set; }
        public DbSet<Shared.Models.Client.CRM.RelationshipType> CRM_RelationshipTypes { get; set; }
        public DbSet<Shared.Models.Client.CRM.School> CRM_Schools { get; set; }
        public DbSet<Shared.Models.Client.CRM.Tag> CRM_Tags { get; set; }
        public DbSet<Shared.Models.Client.CRM.TagGroup> CRM_TagGroups { get; set; }

        #endregion

        #region CP Entities
        
        public DbSet<Shared.Models.Client.CP.EFTFormat> CP_EFTFormats { get; set; }
        public DbSet<Shared.Models.Client.CP.PaymentMethod> CP_PaymentMethods { get; set; }

        #endregion

        #region GL Entities

        public DbSet<Shared.Models.Client.GL.Account> GL_Accounts { get; set; }
        public DbSet<Shared.Models.Client.GL.AccountBalance> GL_AccountBalances { get; set; }
        public DbSet<Shared.Models.Client.GL.GLAccountSelection> GL_GLAccountSelection { get; set; }
        public DbSet<Shared.Models.Client.GL.AccountBudget> GL_AccountBudgets { get; set; }
        public DbSet<Shared.Models.Client.GL.AccountClose> GL_AccountCloses { get; set; }
        public DbSet<Shared.Models.Client.GL.AccountGroup> GL_AccountGroups { get; set; }
        public DbSet<Shared.Models.Client.GL.AccountPriorYear> GL_AccountPriorYears { get; set; }
        public DbSet<Shared.Models.Client.GL.AccountSegment> GL_AccountSegments { get; set; }
        public DbSet<Shared.Models.Client.GL.BusinessUnit> GL_BusinessUnits { get; set; }
        public DbSet<Shared.Models.Client.GL.BusinessUnitFromTo> GL_BusinessUnitFromTos { get; set; }
        public DbSet<Shared.Models.Client.GL.FiscalPeriod> GL_FiscalPeriods { get; set; }
        public DbSet<Shared.Models.Client.GL.FiscalYear> GL_FiscalYears { get; set; }
        public DbSet<Shared.Models.Client.GL.Fund> GL_Funds { get; set; }
        public DbSet<Shared.Models.Client.GL.FundFromTo> GL_FundFromTos { get; set; }
        public DbSet<Shared.Models.Client.GL.Ledger> GL_Ledgers { get; set; }
        public DbSet<Shared.Models.Client.GL.LedgerAccount> GL_LedgerAccounts { get; set; }
        public DbSet<Shared.Models.Client.GL.LedgerAccountMerge> GL_LedgerAccountMerges { get; set; }
        public DbSet<Shared.Models.Client.GL.LedgerAccountYear> GL_LedgerAccountYears { get; set; }
        public DbSet<Shared.Models.Client.GL.PostedTransaction> GL_PostedTransactions { get; set; }
        public DbSet<Shared.Models.Client.GL.Segment> GL_Segments { get; set; }
        public DbSet<Shared.Models.Client.GL.SegmentLevel> GL_SegmentLevels { get; set; }
        public DbSet<Shared.Models.Client.GL.Journal> GL_Journals { get; set; }
        public DbSet<Shared.Models.Client.GL.JournalLine> GL_JournalLines { get; set; }

        #endregion

        #region Auditing Entities

        public DbSet<ChangeSet> ChangeSets { get; set; }
        public DbSet<ObjectChange> ObjectChanges { get; set; }
        public DbSet<PropertyChange> PropertyChanges { get; set; }

        #endregion

        #region Security Entities

        public DbSet<User> Users { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }

        public DbSet<Group> Groups { get; set; }

        #endregion

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
        {

        }
        public DomainContext(Action<DbContext> customSaveChangesLogic = null, ILoggingFilterProvider filterProvider = null) : base(ConnectionManager.Instance().Connections[DOMAIN_CONTEXT_CONNECTION_KEY])
        {
            Database.SetInitializer<DomainContext>(new CreateDatabaseIfNotExists<DomainContext>());
            Logger = new EFAuditModule<ChangeSet, User>(new ChangeSetFactory(), AuditLogContext, this, filterProvider);
            CustomSaveChangesLogic = customSaveChangesLogic;
            this.Configuration.LazyLoadingEnabled = false;
            // Why is this false?
            this.Configuration.ProxyCreationEnabled = false;            
        }       
        #endregion Public Constructors

        #region Method Overrides  
        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            IEntity entity = entityEntry.Entity as IEntity;
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
        public async Task<ISaveResult<ChangeSet>> SaveAsync(User author, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Logger.SaveChangesAsync(author, cancellationToken);
        }
        public override int SaveChanges()
        {
            try
            {
                if (CustomSaveChangesLogic != null)
                    CustomSaveChangesLogic(this);

                return base.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                if (ex?.InnerException?.InnerException != null && ex.InnerException.InnerException is SqlException)
                {
                    var sqlException = ex.InnerException.InnerException as SqlException;
                    if (sqlException.Number == 2627 || sqlException.Number == 2601)
                    {                     
                        _logger.LogInformation(ex);
                        throw new DatabaseConstraintException();
                    }
                    else if (sqlException.Number == 547)
                    {
                        _logger.LogInformation(ex);
                        throw new DatabaseConstraintDeleteException();
                    }
                    else
                    {
                        _logger.LogError(ex);
                        throw ex;
                    }
                }
                else
                {
                    _logger.LogError(ex);
                    throw ex;
                }
            }
            catch
            {
                throw;
            }
        }

        private void ProcessDBExceptions(Exception ex)
        {
            var updateException = ex as DbUpdateException;
            if (updateException != null && updateException?.InnerException?.InnerException != null && updateException.InnerException.InnerException is SqlException)
            {
                var sqlException = updateException.InnerException.InnerException as SqlException;

                var sqlConstraintErrorNumbers = new List<int>(new int[] { 2627, 547, 2601 });
                if (sqlConstraintErrorNumbers.Contains(sqlException.Number))
                {
                    _logger.LogInformation(ex);
                    throw new DatabaseConstraintException();
                }
            }
            else
            {
                _logger.LogError(ex);
                throw ex;
            }

        }
      
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new DecimalPrecisionAttributeConvention());
        }

        public ISaveResult<ChangeSet> Save(User author)
        {
            try
            {
                // NOTE: This will eventually circle back and call our overridden SaveChanges() later
                return Logger.SaveChanges(author);
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                throw ex;
            }
        } 
        #endregion
    }
}
