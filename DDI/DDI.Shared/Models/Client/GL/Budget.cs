using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    public class Budget : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public Guid? AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }
        public BudgetType BudgetType { get; set; }
        public decimal YearAmount { get; set; }
         
        // JFA:
        // The above properties are needed.  Use decimal, not decimal?.  Column typename attribute needs to be specify as decimal(14,2).
         
        // JLT
        // What is the difference between PeriodAmount and Percent for 1 through 13?
        // Could this just be a collection of some object with these properties instead of the hard coded amounts or would that not 
        // make sense for how they are used?
        public decimal PeriodAmount01 { get; set; }         
        public decimal PeriodAmount02 { get; set; }
        public decimal PeriodAmount03 { get; set; }
        public decimal PeriodAmount04 { get; set; }
        public decimal PeriodAmount05 { get; set; }
        public decimal PeriodAmount06 { get; set; }
        public decimal PeriodAmount07 { get; set; }
        public decimal PeriodAmount08 { get; set; }
        public decimal PeriodAmount09 { get; set; }
        public decimal PeriodAmount10 { get; set; }
        public decimal PeriodAmount11 { get; set; }
        public decimal PeriodAmount12 { get; set; }
        public decimal PeriodAmount13 { get; set; }
        public decimal Percent01 { get; set; }
        public decimal Percent02 { get; set; }
        public decimal Percent03 { get; set; }
        public decimal Percent04 { get; set; }
        public decimal Percent05 { get; set; }
        public decimal Percent06 { get; set; }
        public decimal Percent07 { get; set; }
        public decimal Percent08 { get; set; }
        public decimal Percent09 { get; set; }
        public decimal Percent10 { get; set; }
        public decimal Percent11 { get; set; }
        public decimal Percent12 { get; set; }
        public decimal Percent13 { get; set; }
         
        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }
        public Guid? BusinessUnitId { get; set; }
        [ForeignKey(nameof(BusinessUnitId))]
        public BusinessUnit BusinessUnit { get; set; }
    }
}
