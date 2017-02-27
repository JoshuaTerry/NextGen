
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

        #region Public Methods

        /// <summary>
        /// Validation logic
        /// </summary>
        public override void Validate(ContactInfo contactInfo)
        {
            base.Validate(contactInfo);

            if (string.IsNullOrWhiteSpace(contactInfo.Info))
            {
                throw new ValidationException(UserMessagesCRM.ContactInfoBlank);
            }

            if (contactInfo.ContactTypeId.IsNullOrEmpty())
            {
                throw new ValidationException(UserMessagesCRM.ContactTypeMissing);
            }

            if (contactInfo.ConstituentId.IsNullOrEmpty() && contactInfo.ParentContactId.IsNullOrEmpty())
            {
                throw new ValidationException(UserMessagesCRM.ContactInfoNoParent);
            }

            // Get the category code.
            string categoryCode = GetContactCategoryCode(contactInfo);

            // If phone, format the phone number.
            if (categoryCode == ContactCategoryCodes.Phone)
            {
                ValidatePhoneNumber(contactInfo);                
            }

            // check for preferred contactinfos, one per category
            ValidateIsPreferred(contactInfo, categoryCode);
            
        }

        /// <summary>
        /// Validate a phone number, removing formatting characters.
        /// </summary>
        /// <param name="contactInfo">ContactInfo entity containing the phone number to be validated.</param>
        public void ValidatePhoneNumber(ContactInfo contactInfo)
        {
            if (contactInfo == null)
            {
                throw new ArgumentNullException(nameof(contactInfo));
            }

            Country country = GetCountryForContactInfo(contactInfo);
            string phone = contactInfo.Info;

            phone = SimplifyPhoneNumberForCountry(phone, country);
            if (phone == null)
            {
                throw new ValidationException(UserMessagesCRM.PhoneFormatNotValid);
            }

            contactInfo.Info = phone;            
        }


        /// <summary>
        /// Simplify a phone number for a country, removing formatting characters and country-specific prefixes.
        /// </summary>
        /// <param name="phone">Phone number.</param>
        /// <param name="country">Country</param>
        /// <returns>Null if phone number format is not valid for specified country.</returns>
        public string SimplifyPhoneNumberForCountry(string phone, Country country)
        {
            string rawPhone; // Phone # minus extra stuff.
            string format;
            int formatDigits = 0;

            if (string.IsNullOrWhiteSpace(phone))
            {
                return string.Empty;
            }

            rawPhone = phone;

            // Remove leading spaces or + from phone number.  
            rawPhone = rawPhone.TrimStart(ContactInfoDefaults.CallingCodeIndicator, ' ');

            // Remove the leading default international dialing prefix
            if (rawPhone.StartsWith(ContactInfoDefaults.DefaultInternationalPrefix))
            {
                rawPhone = RemovePhoneDigits(rawPhone, ContactInfoDefaults.DefaultInternationalPrefix.Length);
            }

            if (country != null)
            {
                // Remove the country specific international dialing prefix if it's at the beginning of the phone number.
                if (!string.IsNullOrWhiteSpace(country.InternationalPrefix) && rawPhone.StartsWith(country.InternationalPrefix))
                {
                    rawPhone = RemovePhoneDigits(rawPhone, country.InternationalPrefix.Length);
                }

                // Remove the calling code if it's at the beginning of the phone number.
                if (!string.IsNullOrWhiteSpace(country.CallingCode) && rawPhone.StartsWith(country.CallingCode))
                {
                    rawPhone = RemovePhoneDigits(rawPhone, country.CallingCode.Length);
                }

                // Remove the trunk
                if (!string.IsNullOrWhiteSpace(country.TrunkPrefix) && rawPhone.StartsWith(country.TrunkPrefix))
                {
                    rawPhone = RemovePhoneDigits(rawPhone, country.TrunkPrefix.Length);
                }

                format = country.PhoneFormat ?? string.Empty;

                // Count # of X characters in format
                formatDigits = format.Count(p => p == 'X');
            }
            else
            {
                format = string.Empty;
            }

            // Extract digits from rawPhone
            StringBuilder sb = new StringBuilder();
            int digits = 0;
            foreach (char c in rawPhone)
            {
                if (formatDigits > 0 && digits >= formatDigits)
                {
                    sb.Append(c);
                }
                else if (char.IsDigit(c))
                {
                    sb.Append(c);
                    digits++;
                }
            }

            // If there were enough digits, store the raw data.
            if (formatDigits > 0 && formatDigits <= digits)
            {
                rawPhone = sb.ToString();
            }

            // If there were not enough digits, return null.
            else if (formatDigits > digits)
            {
                rawPhone = null;
            }

            return rawPhone;
        }

        /// <summary>
        /// Get the contact category code for a ContactInfo entity.
        /// </summary>
        public string GetContactCategoryCode(ContactInfo contactInfo)
        {
            if (contactInfo == null)
            {
                return string.Empty;
            }

            return GetContactCategoryCode(contactInfo.ContactType ?? UnitOfWork.GetReference(contactInfo, p => p.ContactType));
        }

        /// <summary>
        /// Get the contact category code for a ContactType entity.
        /// </summary>
        public string GetContactCategoryCode(ContactType contactType)
        {
            if (contactType == null)
            {
                return string.Empty;
            }

            ContactCategory category = contactType.ContactCategory ?? UnitOfWork.GetReference(contactType, p => p.ContactCategory);
            return category?.Code ?? string.Empty;
        }

        /// <summary>
        /// Format contact inforation based on the contact information's category (phone, email, etc.)
        /// </summary>
        /// <returns></returns>
        public string FormatContactInformation(ContactInfo contactInfo)
        {
            if (contactInfo == null || string.IsNullOrWhiteSpace(contactInfo.Info))
            {
                return string.Empty;
            }

            // Get the category code.
            string categoryCode = GetContactCategoryCode(contactInfo);

            // If phone, format the phone number.
            if (categoryCode == ContactCategoryCodes.Phone)
            {
                return FormatPhoneNumber(contactInfo);
            }

            return contactInfo.Info;
        }

        /// <summary>
        /// Format a phone number.
        /// </summary>
        /// <param name="contactInfo">ConactInfo entity containing the phone number.</param>
        /// <param name="includeInternationalPrefix">True to include international dialing prefix</param>
        /// <param name="includeNANPPrefix">True to enforce NANP formatting for NANP countries.</param>
        /// <param name="localFormat">True if caller is dialing from their own country.</param>
        public string FormatPhoneNumber(ContactInfo contactInfo, bool includeInternationalPrefix = false, bool includeNANPPrefix = false, bool localFormat = false)
        {
            if (contactInfo == null || string.IsNullOrWhiteSpace(contactInfo.Info))
            {
                return string.Empty;
            }

            Country country = GetCountryForContactInfo(contactInfo);

            return FormatPhoneNumber(contactInfo.Info, country, includeInternationalPrefix, includeNANPPrefix, localFormat);
        }

        /// <summary>
        /// Format an unformatted phone number.
        /// </summary>
        /// <param name="phone">Unformatted phone number.</param>
        /// <param name="country">Country for this phone number.</param>
        /// <param name="includeInternationalPrefix">True to include international dialing prefix.</param>
        /// <param name="includeNANPPrefix">True enforce NANP formatting for NANP countries.</param>
        /// <param name="localFormat">True if caller is dialing from their own country.</param>
        public string FormatPhoneNumber(string phone, Country country = null, bool includeInternationalPrefix = false, bool includeNANPPrefix = false, bool localFormat = false)
        {
            // Note:  Formatting logic supports calling within a country, or calling from USA to some other country.  
            //        It does not currently support calling from a non-US country to a different country.

            if (string.IsNullOrWhiteSpace(phone))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            if (country == null)
            {
                country = UnitOfWork.GetBusinessLogic<AddressLogic>().GetDefaultCountry();
            }

            if (country != null)
            {
                string phoneFormat = country.PhoneFormat;

                if (!localFormat) // Calling from USA...
                {                    
                    if (country.CallingCode == ContactInfoDefaults.NANPCallingCode)
                    {
                        // NANP country: USA, Canada, etc.
                        if (includeNANPPrefix)
                        {
                            sb.Append(ContactInfoDefaults.NANPTrunkPrefix);
                            phoneFormat = ContactInfoDefaults.NANPFormat;
                        }
                    }
                    else
                    {
                        // Other non-NANP country
                        if (includeInternationalPrefix)
                        {
                            sb.Append(ContactInfoDefaults.DefaultInternationalPrefix);
                            sb.Append(' ');
                        }
                        if (!string.IsNullOrWhiteSpace(country.CallingCode))
                        {
                            if (!includeInternationalPrefix)
                            {
                                sb.Append(ContactInfoDefaults.CallingCodeIndicator);
                            }
                            sb.Append(country.CallingCode);
                            sb.Append(' ');
                        }
                    }
                }
                else // Calling from specified country...
                {                    
                    if (country.CallingCode == ContactInfoDefaults.NANPCallingCode)
                    {
                        // USA, Canada, etc.
                        if (includeNANPPrefix)
                        {
                            sb.Append(ContactInfoDefaults.NANPTrunkPrefix);
                            phoneFormat = ContactInfoDefaults.NANPFormat;
                        }
                    }

                    // Include the trunk if there is one.
                    else
                    {
                        // Non-NANP country
                        if (!string.IsNullOrWhiteSpace(country.TrunkPrefix))
                        {
                            sb.Append(country.TrunkPrefix);
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(phoneFormat))
                {
                    sb.Append(phone);
                }
                else
                {
                    string rawPhone = StringHelper.LettersAndDigits(phone);
                    if (rawPhone.Length < phoneFormat.Count(c => c == 'X'))
                    {
                        // Phone doesn't have enough digits, so just append it.
                        sb.Append(phone);
                    }
                    else
                    {
                        int position = 0;
                        foreach (char c in phoneFormat)
                        {
                            if (c == 'X' && position < phone.Length)
                            {
                                sb.Append(phone[position++]);
                            }
                            else
                            {
                                sb.Append(c);
                            }
                        }

                        // Tack on any remaining characters in the raw phone
                        if (position < phone.Length)
                        {
                            sb.Append(' ').Append(phone.Substring(position));
                        }
                    }

                }
            }
            else
            {
                // No country
                sb.Append(phone);
            }

            return sb.ToString();
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Remove specified # of digits from the front of a phone number, including any non-digits after that.
        /// </summary>
        private string RemovePhoneDigits(string text, int digits)
        {
            if (text.Length <= digits)
            {
                return string.Empty;
            }

            for (int index = digits; index < text.Length; index++)
            {
                // Skipping non-digits past the first (digits) characters.
                if (char.IsDigit(text[index]))
                {
                    return text.Substring(index);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// For a ContactInfo, get the consitituent and grab the country from the constituent's primary address.
        /// </summary>
        private Country GetCountryForContactInfo(ContactInfo contactInfo)
        {
            Country resultCountry = null;

            var addressLogic = UnitOfWork.GetBusinessLogic<AddressLogic>();

            Constituent constituent = contactInfo.Constituent ?? UnitOfWork.GetReference(contactInfo, p => p.Constituent);
            if (constituent != null)
            {
                // Get primary address.
                var constituentAddressLogic = UnitOfWork.GetBusinessLogic<ConstituentAddressLogic>();
                Address address = constituentAddressLogic.GetAddress(constituent, AddressCategory.Primary)?.Address;

                if (address != null)
                {
                    // Ensure country is loaded.
                    addressLogic.LoadAllProperties(address);
                    resultCountry = address.Country;
                }                
            }

            return resultCountry ?? addressLogic.GetDefaultCountry();
        }

        private void ValidateIsPreferred(ContactInfo contactInfo, string categoryCode)
        {
            if (contactInfo.IsPreferred)
            {
                var existingPreferredContactInfo = UnitOfWork.GetRepository<ContactInfo>().Entities.FirstOrDefault(ci => ci.ConstituentId == contactInfo.ConstituentId && categoryCode == ci.ContactType.ContactCategory.Code && ci.IsPreferred == true); //&& ci.Id != contactInfo.Id && ci.IsPreferred && categoryCode == contactInfo.ContactType.ContactCategory.Code
                // changed from contactcategorycode != other...wascategorycode == other. may need to change it back, check from  test
                // see how it behaves, it may return ispreferred for all other codes
                // Also, it looks like contactinfo will have a contact category that I can grab the code from...
                if (existingPreferredContactInfo != null)
                {
                    existingPreferredContactInfo.IsPreferred = false;
                    UnitOfWork.GetRepository<ContactInfo>().Update(existingPreferredContactInfo);
                }
            }
        }

        #endregion

    }
}
