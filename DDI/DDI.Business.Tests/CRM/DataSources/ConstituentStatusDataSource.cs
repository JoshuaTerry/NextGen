using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Business.Tests.CRM.DataSources
{
    public static class ConstituentStatusDataSource
    {
        public static IList<ConstituentStatus> GetDataSource(IUnitOfWork uow)
        {
            IList<ConstituentStatus> existing = uow.GetRepositoryDataSource<ConstituentStatus>();
            if (existing != null)
            {
                return existing;
            }

            var list = new List<ConstituentStatus>();

            list.Add(new ConstituentStatus() { BaseStatus = Shared.Enums.CRM.ConstituentBaseStatus.Active, Code = "AC", Name = "Active", IsActive = true, IsRequired = true, Id = GuidHelper.NewSequentialGuid() });
            list.Add(new ConstituentStatus() { BaseStatus = Shared.Enums.CRM.ConstituentBaseStatus.Inactive, Code = "IN", Name = "Inactive", IsActive = true, IsRequired = true, Id = GuidHelper.NewSequentialGuid() });
            list.Add(new ConstituentStatus() { BaseStatus = Shared.Enums.CRM.ConstituentBaseStatus.Blocked, Code = "BL", Name = "Blocked", IsActive = true, IsRequired = true, Id = GuidHelper.NewSequentialGuid() });
            list.Add(new ConstituentStatus() { BaseStatus = Shared.Enums.CRM.ConstituentBaseStatus.Inactive, Code = "DEL", Name = "Deleted", IsActive = true, IsRequired = true, Id = GuidHelper.NewSequentialGuid() });

            uow.CreateRepositoryForDataSource(list);
            return list;
        }    

    }

    
}
