using System;
using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Models.Client;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Services
{
    interface IContactInfoService
    {
        IDataResponse<List<ContactInfo>> GetContactInfoByContactCategoryForConstituent(Guid? categoryId, Guid? constituentId);
    }
}
