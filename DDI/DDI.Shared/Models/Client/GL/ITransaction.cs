using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Client.GL
{
    public interface ITransaction
    {
        Guid Id { get; set; }
        int TransactionId { get; set; }
        string Description { get; set; }
        DateTime TransactionDate { get; set; }
        decimal Amount { get; set; }

        Guid CreditLedgerAccountId { get; set; }
        [ForeignKey(nameof(CreditLedgerAccountId))]
        LedgerAccount CreditLedgerAccount { get; set; }

        Guid DebitLedgerAccountId { get; set; }
        [ForeignKey(nameof(DebitLedgerAccountId))]
        LedgerAccount DebitLedgerAccount { get; set; }
    }
}
