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
    [Table("Disbursement")]
    public class Disbursement : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public DisbursementType DisbursementType { get; set; }

        [DecimalPrecision(14, 2)]
        public decimal Amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DisbursementDate { get; set; }

        public int DisbursementNumber { get; set; }

        public DisbursementStatus DisbursementStatus { get; set; }

        public Guid? BankAccountId { get; set; }
        [ForeignKey(nameof(BankAccountId))]
        public BankAccount BankAccount { get; set; }

        public Guid? LedgerAccountId { get; set; }
        [ForeignKey(nameof(LedgerAccountId))]
        public LedgerAccount LedgerAccount { get; set; }

        public Guid? PayeeId { get; set; }
        [ForeignKey(nameof(PayeeId))]
        public Constituent Payee { get; set; }

        public Guid? PayeeAddressId { get; set; }
        [ForeignKey(nameof(PayeeAddressId))]
        public Address PayeeAddress { get; set; }

        [MaxLength(256)]
        public string FinalPayee { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? VoidDate { get; set; }

        [MaxLength(128)]
        public string VoidComment { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FinalDate { get; set; }

        [MaxLength(512)]
        public string FinalComment { get; set; }

        public bool IsImmediate { get; set; }

        public bool IsManual { get; set; } 

        [Column(TypeName = "date")]
        public DateTime? ClearDate { get; set; }

        [MaxLength(128)]
        public string BankName { get; set; }

        [MaxLength(64)]
        public string AccountNumber { get; set; }

        [MaxLength(64)]
        public string RoutingNumber { get; set; }

        public EFTAccountType EFTAccountType { get; set; }

        public Guid? EFTFormatId { get; set; }
        [ForeignKey(nameof(EFTFormatId))]
        public EFTFormat EFTFormat { get; set; }

        public ICollection<DisbursementLine> DisbursementLines { get; set; }

        public override string DisplayName => DisbursementNumber.ToString();

    }
}
