using DDI.Data;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DDI.Services
{
    public class AlternateIdService : ServiceBase<AlternateId>, IAlternateIdService
    {
        public IDataResponse<List<AlternateId>> GetAlternateIdsByConstituent(Guid constituentId, IPageable search)
        {
            var queryable = UnitOfWork.GetRepository<AlternateId>().Entities.Where(a => a.ConstituentId == constituentId);
            return GetPagedResults(queryable, search);
        }
    }
}
