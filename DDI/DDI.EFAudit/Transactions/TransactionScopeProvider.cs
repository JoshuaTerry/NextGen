using DDI.EFAudit.Exceptions;
using System;
using System.Data.Entity.Core;
using System.Threading.Tasks;
using System.Transactions;

namespace DDI.EFAudit.Transactions
{
    /// <summary>
    /// Wraps the given operations in a TransactionScope-based transaction
    /// </summary>
    public partial class TransactionScopeProvider : ITransactionProvider
    {
        private readonly TransactionOptions _options;

        public TransactionScopeProvider(TransactionOptions options)
        {
            this._options = options;
        }

        public void InTransaction(Action action)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, _options))
                {
                    action();
                    scope.Complete();
                }
            }
            catch (EntityException e)
            {
                if (ConflictingTransactionException.Matches(e))
                    throw new ConflictingTransactionException(e);
                else
                    throw;
            }
        }

        public async Task InTransactionAsync(Func<Task> taskAction)
        {
            // Short circuit
            if (taskAction == null)
                return;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, _options, TransactionScopeAsyncFlowOption.Enabled))
                {
                    await taskAction();
                    scope.Complete();
                }
            }
            catch (EntityException e)
            {
                if (ConflictingTransactionException.Matches(e))
                    throw new ConflictingTransactionException(e);

                throw;
            }
        }
    }
}
