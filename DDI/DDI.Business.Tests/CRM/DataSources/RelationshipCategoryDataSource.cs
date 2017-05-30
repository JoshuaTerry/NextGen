using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Business.Tests.CRM.DataSources
{
    public static class RelationshipCategoryDataSource
    {
        public static IList<RelationshipCategory> GetDataSource(IUnitOfWork uow)
        {
            IList<RelationshipCategory> existing = uow.GetRepositoryDataSource<RelationshipCategory>();
            if (existing != null)
            {
                return existing;
            }

            var list = new List<RelationshipCategory>();
            list.Add(new RelationshipCategory() { Code = "M", Name = "Membership", IsActive = true, IsShownInQuickView = false, Id = GuidHelper.NewSequentialGuid() });
            list.Add(new RelationshipCategory() { Code = "G", Name = "General", IsActive = true, IsShownInQuickView = true, Id = GuidHelper.NewSequentialGuid() });

            uow.CreateRepositoryForDataSource(list);
            return list;
        }    

    }

    
}
