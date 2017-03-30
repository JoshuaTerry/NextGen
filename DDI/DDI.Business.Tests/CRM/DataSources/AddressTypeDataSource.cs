using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Business.Tests.CRM.DataSources
{
    public static class AddressTypeDataSource
    {
        public static IList<AddressType> GetDataSource(UnitOfWorkNoDb uow)
        {
            IList<AddressType> existing = uow.GetRepositoryOrNull<AddressType>()?.Entities.ToList();
            if (existing != null)
            {
                return existing;
            }      
               
            var list = new List<AddressType>();
            list.Add(new AddressType() { Code = "H", Name = "Home address", IsActive = true, Id = GuidHelper.NewSequentialGuid() });
            list.Add(new AddressType() { Code = "L", Name = "Location address", IsActive = true, Id = GuidHelper.NewSequentialGuid() });
            list.Add(new AddressType() { Code = "M", Name = "Mailing address", IsActive = true, Id = GuidHelper.NewSequentialGuid() });
            list.Add(new AddressType() { Code = "V", Name = "Vacation address", IsActive = true, Id = GuidHelper.NewSequentialGuid() });
            list.Add(new AddressType() { Code = "W", Name = "Work address", IsActive = true, Id = GuidHelper.NewSequentialGuid() });
            list.Add(new AddressType() { Code = "A", Name = "Alternate address", IsActive = true, Id = GuidHelper.NewSequentialGuid() });

            uow.CreateRepositoryForDataSource(list);
            return list;
        }    

    }

    
}
