using DDI.Shared.Enums.GL;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    public class Journal : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }
        public Guid? RecurringCodeId { get; set; }
        [ForeignKey(nameof(RecurringCodeId))]
        public RecurringCode RecurringCode { get; set; }

        public DateTime? PreviousDt { get; set; }

        public byte LockDay { get; set; }

        public Decimal ExpireAmount { get; set; } 

        public Decimal ExpireAmountTotal { get; set; }

        public int ExpireCount { get; set; }

        public int ExpireCountTotal { get; set; }

        public DateTime? ExpireDate { get; set; }

        public DateTime? ReverseDate { get; set; }

        public bool IsReversed { get; set; }

        public bool IsExpired { get; set; }

        public Guid? ParentJournalId { get; set; }
        [ForeignKey(nameof(ParentJournalId))]
        public Journal ParentJournal { get; set; }
         
        //JLT what was this what should it be?
       // public DocumentType OrigDocumentType { get; set; }
    }
}
