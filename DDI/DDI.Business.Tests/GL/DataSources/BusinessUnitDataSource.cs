using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.GL;

namespace DDI.Business.Tests.GL.DataSources
{
    public static class BusinessUnitDataSource
    {
        public static IList<BusinessUnit> GetDataSource(UnitOfWorkNoDb uow)
        {
            IList<BusinessUnit> existing = uow.GetRepositoryOrNull<BusinessUnit>()?.Entities.ToList();
            if (existing != null)
            {
                return existing;
            }
            
            var busnessunits = new List<BusinessUnit>();
            busnessunits.Add(new BusinessUnit() { Code = "PAR", Name = "Parent Entity CEFMR",  BusinessUnitType= Shared.Enums.GL.BusinessUnitType.Common, Id = GuidHelper.NewSequentialGuid() });
            busnessunits.Add(new BusinessUnit() { Code = "DCEF", Name = "Disciples Church Extension Fund", BusinessUnitType = Shared.Enums.GL.BusinessUnitType.Common, Id = GuidHelper.NewSequentialGuid() });
            busnessunits.Add(new BusinessUnit() { Code = "HOPE", Name = "Hope Partnership", BusinessUnitType = Shared.Enums.GL.BusinessUnitType.Common, Id = GuidHelper.NewSequentialGuid() });
            busnessunits.Add(new BusinessUnit() { Code = "CE", Name = "Church Extension", BusinessUnitType = Shared.Enums.GL.BusinessUnitType.Common, Id = GuidHelper.NewSequentialGuid() });

            uow.CreateRepositoryForDataSource(busnessunits);
            return busnessunits;
        }    

    }

    
}
