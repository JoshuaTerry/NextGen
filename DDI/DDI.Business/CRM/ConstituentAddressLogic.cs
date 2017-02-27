
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
using DDI.Business.Helpers;
using DDI.Business.Core;

namespace DDI.Business.CRM
{
    /// <summary>
    /// Constituent Address business logic
    /// </summary>
    public class ConstituentAddressLogic : EntityLogicBase<ConstituentAddress>
    {

        #region Constructors 

        public ConstituentAddressLogic() : this(new UnitOfWorkEF()) { }

        public ConstituentAddressLogic(IUnitOfWork uow) : base(uow)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determine if a constituent address is current based on the address's start and end dates.
        /// </summary>
        public bool IsCurrentAddress(ConstituentAddress address, DateTime date)
        {
            bool current = false;

            if (address.StartDay > 0 && address.EndDay > 0)
            {
                // Vacation address.  Determine the day number and compare to StartDay, EndDay.
                int day = date.DayOfYear;
                if ((day >= address.StartDay && day <= address.EndDay && address.EndDay >= address.StartDay) || // Summer address:  StartDay < EndDay
                    ((day >= address.StartDay || day <= address.EndDay) && address.EndDay < address.StartDay))  // Winter address:  StartDay > EndDay
                {
                    current = true;
                }
            }
            // Non-vacation address:  use StartDt, EndDt
            else if ((!address.StartDate.HasValue || address.StartDate.Value <= date) &&
                     (!address.EndDate.HasValue || address.EndDate.Value >= date))
            {
                current = true;
            }

            return current;
        }

        /// <summary>
        /// Get an address for a constituent.
        /// </summary>
        /// <param name="constituent">The constituent</param>
        /// <param name="addressCategory">Address category (Primary, Mailing, Location, Alternate, None)</param>
        public ConstituentAddress GetAddress(Constituent constituent, AddressCategory addressCategory)
        {
            return GetAddress(constituent, addressCategory, string.Empty, true, true, DateTime.Now, Guid.Empty);
        }

        /// <summary>
        /// Get an address for a constituent.
        /// </summary>
        /// <param name="constituent">The constituent</param>
        /// <param name="addressCategory">Address category (Primary, Mailing, Location, Alternate, None)</param>
        /// <param name="addressTypeCode">Address type code or comma delimited list of address type codes.  Blank for all address types.</param>
        /// <param name="allowVacation">TRUE to allow vacation addresses.</param>
        /// <param name="currentOnly">TRUE to return only current addresses.</param>
        /// <param name="baseDate">Date to use for determining current addresses.  If null, current date is used.</param>
        /// <param name="ignoreId">ID of an address to ignore.  If not null, this address will not be returned.</param>
        public ConstituentAddress GetAddress(Constituent constituent, AddressCategory addressCategory, string addressTypeCode, bool allowVacation, bool currentOnly, DateTime? baseDate = null, Guid? ignoreId = null)
        {
            ConstituentAddress resultAddress = null;
            List<WeightedAddress> weightList = new List<WeightedAddress>();
            List<string> typeCodes = new List<string>();

            if (baseDate == null)
            {
                baseDate = DateTime.Now;
            }

            if (addressCategory == AddressCategory.Mailing)
            {
                // Mailing mode uses mailing address types
                var configuration = UnitOfWork.GetBusinessLogic<ConfigurationLogic>().GetConfiguration<CRMConfiguration>();
                typeCodes.AddRange(configuration.MailAddressTypes.Select(p => p.Code));
                if (typeCodes.Count == 0)
                {
                    typeCodes.AddRange(new string[] { AddressTypeCodes.Home, AddressTypeCodes.Mailing });
                }
            }
            else if (addressCategory != AddressCategory.None)
            {
                // All other types except "None" use home address types
                var configuration = UnitOfWork.GetBusinessLogic<ConfigurationLogic>().GetConfiguration<CRMConfiguration>();
                typeCodes.AddRange(configuration.HomeAddressTypes.Select(p => p.Code));
                if (typeCodes.Count == 0)
                {
                    typeCodes.AddRange(new string[] { AddressTypeCodes.Home, AddressTypeCodes.Location, AddressTypeCodes.Work });
                }
            }

            // Add in the vacation address if specified
            if (allowVacation && !typeCodes.Contains(AddressTypeCodes.Vacation))
            {
                typeCodes.Insert(0, AddressTypeCodes.Vacation);
            }

            // If passing in a specific address type code (or list of codes) add them to the front.
            if (!string.IsNullOrWhiteSpace(addressTypeCode))
            {
                foreach (var code in addressTypeCode.Split(',').Reverse()) // Reverse ensures their order is retained.
                {
                    string trimmedCode = code.Trim();
                    if (trimmedCode.Length > 0 && !typeCodes.Contains(trimmedCode))
                    {
                        typeCodes.Insert(0, trimmedCode);
                    }
                }
            }

            // If no address types, return null
            if (typeCodes.Count == 0)
            {
                return null;
            }

            // Ensure constituent addresses are loaded.
            UnitOfWork.LoadReference(constituent, p => p.ConstituentAddresses);
            
            foreach (var thisAddress in constituent.ConstituentAddresses)
            {
                // Get the address type and make sure it's not null.
                AddressType thisType = UnitOfWork.GetReference(thisAddress, p => p.AddressType);
                if (thisType == null)
                {
                    continue;
                }

                // Filter the constituent address rows:
                // If not allowing vacation addresses, filter out any vacation addresses.
                if (thisAddress.AddressType.Code == AddressTypeCodes.Vacation && !allowVacation)
                {
                    continue;
                }

                // If wanting current addresses only, filter out those that aren't current.
                if (currentOnly && !IsCurrentAddress(thisAddress, baseDate.Value))
                {
                    continue;
                }
                                
                // Calculate "weight" based on the order of address types in "types" - higher to lower, 0 if not in list.
                int weight = typeCodes.IndexOf(thisAddress.AddressType.Code) + 1;
                if (weight > 0)
                {
                    weight = typeCodes.Count - weight + 1;
                }

                // Give non-temporary addresses precedence over temporary addresses of the same type
                weight = weight * 2 - (thisAddress.StartDay > 0 ? 1 : 0);

                // If looking for primary address, give these precedence over non-primary
                if (addressCategory == AddressCategory.Primary && thisAddress.IsPrimary)
                {
                    weight += 100;
                }

                weightList.Add(new WeightedAddress(weight, thisAddress));

            }

            // Now find the best matching address

            bool skipped = false;
            foreach (var entry in weightList.OrderByDescending(p => p.Weight))
            {
                // If looking for an alternate address, skip the first address;
                if (addressCategory == AddressCategory.Alternate && !skipped)
                {
                    skipped = true;
                    continue;
                }

                // Ignoring an address number?
                if (ignoreId != null && ignoreId == entry.ConstituentAddress.Id)
                    continue;

                // Found the best match, so exit.
                resultAddress = entry.ConstituentAddress;
                break;
            }

            if (resultAddress != null)
            {
                UnitOfWork.GetReference(resultAddress, p => p.Address);
            }

            return resultAddress;
        }

        public override void Validate(ConstituentAddress entity)
        {
            if (entity.IsPrimary)
            {
                var existingPrimaryAddress = UnitOfWork.GetRepository<ConstituentAddress>().Entities.FirstOrDefault(ca => ca.ConstituentId == entity.ConstituentId && ca.Id != entity.Id && ca.IsPrimary);
                if (existingPrimaryAddress != null)
                {
                    existingPrimaryAddress.IsPrimary = false;
                    UnitOfWork.GetRepository<ConstituentAddress>().Update(existingPrimaryAddress);
                }
            }

            var constituentLogic = UnitOfWork.GetBusinessLogic<ConstituentLogic>();
            constituentLogic.ScheduleUpdateSearchDocument(entity.ConstituentId);
        }

        #endregion

        #region Inner Classes

        /// <summary>
        /// Struct used to sort addresses in GetAddress method.
        /// </summary>
        private struct WeightedAddress
        {
            public int Weight;
            public ConstituentAddress ConstituentAddress;

            public WeightedAddress(int weight, ConstituentAddress constituentAddress)
            {
                Weight = weight;
                ConstituentAddress = constituentAddress;
            }
        }

        #endregion


    }
}
