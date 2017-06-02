using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Business.Tests.CRM.DataSources
{
    public static class ConstituentTypeDataSource
    {
        public static IList<ConstituentType> GetDataSource(IUnitOfWork uow)
        {
            IList<ConstituentType> existing = uow.GetRepositoryDataSource<ConstituentType>();
            if (existing != null)
            {
                return existing;
            }
            
            var constituentTypes = new List<ConstituentType>();
            constituentTypes.Add(new ConstituentType()
            {
                Category = ConstituentCategory.Individual,
                Code = "I",
                Name = "Individual",
                IsActive = true,
                IsRequired = true,
                Tags = new List<Tag>(),
                NameFormat = "{P}{F}{MI}{L}{S}",
                SalutationFormal = "Dear {P}{L}",
                SalutationInformal = "Dear {F}",
                Id = GuidHelper.NewSequentialGuid()
            });

            constituentTypes.Add(new ConstituentType()
            {
                Category = ConstituentCategory.Organization,
                Code = "O",
                Name = "Organization",
                IsActive = true,
                IsRequired = true,
                Tags = new List<Tag>(),
                NameFormat = "",
                SalutationFormal = "Dear Friends",
                SalutationInformal = "Dear Friends",
                Id = GuidHelper.NewSequentialGuid()
            });

            constituentTypes.Add(new ConstituentType()
            {
                Category = ConstituentCategory.Organization,
                Code = "C",
                Name = "Church",
                IsActive = true,
                IsRequired = true,
                Tags = new List<Tag>(),
                NameFormat = "",
                SalutationFormal = "Dear Friends",
                SalutationInformal = "Dear Friends",
                Id = GuidHelper.NewSequentialGuid()
            });

            uow.CreateRepositoryForDataSource(constituentTypes);
            return constituentTypes;
        }    

    }

    
}
