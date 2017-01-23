using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DDI.Shared.Enums.CP;

namespace DDI.Shared.Models.Client.CP
{
    public class EFTPaymentMethod : PaymentMethodBase
    {
        [MaxLength(128)]
        public string BankName { get; set; }

        [MaxLength(64)]
        public string BankAccount { get; set; }

        [MaxLength(64)]
        public string RoutingNumber { get; set; }

        public EFTAccountType TranCode { get; set; }

        public Guid? EFTFormatId { get; set; }

        public EFTFormat EFTFormat { get; set; }

    }
}
