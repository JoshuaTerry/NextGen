﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    [Table("AccountClose")]
    public class AccountClose : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid? AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }

        public PeriodAmountList Debit { get; set; }

        public PeriodAmountList Credit { get; set; }   

    }
}
