using System;
using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Models.Client;
using DDI.Shared.Models.Client.CRM;
using DDI.Services.ServiceInterfaces;

namespace DDI.Services
{
    public interface IContactInfoService : IService<ContactInfo>
    {
        IDataResponse<List<ContactInfo>> GetContactInfoByContactCategoryForConstituent(Guid? categoryId, Guid? constituentId);
    }
}
