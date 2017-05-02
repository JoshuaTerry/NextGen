using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Attributes.Models;
using DDI.Shared.Enums.GL;

namespace DDI.Shared.Models.Client.GL
{
   
    public class AccountActivityDetail : ICanTransmogrify
    {
        public Guid Id { get; set; }

        public int PeriodNumber { get; set; }
        public string PeriodName { get; set; }

        public decimal BeginningBalance { get; set; }
        public decimal Debits { get; set; }
        public decimal Credits { get; set; }
        public decimal Activity { get; set; }
        public decimal EndingBalance { get; set; }

        public decimal PriorBeginningBalance { get; set; }
        public decimal PriorDebits { get; set; }
        public decimal PriorCredits { get; set; }
        public decimal PriorActivity { get; set; }
        public decimal PriorEndingBalance { get; set; }

        public decimal WorkingBudget { get; set; }
        public decimal FixedBudget { get; set; }
        public decimal WhatIfBudget { get; set; }

        public decimal WorkingBudgetVariance { get; set; }
        public decimal FixedBudgetVariance { get; set; }
        public decimal WhatIfBudgetVariance { get; set; }
        
    }    
     
}
