using System;
using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Models.Client.GL;

namespace DDI.Services.ServiceInterfaces
{
    public interface IPostedTransactionService : IService<PostedTransaction>
    {
        IEnumerable<PostedTransaction> GetPostedTransactionsForAccountId(Guid accountId);
        IEnumerable<PostedTransaction> GetPostedTransactionsForLedgerAccountYearId(Guid ledgerAccountId);
    }
}