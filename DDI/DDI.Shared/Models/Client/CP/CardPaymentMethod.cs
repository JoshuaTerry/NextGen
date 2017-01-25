using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Shared.Models.Client.CP
{
    public class CardPaymentMethod : PaymentMethodBase
    {
        [MaxLength(128)]
        public string CardToken { get; set; }
    }
}
