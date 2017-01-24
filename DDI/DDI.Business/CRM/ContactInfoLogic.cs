
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using System;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Common;
using DDI.Shared.Helpers;
using DDI.Shared.Statics.CRM;
namespace DDI.Business.CRM
{
    public class ContactInfoLogic : EntityLogicBase<ContactInfo>
    {
        #region Constructors 

        public ContactInfoLogic() : this(new UnitOfWorkEF()) { }

        public ContactInfoLogic(IUnitOfWork uow) : base(uow)
        {
        }

        #endregion

        public override void Validate(ContactInfo entity)
        {
            base.Validate(entity);

            if (string.IsNullOrWhiteSpace(entity.Info))
            {
                throw new Exception("Contact information cannot be blank.");
            }

            if (entity.ContactTypeId.IsNullOrEmpty())
            {
                throw new Exception("Contact type is not specified.");
            }

        }

        public bool ValidatePhoneNumber(ContactInfo entity)
        {
            return true;
        }


        public bool ValidatePhoneNumber(ref string phone, Country country)
        {
            string origPhone;
            string format;
            int formatDigits = 0;

            if (string.IsNullOrWhiteSpace(phone))
            {
                return true;
            }

            // Determine if phone # is raw digits.
            bool isRaw = (phone.Any(p => !char.IsDigit(p)));

            origPhone = phone;

            // Remove leading spaces or + from phone number.  
            origPhone = origPhone.TrimStart('+', ' ');

            // Remove the leading default international dialing prefix
            if (origPhone.StartsWith(ContactInfoDefaults.DefaultInternationalPrefix))
            {
                origPhone = RemovePhoneDigits(origPhone, ContactInfoDefaults.DefaultInternationalPrefix.Length);
            }

            if (country != null)
            {
                // Remove the country specific international dialing prefix if it's at the beginning of the phone number.
                if (!string.IsNullOrWhiteSpace(country.InternationalPrefix) && origPhone.StartsWith(country.InternationalPrefix))
                {
                    origPhone = RemovePhoneDigits(origPhone, country.InternationalPrefix.Length);
                }

                // Remove the calling code if it's at the beginning of the phone number.
                if (!string.IsNullOrWhiteSpace(country.CallingCode) && origPhone.StartsWith(country.CallingCode))
                {
                    origPhone = RemovePhoneDigits(origPhone, country.CallingCode.Length);
                }

                // Remove the trunk
                if (!string.IsNullOrWhiteSpace(country.TrunkPrefix) && origPhone.StartsWith(country.TrunkPrefix))
                {
                    origPhone = RemovePhoneDigits(origPhone, country.TrunkPrefix.Length);
                }

                format = country.PhoneFormat ?? string.Empty;

                // Count # of X characters in format
                formatDigits = format.Count(p => p == 'X');
            }
            else
            {
                format = string.Empty;
            }

            // Extract digits from origPhone
            StringBuilder sb = new StringBuilder();
            int digits = 0;
            foreach (char c in origPhone)
            {
                if (formatDigits > 0 && digits >= formatDigits)
                    sb.Append(c);
                else if (char.IsDigit(c))
                {
                    sb.Append(c);
                    digits++;
                }
            }

            // If there were enough digits, store the raw data.
            if (formatDigits > 0 && formatDigits <= digits)
            {
                phone = sb.ToString();
            }
            else
            {
                phone = origPhone;
            }

            return (digits >= formatDigits);
        }

        /// <summary>
        /// Remove specified # of digits from the front of a phone number, including any non-digits after that.
        /// </summary>
        private string RemovePhoneDigits(string text, int digits)
        {
            if (text.Length <= digits)
            {
                return string.Empty;
            }

            for (int pos = digits; pos < text.Length; pos++)
            {
                // Skipping non-digits past the first (digits) characters.
                if (char.IsDigit(text[pos]))
                {
                    return text.Substring(pos);
                }
            }
            return string.Empty;
        }

        private Country GetCountryForContactInfo(ContactInfo entity)
        {
            Constituent constituent = UnitOfWork.GetReference(entity, p => p.Constituent);
            if (constituent != null)
            {
            }

            return UnitOfWork.FirstOrDefault<Country>(p => p.ISOCode == "US");
        }
    }
}
