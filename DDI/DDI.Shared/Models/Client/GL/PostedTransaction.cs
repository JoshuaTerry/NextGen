using DDI.Shared.Enums.GL;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Attributes.Models;
using System.Collections.Generic;

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

        public PostedTranType TransactionType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set; }

        [DecimalPrecision(14, 2)]
        public decimal Amount { get; set; } 

        [MaxLength(4)]
        public string DocumentType { get; set; }
        
        public int LineNumber { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public int TransactionId { get; set; }

        public bool IsAdjustment { get; set; }

        public Guid? SubledgerTransactionId { get; set; }        
        [ForeignKey(nameof(SubledgerTransactionId))]
        public SubledgerTransaction SubledgerTransaction { get; set; }

    }
}
