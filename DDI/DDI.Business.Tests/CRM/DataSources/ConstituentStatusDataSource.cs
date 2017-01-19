using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Tests.Helpers;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Business.Tests.CRM.DataSources
{
    public static class ConstituentStatusDataSource
    {
        public static IList<ConstituentStatus> GetDataSource(UnitOfWorkNoDb uow)
        {
            var list = new List<ConstituentStatus>();

            list.Add(new ConstituentStatus() { BaseStatus = Shared.Enums.CRM.ConstituentBaseStatus.Active, Code = "AC", Name = "Active", IsActive = true, IsRequired = true, Id = GuidHelper.NextGuid() });
            list.Add(new ConstituentStatus() { BaseStatus = Shared.Enums.CRM.ConstituentBaseStatus.Inactive, Code = "IN", Name = "Inactive", IsActive = true, IsRequired = true, Id = GuidHelper.NextGuid() });
            list.Add(new ConstituentStatus() { BaseStatus = Shared.Enums.CRM.ConstituentBaseStatus.Blocked, Code = "BL", Name = "Blocked", IsActive = true, IsRequired = true, Id = GuidHelper.NextGuid() });
            list.Add(new ConstituentStatus() { BaseStatus = Shared.Enums.CRM.ConstituentBaseStatus.Inactive, Code = "DEL", Name = "Deleted", IsActive = true, IsRequired = true, Id = GuidHelper.NextGuid() });

            uow.CreateRepositoryForDataSource(list);
            return list;
        }    

    }

    
}
