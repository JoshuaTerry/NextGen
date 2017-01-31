﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.CP;

namespace DDI.Business.CP
{
    public class PaymentMethodLogic : EntityLogicBase<PaymentMethodBase>
    {
        public PaymentMethodLogic() : this(new UnitOfWorkEF()) { }

        public PaymentMethodLogic(IUnitOfWork uow) : base(uow)
        {
        }

        public override void Validate(PaymentMethodBase entity)
        {
            base.Validate(entity);
            if (entity is EFTPaymentMethod)
            {
                EFTPaymentMethod eftEntity = entity as EFTPaymentMethod;
                ValidateRoutingNumber(eftEntity.RoutingNumber);

                if (eftEntity.EFTFormat == null)
                {
                    throw new Exception("EFT Format is required.");
                }
            }
        }

        /// <summary>
        /// Validate a routing number.  Throws validation exceptions if routing number does not meet ABA requirements.
        /// </summary>
        public void ValidateRoutingNumber(string number)
        {
            // Allow blank values
            if (string.IsNullOrWhiteSpace(number))
            {
                return;
            }

            if (!Regex.IsMatch(number, @"^\d{9}$"))
            {
                throw new Exception("Routing number must have nine digits.");
            }

            if (number == "000000000")
            {
                throw new Exception("Routing number cannot be all zeros.");
            }

            int checksum = (int)(char.GetNumericValue(number[0]) * 3 +
                                 char.GetNumericValue(number[1]) * 7 +
                                 char.GetNumericValue(number[2]) +
                                 char.GetNumericValue(number[3]) * 3 +
                                 char.GetNumericValue(number[4]) * 7 +
                                 char.GetNumericValue(number[5]) +
                                 char.GetNumericValue(number[6]) * 3 +
                                 char.GetNumericValue(number[7]) * 7 +
                                 char.GetNumericValue(number[8]));

            if (checksum % 10 != 0)
            {
                throw new Exception("Invalid routing number (based on check digit.)");
            }
        }
    }
}
