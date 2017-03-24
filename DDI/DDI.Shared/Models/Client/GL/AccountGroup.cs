using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    [Table("AccountGroup")]
    public class AccountGroup : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public int? Sequence { get; set; }

        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }

        public AccountCategory Category { get; set; }

        public Guid? ParentGroupId { get; set; }
        [ForeignKey(nameof(ParentGroupId))]
        public AccountGroup ParentGroup { get; set; }

        [InverseProperty(nameof(ParentGroup))]
        public ICollection<AccountGroup> ChildGroups { get; set; }

        [InverseProperty(nameof(Account.Group1))]
        public ICollection<Account> Group1Accounts { get; set; }

        [InverseProperty(nameof(Account.Group2))]
        public ICollection<Account> Group2Accounts { get; set; }

        [InverseProperty(nameof(Account.Group3))]
        public ICollection<Account> Group3Accounts { get; set; }

        [InverseProperty(nameof(Account.Group4))]
        public ICollection<Account> Group4Accounts { get; set; }

    }
}
