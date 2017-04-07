using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Attributes.Models;
using DDI.Shared.Enums.Core;
using DDI.Shared.Enums.GL;

namespace DDI.Shared.Models.Client.GL
{
    [Table("Journal")]
    public class Journal : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public int JournalNumber { get; set; }

        public JournalType JournalType { get; set; }

        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }

        [MaxLength(255)]
        public string Comment { get; set; }

        [DecimalPrecision(14, 2)]
        public decimal Amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReverseOnDate { get; set; }

        public bool IsReversed { get; set; }

        public bool IsDeleted { get; set; }

        // Recurring journal properties

        public RecurringType RecurringType { get; set; }

        public RecurringDay RecurringDay { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PreviousDate { get; set; }

        public bool IsExpired { get; set; }

        [DecimalPrecision(14, 2)]
        public decimal ExpireAmount { get; set; }

        [DecimalPrecision(14, 2)]
        public decimal ExpireAmountTotal { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpireDate { get; set; }

        public int ExpireCount { get; set; }

        public int ExpireCountTotal { get; set; }

        public ICollection<JournalLine> JournalLines { get; set; }

        // Self relations

        public Guid? ParentJournalId { get; set; }
        [ForeignKey(nameof(ParentJournalId))]
        public Journal ParentJournal { get; set; }

        [InverseProperty(nameof(ParentJournal))]
        public ICollection<Journal> ChildJournals { get; set; }


    }
}
