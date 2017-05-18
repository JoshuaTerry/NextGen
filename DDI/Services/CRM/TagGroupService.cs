using DDI.Data;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Client.CRM;
using System.Collections.Generic;
using System.Linq;

namespace DDI.Services
{
    public class TagGroupService : ServiceBase<TagGroup>, ITagGroupService
    {

        public IDataResponse<List<TagGroup>> GetGroupsAndTags(ConstituentCategory category)
        {
            var results = UnitOfWork.GetEntities<TagGroup>(p => p.Tags).Where(tg => tg.IsActive).OrderBy(tg => tg.Order).ToList();

            results.ForEach(tg => tg.Tags = tg.Tags.Where(t => t.IsActive && 
                                                               (category == ConstituentCategory.Both || t.ConstituentCategory == ConstituentCategory.Both || t.ConstituentCategory == category))
                                                   .OrderBy(t => t.Order)
                                                   .ToList());

            return GetIDataResponse(() => results);
        }
    }
}
