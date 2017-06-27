using DDI.Shared.Attributes.Models;
using DDI.Shared.Enums.Core;
using DDI.Shared.Enums.GL;
using DDI.Shared.Models.Client.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    [Table("PostedTransaction")]
    public class PostedTransaction : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Int64 TransactionNumber { get; set; }

        public Guid? LedgerAccountYearId { get; set; }
        [ForeignKey(nameof(LedgerAccountYearId))]
        public LedgerAccountYear LedgerAccountYear { get; set; }

        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }

        public int PeriodNumber { get; set; }

        public PostedTransactionType PostedTransactionType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set; }

        [DecimalPrecision(14, 2)]
        public decimal Amount { get; set; } 
        
        public int LineNumber { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public TransactionType TransactionType { get; set; }

        public bool IsAdjustment { get; set; }

        public Guid? TransactionId { get; set; }        
        [ForeignKey(nameof(TransactionId))]
        public Transaction Transaction { get; set; }
        

    }
}
