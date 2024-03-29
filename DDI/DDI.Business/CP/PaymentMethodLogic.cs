﻿using DDI.Data;
using DDI.Logger;
using DDI.Shared;
using DDI.Shared.Models.Client.CP;
using DDI.Shared.Statics.CP;
using System.Text.RegularExpressions;

namespace DDI.Business.CP
{
    public class PaymentMethodLogic : EntityLogicBase<PaymentMethod>
    {
        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(PaymentMethodLogic));
        public PaymentMethodLogic() : this(new UnitOfWorkEF()) { }

        public PaymentMethodLogic(IUnitOfWork uow) : base(uow)
        {
        }

        public override void Validate(PaymentMethod entity)
        {
            base.Validate(entity);

            if (entity.Category == Shared.Enums.CP.PaymentMethodCategory.EFT)
            {
                if (entity.EFTFormatId == null)
                {
                    throw new ValidationException(UserMessagesCP.EFTFormatRequired);
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
                throw new ValidationException(UserMessagesCP.EFTRoutingNumberDigits);
            }

            if (number == "000000000")
            {
                throw new ValidationException(UserMessagesCP.EFTRoutingNumberZero);
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
                throw new ValidationException(UserMessagesCP.EFTRoutingNumberNotValid);
            }
        }
    }
}
