using DDI.Shared.Models.Common;
using System.Collections.Generic;
using System.Linq;

namespace DDI.Services
{
    public class CountryService : ServiceBase<Country>
    {
        protected override List<Country> ModifySortOrder(List<Country> data)
        {
            var us = data.FirstOrDefault(c => c.CountryCode == DDI.Shared.Statics.CRM.AddressDefaults.DefaultCountryCode);
            if (us != null)
            {
                int index = data.IndexOf(us);
                data.RemoveAt(index);
                data.Insert(0, us);
            }
            return data;
        }
    }
}
