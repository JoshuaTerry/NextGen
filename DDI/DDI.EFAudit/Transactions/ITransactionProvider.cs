using System;
using System.Threading.Tasks;

namespace DDI.EFAudit.Transactions
{ 
    public interface ITransactionProvider
    {
        void InTransaction(Action action);
        Task InTransactionAsync(Func<Task> taskAction);
    }
}
