using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    public class AccountGroup : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; } 
        [MaxLength(255)]
        public string Description { get; set; }
        public int? Sequence { get; set; }
        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }
        public Guid? CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public AccountCategory Category { get; set; }

        //Parent & Child Groups
        //Group 1 thru 4 Accounts

        // JFA:
        // The above properties are needed in order to represent the chart of accounts in a grid with grouping.  
    }
}
