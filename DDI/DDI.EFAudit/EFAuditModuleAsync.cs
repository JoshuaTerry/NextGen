using DDI.EFAudit.Exceptions;
using DDI.EFAudit.Logging;
using DDI.EFAudit.Transactions;
using DDI.Shared.Models.Client.Audit;
using System;
using System.Data.Entity.Core.Objects;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace DDI.EFAudit
{
    public partial class EFAuditModule<TChangeSet, TPrincipal>
        where TChangeSet : IChangeSet<TPrincipal>
    {
        /// <summary>        
        /// If you are using an explicit transaction, and not using the TransactionScope Use SaveChangesWithinExplicitTransaction.
        /// </summary>
        public async Task<ISaveResult<TChangeSet>> SaveChangesAsync(TPrincipal principal, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await SaveChangesAsync(principal, new TransactionOptions(), cancellationToken);
        }
        
        /// <summary>
        /// If you are using an explicit transaction, and not using the TransactionScope Use SaveChangesWithinExplicitTransaction.
        /// </summary>
        public async Task<ISaveResult<TChangeSet>> SaveChangesAsync(TPrincipal principal, TransactionOptions transactionOptions, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await saveChangesAsync(principal, new TransactionScopeProvider(transactionOptions), cancellationToken);
        }

        /// <summary>
        /// Save the changes and log them as controlled by the logging filter. 
        /// Only use this overload if you are wrapping the call to EFAuditModule in your own transaction. 
        /// This keeps me from automatically creating a transaction.        
        /// </summary>    
        public async Task<ISaveResult<TChangeSet>> SaveChangesWithinExplicitTransactionAsync(TPrincipal principal, CancellationToken cancellationToken = default(CancellationToken))
        {
            // If there is already an explicit transaction in use, use the NullTransactionProvider
            return await saveChangesAsync(principal, new NullTransactionProvider(), cancellationToken);
        }

        protected async Task<ISaveResult<TChangeSet>> saveChangesAsync(TPrincipal principal, ITransactionProvider transactionProvider, CancellationToken cancellationToken)
        {
            if (!Enabled)
                return new SaveResult<TChangeSet, TPrincipal>(await _context.SaveAndAcceptChangesAsync(cancellationToken: cancellationToken));

            var result = new SaveResult<TChangeSet, TPrincipal>();
            
            cancellationToken.ThrowIfCancellationRequested();

            await transactionProvider.InTransactionAsync(async () =>
            {
                var logger = new ChangeLogger<TChangeSet, TPrincipal>(_context, _dbcontext, _factory, _filter, _serializer);
                var oven = (IOven<TChangeSet, TPrincipal>)null;
                 
                cancellationToken.ThrowIfCancellationRequested(); 
                _context.DetectChanges(); 
                cancellationToken.ThrowIfCancellationRequested();

                result.AffectedObjectCount = await _context.SaveAndAcceptChangesAsync(cancellationToken: cancellationToken, onSavingChanges:
                    (sender, args) =>
                    {                        
                        cancellationToken.ThrowIfCancellationRequested();
                        oven = logger.Log(_context.ObjectStateManager);

                        // NOTE: This is the last chance to cancel the save.
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                );

                if (oven == null)
                    throw new ChangesNotDetectedException();
                
                if (oven.HasChangeSet)
                {                  
                    result.ChangeSet = oven.Bake(DateTime.UtcNow, principal);
                    _context.AddChangeSet(result.ChangeSet);
                    _context.DetectChanges();
                    
                    await _context.SaveChangesAsync(SaveOptions.AcceptAllChangesAfterSave);
                }
            });

            return result;
        }
    }
}
