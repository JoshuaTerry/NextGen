using DDI.Shared.Attributes.Models;
using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    [Table("AccountBudget")]
    public class AccountBudget : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Index("IX_Account_BudgetType", IsUnique = true, Order = 1)]
        public Guid? AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }

        [Index("IX_Account_BudgetType", IsUnique = true, Order = 2)]
        public BudgetType BudgetType { get; set; }

        [DecimalPrecision(14, 2)]
        public decimal YearAmount { get; set; }

        public PeriodAmountList Budget { get; set; } = new PeriodAmountList();

        public PeriodAmountList Percent { get; set; }

        public override string DisplayName => $"{Account?.DisplayName} {BudgetType}";

    }
}
