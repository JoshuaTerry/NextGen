using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Shared;
using DDI.Shared.Enums.Common;
using DDI.Shared.Models.Client.Core;

namespace DDI.Services
{
    public class CustomFieldService : ServiceBase<CustomField>, ICustomFieldService
    {

        public IDataResponse<List<CustomField>> GetByEntityId(int entityId, Guid constituentId)
        {
            var answers = UnitOfWork.GetEntities<CustomFieldData>().Where(a => a.ParentEntityId == constituentId).ToList();
            var customfields = UnitOfWork.GetEntities(base.IncludesForList).Where(c => c.Entity == (CustomFieldEntity)entityId).ToList();

            customfields.ForEach(f => f.Data = answers.Where(a => a.CustomFieldId == f.Id).ToList());

            return GetIDataResponse(() => customfields);
        }
    }
}
