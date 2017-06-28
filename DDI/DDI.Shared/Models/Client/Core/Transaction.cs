using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Attributes.Models;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Enums.Core;

namespace DDI.Shared.Models.Client.Core
{
    [Table("Transaction")]
    public class Transaction : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }

        [Index("IX_TransactionNumber", Order = 1, IsUnique = true)]
        public Int64 TransactionNumber { get; set; }

        [Index("IX_TransactionNumber", Order = 2, IsUnique = true)]
        public int LineNumber { get; set; }

        public TransactionType TransactionType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set; }

        public DateTime? PostDate { get; set; }
        
        [DecimalPrecision(14, 2)]
        public decimal Amount { get; set; }

        public Guid? DebitAccountId { get; set; }
        [ForeignKey(nameof(DebitAccountId))]
        public LedgerAccountYear DebitAccount { get; set; }

        public Guid? CreditAccountId { get; set; }
        [ForeignKey(nameof(CreditAccountId))]
        public LedgerAccountYear CreditAccount { get; set; }

        public TransactionStatus Status { get; set; }

        public bool IsAdjustment { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [MaxLength(64)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreatedOn { get; set; }

        // TBD 
        // public Guid? ReconciliationId { get; set; }
        // [ForeignKey(nameof(ReconciliationId))]
        // public Reconciliation Reconciliation { get; set; }

        // public DateTime? DateCleared { get; set; }

        public ICollection<PostedTransaction> PostedTransactions { get; set; }
        
        public ICollection<EntityTransaction> EntityTransactions { get; set; }

        public override string DisplayName => $"{TransactionNumber}-{LineNumber}";

        [NotMapped]
        public IEntity EntityLine { get; set; } // For relating a transaction to an entity line item.

    }
}
