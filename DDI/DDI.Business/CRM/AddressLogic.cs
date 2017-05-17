
using DDI.Business.Helpers;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDI.Business.CRM
{
    /// <summary>
    /// Address business logic
    /// </summary>
    public class AddressLogic : EntityLogicBase<Address>
    {

        #region Constructors 

        public AddressLogic() : this(new UnitOfWorkEF()) { }

        public AddressLogic(IUnitOfWork uow) : base(uow)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Cached queryable collection of countries
        /// </summary>
        public IQueryable<Country> Countries
        {
            get
            {
                return UnitOfWork.GetCachedRepository<Country>().Entities;
            }
        }

        /// <summary>
        /// Cached queryable collection of states
        /// </summary>
        public IQueryable<State> States
        {
            get
            {
                return UnitOfWork.GetCachedRepository<State>().Entities;
            }
        }

        /// <summary>
        /// Cached queryable collection of counties
        /// </summary>
        public IQueryable<County> Counties
        {
            get
            {
                return UnitOfWork.GetCachedRepository<County>().Entities;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Load the Country, State, and County properties for an address.
        /// </summary>
        /// <param name="address"></param>
        public void LoadAllProperties(Address address, bool reload = false)
        {
            if (address.CountryId != null)
            {
                if (reload || address.Country == null)
                {
                    address.Country = Countries.FirstOrDefault(p => p.Id == address.CountryId);
                }
            }
            else
            {
                address.Country = null;
            }

            if (address.StateId != null)
            {
                if (reload || address.State == null)
                {
                    address.State = States.FirstOrDefault(p => p.Id == address.StateId);
                }
            }
            else
            {
                address.State = null;
            }

            if (address.CountyId != null)
            {
                if (reload || address.County == null)
                {
                    address.County = Counties.FirstOrDefault(p => p.Id == address.CountyId);
                }
            }
            else
            {
                address.County = null;
            }

        }

        /// <summary>
        /// Determine if a country is non-US
        /// </summary>
        public bool IsForeignCountry(Country country)
        {
            return country.ISOCode != AddressDefaults.DefaultCountryCode;
        }

        /// <summary>
        /// Get the default country.
        /// </summary>
        public Country GetDefaultCountry()
        {
            return Countries.FirstOrDefault(p => p.ISOCode == AddressDefaults.DefaultCountryCode);
        }

        /// <summary>
        /// Format an address.  Multiple lines are separated by newline characters.
        /// </summary>
        /// <param name="address">The address to format</param>
        /// <param name="caps">TRUE to abbreviate text and convert to uppercase</param>
        /// <param name="expand">TRUE to expand all abbreviations</param>
        /// <param name="maxLength">Maximum line length (0 for no maximum)</param>
        /// <returns></returns>
        public string FormatAddress(Address address, bool caps = false, bool expand = false, int maxLength = 0)
        {
            StringBuilder sb = new StringBuilder();
            string text;
            string[] AddressLines = new string[] { address.AddressLine1, address.AddressLine2 };

            LoadAllProperties(address);

            for (int i = 0; i < AddressLines.Count(); i++)
            {
                if (!string.IsNullOrWhiteSpace(AddressLines[i]))
                {
                    text = AddressLines[i];

                    if (expand)
                    {
                        text = AbbreviationHelper.ExpandAddressLine(text, UnitOfWork);
                        if (caps)
                        {
                            text = text.ToUpper();
                        }
                    }
                    else if (maxLength > 0)
                    {
                        text = AbbreviationHelper.AbbreviateAddressLine(text, maxLength, caps, UnitOfWork);
                    }
                    else if (caps)
                    {
                        text = AbbreviationHelper.AbbreviateAddressLine(text, true, false, UnitOfWork);
                    }

                    sb.Append(text).Append('\n');
                }
            }

            text = FormatCityStatePostalCode(address.City, address.State?.StateCode, address.PostalCode, address.Country);
            if (caps)
            {
                sb.Append(text.ToUpper());
            }
            else
            {
                sb.Append(text);
            }

            return sb.ToString().Trim('\n');
        }

        /// <summary>
        /// Format a city, state, postal code, and country.  Multiple lines are separated by newline characters.
        /// </summary>
        public string FormatCityStatePostalCode(Address address)
        {
            LoadAllProperties(address);   
            return FormatCityStatePostalCode(address.City, address.State?.StateCode, address.PostalCode, address.Country);
        }

        /// <summary>
        /// Format a city, state, postal code, and country.  Multiple lines are separated by newline characters.
        /// </summary>
        public string FormatCityStatePostalCode(string city, string stateCode, string postalCode, Country country)
        {
            string rslt;
            string format;

            if (stateCode == null)
            {
                stateCode = string.Empty;
            }

            if (postalCode == null)
            {
                postalCode = string.Empty;
            }

            if (country == null)
            {
                return string.Format("{0} {1} {2}", city, stateCode, postalCode);
            }

            if (string.IsNullOrWhiteSpace(country.AddressFormat))
            {
                format = $"{AddressFormatMacros.City}, {AddressFormatMacros.StateCode} {AddressFormatMacros.PostalCode}";
            }
            else
            {
                format = country.AddressFormat.ToUpper();
                // If non-US and there's no $COUNTRY in the format, append it.
                if (IsForeignCountry(country) && format.IndexOf(AddressFormatMacros.Country) < 0)
                {
                    format = format + AddressFormatMacros.Newline + AddressFormatMacros.Country;
                }            
            }

            rslt = format;

            if (rslt.IndexOf(AddressFormatMacros.State) >= 0)
            {
                State st = country.States.FirstOrDefault(p => string.Compare(p.StateCode, stateCode, true) == 0);
                string stateName = (st != null ? st.Description : string.Empty);
                rslt = rslt.Replace(AddressFormatMacros.State, stateName);
            }

            if (rslt.IndexOf(AddressFormatMacros.PostalCode) >= 0)
            {
                rslt = rslt.Replace(AddressFormatMacros.PostalCode, FormatPostalCode(postalCode, country));
            }


            // Macros
            rslt = rslt.Replace(AddressFormatMacros.Country, country.Description.ToUpper())
                         .Replace(AddressFormatMacros.ISOCode, country.ISOCode)
                         .Replace(AddressFormatMacros.City, city)
                         .Replace(AddressFormatMacros.StateCode, stateCode.ToUpper());

            return rslt.Replace(AddressFormatMacros.Newline, "\n");
        }

        /// <summary>
        /// Format a postal code based on country-specific formatting.
        /// </summary>
        public string FormatPostalCode(string postalCode, Country country)
        {

            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(postalCode))
            {
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(country.PostalCodeFormat))
            {
                return postalCode;
            }

            // Convert postal code to raw format
            string rawcode = StringHelper.LettersAndDigits(postalCode);

            // Find a suitable format whose raw length matches the raw postal code's length
            //string format = country.PostalCodeFormat.Split(',').FirstOrDefault(p => StringHelper.LettersAndDigits(p).Length == rawcode.Length);
            string format = null;
            foreach (var entry in country.PostalCodeFormat.Split(','))
            {
                string rawEntry = StringHelper.LettersAndDigits(entry);

                // Special case for BP - remove it from the raw format and raw code.
                if (rawEntry.IndexOf(AddressDefaults.PostalBoxSpecifier) >= 0)
                {
                    rawEntry = rawEntry.Replace(AddressDefaults.PostalBoxSpecifier, string.Empty);
                    rawcode = rawcode.Replace(AddressDefaults.PostalBoxSpecifier, string.Empty);
                }
                if (rawEntry.Length == rawcode.Length)
                {
                    format = entry;
                    break;
                }
            }

            if (format == null)
            {
                return postalCode;
            }

            // Format the postal code
            int position = 0;
            foreach (char c in format)
            {
                if (c == AddressFormatMacros.AlphanumericSpecifier || 
                    c == AddressFormatMacros.NumericSpecifier || 
                    c == AddressFormatMacros.AlphaSpecifier)
                {
                    if (position < rawcode.Length)
                    {
                        sb.Append(rawcode[position++]);
                    }
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }


        /// <summary>
        ///  Validate a postal code, based on country-specific formatting.
        /// </summary>
        /// <param name="postalCode">Postal code to validate</param>
        /// <param name="countryId">ID of country</param>
        /// <param name="rawPostalCode">Postal code with all formatting characters removed.</param>
        /// <param name="formattedPostalCode">Fully formatted postal code</param>
        /// <returns>TRUE if postal code formatting is valid.</returns>
        public bool ValidatePostalCode(string postalCode, Guid? countryId, out string rawPostalCode, out string formattedPostalCode)
        {
            Country country = null;
            if (countryId.HasValue)
            {                
                country = Countries.FirstOrDefault(p => p.Id == countryId.Value);
            }
            return ValidatePostalCode(postalCode, country, out rawPostalCode, out formattedPostalCode);
        }

        /// <summary>
        ///  Validate a postal code, based on country-specific formatting.
        /// </summary>
        /// <param name="postalCode">Postal code to validate</param>
        /// <param name="country">Country</param>
        /// <param name="rawPostalCode">Postal code with all formatting characters removed.</param>
        /// <param name="formattedPostalCode">Fully formatted postal code</param>
        /// <returns>TRUE if postal code formatting is valid.</returns>
        public bool ValidatePostalCode(string postalCode, Country country, out string rawPostalCode, out string formattedPostalCode)
        {
            bool isValid = true;
            rawPostalCode = string.Empty;
            formattedPostalCode = string.Empty;

            if (string.IsNullOrWhiteSpace(postalCode) || country == null)
            {
                rawPostalCode = postalCode;
                formattedPostalCode = postalCode;
                return isValid;
            }

            string rawCode = StringHelper.LettersAndDigits(postalCode);
            rawPostalCode = rawCode;
            formattedPostalCode = postalCode;

            if (string.IsNullOrWhiteSpace(country.PostalCodeFormat))
            {
                return isValid;
            }

            // Find a suitable format whose raw length matches the raw postal code's length
            string format = null;
            foreach (var entry in country.PostalCodeFormat.Split(','))
            {
                string rawEntry = StringHelper.LettersAndDigits(entry);

                // Special case for BP - remove it from the raw format and raw code.
                if (rawEntry.IndexOf(AddressDefaults.PostalBoxSpecifier) >= 0)
                {
                    rawEntry = rawEntry.Replace(AddressDefaults.PostalBoxSpecifier, string.Empty);
                    rawCode = rawCode.Replace(AddressDefaults.PostalBoxSpecifier, string.Empty);
                    rawPostalCode = rawCode;
                }
                if (rawEntry.Length == rawCode.Length)
                {
                    format = entry;
                    break;
                }
            }

            if (format == null)
            {
                isValid = false;
            }
            else
            {
                // Format and validate the postal code
                StringBuilder sb = new StringBuilder();
                int position = 0;
                foreach (char c in format)
                {
                    if (c == 'A') // requires a letter
                    {
                        if (position < rawCode.Length)
                        {
                            isValid &= char.IsLetter(rawCode[position]);
                            sb.Append(rawCode[position++]);
                        }
                        else
                            isValid = false;
                    }
                    else if (c == '9') // requires a digit
                    {
                        if (position < rawCode.Length)
                        {
                            isValid &= char.IsDigit(rawCode[position]);
                            sb.Append(rawCode[position++]);
                        }
                        else
                            isValid = false;
                    }
                    else if (c == 'X') // requires any character
                    {
                        if (position < rawCode.Length)
                            sb.Append(rawCode[position++]);
                    }
                    else
                        sb.Append(c);
                }

                if (isValid)
                {
                    formattedPostalCode = sb.ToString();
                }
            }
            return isValid;
        }



        public override void Validate(Address address)
        {
            base.Validate(address);

            // Process logic for each constituent linked to this address.
            var constituentLogic = UnitOfWork.GetBusinessLogic<ConstituentLogic>();
            var constituentIdsVisited = new List<Guid>();
            foreach (var constituentAddress in UnitOfWork.GetReference(address, p => p.ConstituentAddresses).Where(p => p.ConstituentId != null))
            {
                if (!constituentIdsVisited.Contains(constituentAddress.ConstituentId.Value))
                {
                    // Update the constituent search document
                    constituentLogic.ScheduleUpdateSearchDocument(constituentAddress.ConstituentId);

                    // Ensure this constituent is processed only once.
                    constituentIdsVisited.Add(constituentAddress.ConstituentId.Value);
                }
            }
        }

        #endregion

        #region Inner Classes

        #endregion

    }
}
