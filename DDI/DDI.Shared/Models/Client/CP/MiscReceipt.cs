using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Attributes.Models;
using DDI.Shared.Enums.CP;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Client.GL;

namespace DDI.Shared.Models.Client.CP
{
    [Table("MiscReceipt")]
    public class MiscReceipt : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public int MiscReceiptNumber { get; set; }

        public MiscReceiptType MiscReceiptType { get; set; }

        public Guid? BusinessUnitId { get; set; }
        [ForeignKey(nameof(BusinessUnitId))]
        public BusinessUnit BusinessUnit { get; set; }

        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }

        [MaxLength(256)]
        public string Comment { get; set; }

        [DecimalPrecision(14, 2)]
        public decimal Amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set; }

        public bool IsReversed { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DeletionDate { get; set; }

        public Guid? ConstituentId { get; set; }
        [ForeignKey(nameof(ConstituentId))]
        public Constituent Constituent { get; set; }

        public Guid? DebitLedgerAccountId { get; set; }
        [ForeignKey(nameof(DebitLedgerAccountId))]
        public LedgerAccount DebitLedgerAccount { get; set; }

        public ICollection<MiscReceiptLine> MiscReceiptLines { get; set; }

        #region NotMapped Properties

        [NotMapped]
        public string MiscReceiptDescription { get; set; }

        [NotMapped]
        public string StatusDescription { get; set; }

        #endregion



    }
}
