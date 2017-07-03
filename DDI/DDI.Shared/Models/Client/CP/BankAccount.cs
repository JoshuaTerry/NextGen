using DDI.Shared.Enums.CP;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Models.Client.GL;

namespace DDI.Shared.Models.Client.CP
{
    [Table("BankAccount")]
    public class BankAccount : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [Index("IX_Code", IsUnique = true), MaxLength(4)]
        public string Code { get; set; }

        public Guid? BusinessUnitId { get; set; }
        [ForeignKey(nameof(BusinessUnitId))]
        public BusinessUnit BusinessUnit { get; set; }

        public Guid? DebitAccountId { get; set; }
        [ForeignKey(nameof(DebitAccountId))]
        public LedgerAccount DebitAccount { get; set; }

        public Guid? CreditAccountId { get; set; }
        [ForeignKey(nameof(CreditAccountId))]
        public LedgerAccount CreditAccount { get; set; }

        public EFTAccountType BankAccountType { get; set; }

        [MaxLength(30)]
        public string CompanyName { get; set; }

        [MaxLength(128)]
        public string BankName { get; set; }

        [MaxLength(9)]
        public string RoutingNumber { get; set; }

        [MaxLength(128)]
        public string AccountNumber { get; set; }

        [MaxLength(10)]
        public string OriginNumber { get; set; }

        [MaxLength(30)]
        public string OriginName { get; set; }

        [MaxLength(10)]
        public string DestinationNumber { get; set; }

        [MaxLength(30)]
        public string DestinationName { get; set; }

        [MaxLength(10)]
        public string CompanyIdNumber { get; set; }

        [MaxLength(10)]
        public string OriginatingFIDNumber { get; set; }

        [MaxLength(1)]
        public string FileIdModifier { get; set; }

        [MaxLength(30)]
        public string FractionalFormat { get; set; }

        [MaxLength(9)]
        public string OffsetRoutingNumber { get; set; }

        [MaxLength(30)]
        public string OffsetAccountNumber { get; set; }

        [MaxLength(30)]
        public string OffsetDescription { get; set; }

        public bool IsActive { get; set; }


    }
}
