using DDI.EFAudit.Contexts;
using DDI.EFAudit.Exceptions;
using DDI.EFAudit.Filter;
using DDI.EFAudit.Logging;
using DDI.EFAudit.Models;
using DDI.EFAudit.Transactions;
using System;
using System.Data.Entity.Core.Objects;
using System.Threading;
using System.Transactions;
using DDI.EFAudit.Translation;
using DDI.EFAudit.Translation.Serializers;

namespace DDI.EFAudit
{
    public partial class EFAuditModule<TChangeSet, TPrincipal>
        where TChangeSet : IChangeSet<TPrincipal>
    {
        public bool Enabled { get; set; }
        private IChangeSetFactory<TChangeSet, TPrincipal> factory;
        private IAuditLogContext<TChangeSet, TPrincipal> context;        
        private ILoggingFilter filter;
        private ISerializationManager serializer;

        public EFAuditModule(IChangeSetFactory<TChangeSet, TPrincipal> factory,
            IAuditLogContext<TChangeSet, TPrincipal> context,
            ILoggingFilterProvider filter = null,
            ISerializationManager serializer = null)
        {
            this.factory = factory;
            this.context = context;
            this.filter = (filter ?? Filters.Default).Get(context);
            this.serializer = (serializer ?? new ValueTranslationManager(context));
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
                return new SaveResult<TChangeSet, TPrincipal>(context.SaveAndAcceptChanges());

            var result = new SaveResult<TChangeSet,TPrincipal>();
            
            transactionProvider.InTransaction(() =>
            {
                var logger = new ChangeLogger<TChangeSet, TPrincipal>(context, factory, filter, serializer);
                var oven = (IOven<TChangeSet, TPrincipal>) null;
                 
                context.DetectChanges();
                 
                result.AffectedObjectCount = context.SaveAndAcceptChanges((sender, args) =>
                { 
                    oven = logger.Log(context.ObjectStateManager);                
                });
                 
                if (oven == null)
                    throw new ChangesNotDetectedException();
                 
                if (oven.HasChangeSet)
                { 
                    result.ChangeSet = oven.Bake(DateTime.Now, principal);
                    context.AddChangeSet(result.ChangeSet);
                    context.DetectChanges();
                     
                    context.SaveChanges(SaveOptions.AcceptAllChangesAfterSave);
                }
            });

            return result;
        }
    }
}
