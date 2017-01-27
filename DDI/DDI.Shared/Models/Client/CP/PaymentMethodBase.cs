﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DDI.Shared.Enums.CP;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Shared.Models.Client.CP
{
    [Table("PaymentMethod")]
    public class PaymentMethodBase : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [MaxLength(128)]
        public string Description { get; set; }

        public PaymentMethodStatus Status { get; set; }

        public DateTime? StatusDate { get; set; }

        public ICollection<Constituent> Constituents { get; set; }

        public override string DisplayName => Description;
    }
}