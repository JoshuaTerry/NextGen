using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Business.Tests.CRM.DataSources
{
    public static class GenderDataSource
    {
        public static IList<Gender> GetDataSource(IUnitOfWork uow)
        {
            IList<Gender> existing = uow.GetRepositoryDataSource<Gender>();
            if (existing != null)
            {
                return existing;
            }
            
            var genders = new List<Gender>();
            genders.Add(new Gender() { Code = "M", Name = "Male", IsMasculine = true, Id = GuidHelper.NewSequentialGuid() });
            genders.Add(new Gender() { Code = "F", Name = "Female", IsMasculine = false, Id = GuidHelper.NewSequentialGuid() });

            uow.CreateRepositoryForDataSource(genders);
            return genders;
        }    

    }

    
}
