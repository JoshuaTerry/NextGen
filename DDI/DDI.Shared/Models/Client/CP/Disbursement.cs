using DDI.Shared.Enums.CP;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Client.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.CP
{
    [Table("Disbursement")]
    public class Disbursement : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public DisbursementType DisbursementType { get; set; }

        public DateTime? DisbursementDate { get; set; }

        public Guid BankAccountId { get; set; }
        [ForeignKey("BankAccountId")]
        public BankAccount BankAccount { get; set; }

        public Guid LedgerAccountId { get; set; }
        [ForeignKey("LedgerAccountId")]
        public LedgerAccount LedgerAccount { get; set; }
        [MaxLength(128)]
        public string Payee { get; set; }

        public Guid PayeeAddressId { get; set; }
        [ForeignKey("PayeeAddressId")]
        public Address PayeeAddress { get; set; }

        [MaxLength(128)]
        public string PayeeLegacyKey { get; set; }

        public DateTime? VoidDate { get; set; }

        public DisbursementStatus DisbursementStatus { get; set; }

        [MaxLength(128)]
        public string VoidComment { get; set; }
        [MaxLength(128)]
        public string FinalPayee { get; set; }
        public DateTime? FinalDate { get; set; }
        [MaxLength(128)]
        public string FinalComment { get; set; }

        public Guid? CheckFromId { get; set; }
        [ForeignKey("CheckFromId")]
        public CheckFrom CheckFrom { get; set; }

        public bool IsImmediate { get; set; }
        public bool IsManual { get; set; }
        public EFTInfo EFTInfo { get; set; }
        public DateTime? ClearDate { get; set; }
    }
}
