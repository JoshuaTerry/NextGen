using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared.Enums.GL;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.GL;

namespace DDI.Business.Tests.GL.DataSources
{
    public static class BusinessUnitDataSource
    {
        public const string UNIT_CODE1 = "ABC";
        public const string UNIT_CODE2 = "DEF";
        public const string UNIT_CODE_SEPARATE = "XYZ";
        
        public static IList<BusinessUnit> GetDataSource(UnitOfWorkNoDb uow)
        {
            IList<BusinessUnit> existing = uow.GetRepositoryOrNull<BusinessUnit>()?.Entities.ToList();
            if (existing != null)
            {
                return existing;
            }
            
            var businessUnits = new List<BusinessUnit>();
            businessUnits.Add(new BusinessUnit() { Code = "*", Name = "Organizational Business Unit", BusinessUnitType = BusinessUnitType.Organization, Id = GuidHelper.NewSequentialGuid() });
            businessUnits.Add(new BusinessUnit() { Code = UNIT_CODE1, Name = "Common Business Unit 1",  BusinessUnitType= BusinessUnitType.Common, Id = GuidHelper.NewSequentialGuid() });
            businessUnits.Add(new BusinessUnit() { Code = UNIT_CODE2, Name = "Common Busienss Unit 2", BusinessUnitType = BusinessUnitType.Common, Id = GuidHelper.NewSequentialGuid() });
            businessUnits.Add(new BusinessUnit() { Code = UNIT_CODE_SEPARATE, Name = "Separate Business Unit", BusinessUnitType = BusinessUnitType.Separate, Id = GuidHelper.NewSequentialGuid() });

            uow.CreateRepositoryForDataSource(businessUnits);
            return businessUnits;
        }    

    }

    
}
