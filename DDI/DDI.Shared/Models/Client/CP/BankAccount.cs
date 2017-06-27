using DDI.Shared.Enums.CP;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.CP
{
    [Table("BankAccount")]
    public class BankAccount : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [MaxLength(128)]
        public string Description { get; set; }

        [Index("IX_Code", IsUnique = true), MaxLength(4)]
        public string Code { get; set; }

        [MaxLength(128)]
        public string CompanyName { get; set; }
        [MaxLength(128)]
        public string BankName { get; set; }
        [MaxLength(9)]
        public string BankRoutingNumber { get; set; }
        [MaxLength(128)]
        public string BankAccountNumber { get; set; }
        [MaxLength(128)]
        public string OriginNumber { get; set; }
        [MaxLength(128)]
        public string DestinationNumber { get; set; }
        [MaxLength(128)]
        public string CompanyIdNumber { get; set; }
        [MaxLength(128)]
        public string FileIdModifier { get; set; }
        public EFTAccountType BankAccountType { get; set; }
        public byte CheckDigits { get; set; }
        [MaxLength(128)]
        public string FractionalFormat { get; set; }
        [MaxLength(128)]
        public string OffsetRoutingNumber { get; set; }
        [MaxLength(128)]
        public string OffsetDescription { get; set; }

        public Guid? DefaultCheckFromId { get; set; }
        [ForeignKey("DefaultCheckFromId")]
        public CheckFrom DefaultCheckFrom { get; set; }

        public BankAccountStatus Status { get; set; }


    }
}
