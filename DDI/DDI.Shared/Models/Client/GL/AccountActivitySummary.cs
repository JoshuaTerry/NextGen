using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Attributes.Models;
using DDI.Shared.Enums.GL;

namespace DDI.Shared.Models.Client.GL
{
   
    public class AccountActivitySummary : ICanTransmogrify
    {
        public Guid Id { get; set; }

        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string FiscalYearName { get; set; }

        public string PriorYearName { get; set; }
        public string WorkingBudgetName { get; set; }
        public string FixedBudgetName { get; set; }
        public string WhatIfBudgetName { get; set; }

        public decimal ActivityTotal { get; set; }
        public decimal FinalEndingBalance { get; set; }

        public IList<AccountActivityDetail> Detail { get; set; }

    }    
     
}
