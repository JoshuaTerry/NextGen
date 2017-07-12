using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Attributes.Models;
using DDI.Shared.Models.Client.GL;

namespace DDI.Shared.Models.Client.CP
{
    [Table("MiscReceiptLine")]
    public class MiscReceiptLine : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public int LineNumber { get; set; }

        [MaxLength(255)]
        public string Comment { get; set; }
        
        public Guid? LedgerAccountId { get; set; }
        [ForeignKey(nameof(LedgerAccountId))]
        public LedgerAccount LedgerAccount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DeletedOn { get; set; }

        [DecimalPrecision(14, 2)]
        public decimal Amount { get; set; }

        public Guid MiscReceiptId { get; set; }
        [ForeignKey(nameof(MiscReceiptId))]
        public MiscReceipt MiscReceipt { get; set; }

        /// <summary>
        /// The client can set IsDeleted to TRUE prior to calling PATCH to cause line items to be deleted.
        /// </summary>
        [NotMapped]
        public bool IsDeleted { get; set; }

    }
}
