using DDI.Shared.Models.Client.GL;
using DDI.Services.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared;
using DDI.Shared.Models;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace DDI.Services.GL
{
    public class BusinessUnitFromToService : ServiceBase<BusinessUnitFromTo>, IBusinessUnitFromToService
    {
        public IDataResponse<List<ICanTransmogrify>> GetForFiscalYear(Guid? yearId)
        {
            List<BusinessUnitFromTo> results = new List<BusinessUnitFromTo>();

            // Get all the existing rows from the db.
            var list = UnitOfWork.GetEntities(IncludesForList).Where(p => p.FiscalYearId == yearId).ToList();

            // Get the fiscal year
            FiscalYear year = UnitOfWork.GetById<FiscalYear>(yearId.Value, p => p.Ledger.BusinessUnit);

            // Add the entry for default.
            BusinessUnitFromTo target = list.FirstOrDefault(p => p.OffsettingBusinessUnitId == null);
            if (target != null)
            {
                target.Name = "(Default)";
                results.Add(target);
            }
            else
            {
                results.Add(new BusinessUnitFromTo()
                {
                    Name = "(Default)",
                    FiscalYear = year,
                    FiscalYearId = yearId,
                    BusinessUnit = year.Ledger.BusinessUnit,
                    BusinessUnitId = year.Ledger.BusinessUnitId
                });
            }

            // Add entries for each offsetting business unit.
            foreach (var entry in UnitOfWork.GetEntities<BusinessUnit>().Where(p => p.BusinessUnitType != Shared.Enums.GL.BusinessUnitType.Organization).OrderBy(p => p.Code))
            {
                if (entry.Id == year.Ledger.BusinessUnitId)
                {
                    // Ignore the business unit that matches the fiscal year.
                    continue;
                }

                target = list.FirstOrDefault(p => p.OffsettingBusinessUnitId == entry.Id);
                if (target != null)
                {
                    target.Name = entry.Code;
                    results.Add(target);
                }
                else
                {
                    results.Add(new BusinessUnitFromTo()
                    {
                        Name = entry.Code,
                        FiscalYear = year,
                        FiscalYearId = yearId,
                        BusinessUnit = year.Ledger.BusinessUnit,
                        BusinessUnitId = year.Ledger.BusinessUnitId,
                        OffsettingBusinessUnit = entry,
                        OffsettingBusinessUnitId = entry.Id,                        
                    });
                }

            }

            FormatEntityListForGet(results);
            var response = GetIDataResponse(() => results.ToList<ICanTransmogrify>());
            response.TotalResults = results.Count;
            return response;
        }
    }

}
