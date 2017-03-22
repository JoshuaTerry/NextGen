using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Attributes.Models;

namespace DDI.Shared.Models.Client.GL
{
    [Table("GL_SubledgerTransaction")]
    public class SubledgerTransaction : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }

        public Int64 TransactionNumber { get; set; }

        public int LineNumber { get; set; }

        public int TransactionId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set; }

        public DateTime? PostDate { get; set; }

        [MaxLength(4)]
        public string DocumentType { get; set; }

        // TBD
        // public BaseDocument Document { get; set; }

        // TBD
        // public BaseDocumentLine DocumentLine { get; set; }

        [DecimalPrecision(14, 2)]
        public decimal Amount { get; set; }

        // TBD
        // public string AmountType { get; set; }

        public Guid? DebitAccountId { get; set; }
        [ForeignKey(nameof(DebitAccountId))]
        public LedgerAccountYear DebitAccount { get; set; }

        public Guid? CreditAccountId { get; set; }
        [ForeignKey(nameof(CreditAccountId))]
        public LedgerAccountYear CreditAccount { get; set; }

        public SubLedgerTransactionStatus Status { get; set; }

        public bool IsAdjustment { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }
        
        // TBD 
        // public Guid? ReconciliationId { get; set; }
        // [ForeignKey(nameof(ReconciliationId))]
        // public Reconciliation Reconciliation { get; set; }

        // public DateTime? DateCleared { get; set; }

        public ICollection<PostedTransaction> PostedTransactions { get; set; }

        // TBD
        // public ICollection<SubLedgerXref> SubLedgerXrefs { get; set; }
         
        // TBD
        // public IList<BaseDocumentTran> DocumentTrans { get; set; }
    }
}
