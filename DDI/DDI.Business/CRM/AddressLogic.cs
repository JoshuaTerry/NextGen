
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
    /// <summary>
    /// Address business logic
    /// </summary>
    public class AddressLogic : EntityLogicBase<Address>
    {
        #region Private Fields

        private const string BP = "BP";  // Special text that appears in some postal code formats.  (No idea what it means...)

        #endregion

        #region Constructors 

        public AddressLogic() : this(new UnitOfWorkEF()) { }

        public AddressLogic(IUnitOfWork uow) : base(uow)
        {
        }

        #endregion

        #region Public Properties

        public string DefaultCountryCode => "US";

        public IQueryable<Country> Countries
        {
            get
            {
                return UnitOfWork.GetCachedRepository<Country>().Entities;
            }
        }

        public IQueryable<State> States
        {
            get
            {
                return UnitOfWork.GetCachedRepository<State>().Entities;
            }
        }

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
        public void LoadAllProperties(Address address)
        {
            if (address.CountryId != null)
            {               
                address.Country = Countries.FirstOrDefault(p => p.Id == address.CountryId);                
            }
            else
            {
                address.Country = null;
            }

            if (address.StateId != null)
            {
                address.State = States.FirstOrDefault(p => p.Id == address.StateId);
            }
            else
            {
                address.State = null;
            }

            if (address.CountyId != null)
            {
                address.County = Counties.FirstOrDefault(p => p.Id == address.CountyId);
            }
            else
            {
                address.County = null;
            }

        }

        public bool IsForeignCountry(Country country)
        {
            return country.ISOCode != DefaultCountryCode;
        }

        public string FormatCityStatePostalCode(string city, string stateCode, string postalCode, Country country)
        {
            string rslt;
            string format;

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
                if (rawEntry.IndexOf(BP) >= 0)
                {
                    rawEntry = rawEntry.Replace(BP, string.Empty);
                    rawcode = rawcode.Replace(BP, string.Empty);
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


        public bool ValidatePostalCode(string postalCode, Guid? countryId, out string rawPostalCode, out string formattedPostalCode)
        {
            Country country = null;
            if (countryId.HasValue)
            {                
                country = Countries.FirstOrDefault(p => p.Id == countryId.Value);
            }
            return ValidatePostalCode(postalCode, country, out rawPostalCode, out formattedPostalCode);
        }
        
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
                if (rawEntry.IndexOf(BP) >= 0)
                {
                    rawEntry = rawEntry.Replace(BP, string.Empty);
                    rawCode = rawCode.Replace(BP, string.Empty);
                    rawPostalCode = rawCode;
                }
                if (rawEntry.Length == rawCode.Length)
                {
                    format = entry;
                    break;
                }
            }

            if (format == null)
                isValid = false;
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
        }

        #endregion

        #region Inner Classes

        #endregion

    }
}
