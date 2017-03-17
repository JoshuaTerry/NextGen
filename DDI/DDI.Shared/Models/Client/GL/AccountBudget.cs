using DDI.Shared.Attributes.Models;
using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    public class AccountBudget : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid? AccountId { get; set; }
        public Account Account { get; set; }

        public BudgetType BudgetType { get; set; }

        [DecimalPrecision(14, 2)]
        public decimal YearAmount { get; set; }

        public PeriodAmountList Budget { get; set; }

        public PeriodAmountList Percent { get; set; }
         
        public Guid? FiscalYearId { get; set; }
        public FiscalYear FiscalYear { get; set; }


    }
}
