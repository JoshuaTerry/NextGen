using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Common;

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
