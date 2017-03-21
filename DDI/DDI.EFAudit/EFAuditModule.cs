using DDI.EFAudit.Contexts;
using DDI.EFAudit.Exceptions;
using DDI.EFAudit.Filter;
using DDI.EFAudit.Logging;
using DDI.EFAudit.Transactions;
using DDI.EFAudit.Translation;
using DDI.EFAudit.Translation.Serializers;
using DDI.Logger;
using DDI.Shared;
using DDI.Shared.Caching;
using DDI.Shared.Models.Client.Audit;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Transactions;

namespace DDI.EFAudit
{
    public static class EFAuditModule
    {
        private static ILogger _logger = LoggerManager.GetLogger(typeof(EFAuditModule));
        private const string AuditEnabledTag = "AuditEnabled";
        private static object _syncRoot = new object();
        private static bool? isAuditEnabled = null;
        public static bool IsAuditEnabled
        {
            get
            {
                if (!isAuditEnabled.HasValue)
                {
                    lock(_syncRoot)
                    {
                        try
                        {
                            string auditTag = CacheHelper.GetEntry<string>(AuditEnabledTag, () => new DDIConfigurationManager().AppSettings["AuditEnabled"] ?? "false");
                            isAuditEnabled = string.Compare(auditTag, "true", true) == 0;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex);
                            isAuditEnabled = false;
                        }
                    }
                }
                
                return isAuditEnabled.Value;
            }
        }

    }
    public partial class EFAuditModule<TChangeSet, TPrincipal>  where TChangeSet : IChangeSet<TPrincipal>
    {
        public bool Enabled { get; set; }
        private IChangeSetFactory<TChangeSet, TPrincipal> _factory;
        private IAuditLogContext<TChangeSet, TPrincipal> _context;
        private ILoggingFilter _filter;
        private ISerializationManager _serializer;
        private DbContext _dbcontext;

        public EFAuditModule(IChangeSetFactory<TChangeSet, TPrincipal> factory,
            IAuditLogContext<TChangeSet, TPrincipal> context,
            DbContext dbcontext,
            ILoggingFilterProvider filter = null,
            ISerializationManager serializer = null)
        {
            this._factory = factory;
            this._context = context;
            this._dbcontext = dbcontext;
            this._filter = (filter ?? Filters.Default).Get(context);
            this._serializer = (serializer ?? new ValueTranslationManager(context));
            Enabled = true;
        }

        /// <summary>
        /// Save the changes and log them as controlled by the logging filter. 
        /// A TransactionScope is used to wrap save, which will use an ambient transaction if available, or create a new one.
        ///  
        /// If you are using an explicit transaction, and not using the TransactionScope Use SaveChangesWithinExplicitTransaction.
        /// </summary>
        public ISaveResult<TChangeSet> SaveChanges(TPrincipal principal)
        {
            return SaveChanges(principal, new TransactionOptions());
        }

        public ISaveResult<TChangeSet> SaveChanges(TPrincipal principal, TransactionOptions transactionOptions)
        {
            return SaveChanges(principal, new TransactionScopeProvider(transactionOptions));
        }

        public ISaveResult<TChangeSet> SaveChangesWithinExplicitTransaction(TPrincipal principal)
        {
            // If there is already an explicit transaction in use, we don't need to do anything
            // with transactions in EFAuditModule, so just use the NullTransactionProvider
            return SaveChanges(principal, new NullTransactionProvider());
        }

        protected ISaveResult<TChangeSet> SaveChanges(TPrincipal principal, ITransactionProvider transactionProvider)
        {
            if (!Enabled)
                return new SaveResult<TChangeSet, TPrincipal>(_context.SaveAndAcceptChanges());

            var result = new SaveResult<TChangeSet, TPrincipal>();

            transactionProvider.InTransaction(() =>
            {
                var logger = new ChangeLogger<TChangeSet, TPrincipal>(_context, _dbcontext, _factory, _filter, _serializer);
                var oven = (IOven<TChangeSet, TPrincipal>)null;

                _context.DetectChanges();

                result.AffectedObjectCount = _context.SaveAndAcceptChanges((sender, args) =>
                {
                    oven = logger.Log(_context.ObjectStateManager);
                });

                if (oven == null)
                    throw new ChangesNotDetectedException();

                if (oven.HasChangeSet)
                {
                    result.ChangeSet = oven.Bake(DateTime.UtcNow, principal);
                    _context.AddChangeSet(result.ChangeSet);
                    _context.DetectChanges();

                    _context.SaveChanges(SaveOptions.AcceptAllChangesAfterSave);
                }
            });

            return result;
        }
    }

}
