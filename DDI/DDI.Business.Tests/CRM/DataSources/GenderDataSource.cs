using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Business.Tests.CRM.DataSources
{
    public static class GenderDataSource
    {
        public static IList<Gender> GetDataSource(UnitOfWorkNoDb uow)
        {
            IList<Gender> existing = uow.GetRepositoryOrNull<Gender>()?.Entities.ToList();
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
