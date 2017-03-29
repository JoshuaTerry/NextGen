using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Attributes.Models;
using DDI.Shared.Enums.GL;

namespace DDI.Shared.Models.Client.GL
{    
    public class AccountBalance : ICanTransmogrify
    {
        public Guid Id { get; set; }

        public int PeriodNumber { get; set; }

        public string DebitCredit { get; set; }
        
        public decimal TotalAmount { get; set; }
    }    
     
}
