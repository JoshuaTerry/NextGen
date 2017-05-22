using DDI.Shared.Models.Common;
using System.Collections.Generic;
using System.Linq;
using DDI.Shared.Statics;
using DDI.Shared;

namespace DDI.Services
{
    public class CountryService : ServiceBase<Country>
    {
        public CountryService(IUnitOfWork uow) : base(uow) { }

        /// <summary>
        /// Ensures the default country code (US) is listed first in the sorting order.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override List<Country> ModifySortOrder(string orderBy, List<Country> data)
        {
            if (orderBy == OrderByProperties.DisplayName)
            {
                var us = data.FirstOrDefault(c => c.CountryCode == DDI.Shared.Statics.CRM.AddressDefaults.DefaultCountryCode);
                if (us != null)
                {
                    int index = data.IndexOf(us);
                    data.RemoveAt(index);
                    data.Insert(0, us);
                }
            }

            return data;
        }
    }
}
