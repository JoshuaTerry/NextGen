using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    [Table("AccountBalanceByPeriod")]
    public class AccountBalance : ICanTransmogrify
    {
        [Key,Column(Order = 1)]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Id))]
        public Account Account { get; set; }

        [Key, Column(Order = 2)]
        public int PeriodNumber { get; set; }

        [Key, Column(Order = 3)]
        public string DebitCredit { get; set; }
        
        public decimal TotalAmount { get; set; }
    }    
     
}
