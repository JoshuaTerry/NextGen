using DDI.Shared.Enums.GL;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DDI.Shared.Models.Client.GL
{
    public class PostedTransaction : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public Guid? LedgerAccountYearId { get; set; }
        [ForeignKey(nameof(LedgerAccountYearId))]
        public LedgerAccountYear LedgerAccountYear { get; set; }
        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }

        public byte PeriodNumber { get; set; }

        public PostedTranType TransactionType { get; set; }

        public DateTime? TransactionDate { get; set; }

        public decimal Amount { get; set; }

        //JLT What was this a reference to before
        // and what should it be referencing now?
        //public DocumentType TranSource { get; set; }

        //JLT How is TransactionNumber different from TransactionId?
        public Int64 TransactionNumber { get; set; }

        public int LineNumber { get; set; }

        public string Description { get; set; }

        public int TransactionId { get; set; }

        public bool IsAdjustment { get; set; }

        public int LegacyKey { get; set; }

        public Guid? SubledgerTranId { get; set; }
        [ForeignKey(nameof(SubledgerTranId))]
        public SubledgerTransaction SubledgerTransaction { get; set; }

        //JLT What are these Tags?
        // Why do we need just 10?
        // Can they be a collection?
        public string Tag01 { get; set; }
        public string Tag02 { get; set; }
        public string Tag03 { get; set; }
        public string Tag04 { get; set; }
        public string Tag05 { get; set; }
        public string Tag06 { get; set; }         
        public string Tag07 { get; set; }         
        public string Tag08 { get; set; }         
        public string Tag09 { get; set; }         
        public string Tag10 { get; set; }
    }
}
