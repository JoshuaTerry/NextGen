using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    public class SubledgerTransaction : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }

        public int LegacyKey { get; set; }
         
        public Int64 TrasactionNumber { get; set; }

        public int LineNumber { get; set; }

        public int TransactionId { get; set; }

        public DateTime? TransactionDate { get; set; }

        public DateTime? PostDate { get; set; }

        //JLT what was this?  What should it be now?
        //public DocumentType DocumentType { get; set; }

        //JLT what was this?  What should it be now?
        //public BaseDocument Document { get; set; }

        //JLT what was this?  What should it be now?
        // public BaseDocumentLine DocumentLine { get; set; }

        public decimal Amount { get; set; }
        //JLT this was size 20.  What is this?  Should it be an Enum?
        public string AmountType { get; set; }

        public Guid? DebitAccountId { get; set; }
        [ForeignKey(nameof(DebitAccountId))]
        public LedgerAccountYear DebitAccount { get; set; }
        public Guid? CreditAccountId { get; set; }
        [ForeignKey(nameof(CreditAccountId))]
        public LedgerAccountYear CreditAccount { get; set; }

        public SubLedgerTransactionStatus Status { get; set; }

        public bool IsAdjustment { get; set; }

        public string Description { get; set; }
         
        public Guid? ReconciliationId { get; set; }
        [ForeignKey(nameof(ReconciliationId))]
        public Reconciliation Reconciliation { get; set; }

        public DateTime? DateCleared { get; set; }

        public ICollection<PostedTransaction> PostedTransaction { get; set; }

        public ICollection<SubLedgerTransfer> SubLedgerTransfers { get; set; }

         
        //JLT What is this?
        //public IList<BaseDocumentTran> DocumentTrans { get; set; }
    }
}
