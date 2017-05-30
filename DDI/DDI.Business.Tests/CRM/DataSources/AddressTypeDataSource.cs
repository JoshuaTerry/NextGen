using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Business.Tests.CRM.DataSources
{
    public static class AddressTypeDataSource
    {
        public static IList<AddressType> GetDataSource(IUnitOfWork uow)
        {
            IList<AddressType> existing = uow.GetRepositoryDataSource<AddressType>();
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
