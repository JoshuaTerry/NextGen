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
    public class FundFromToService : ServiceBase<FundFromTo>, IFundFromToService
    {
        public IDataResponse<List<ICanTransmogrify>> GetForFund(Guid? fundId)
        {
            List<FundFromTo> results = new List<FundFromTo>();

            // Get all the existing rows from the db.
            var list = UnitOfWork.GetEntities(IncludesForList).Where(p => p.FundId == fundId).ToList();

            // Get the fund
            Fund fund = UnitOfWork.GetById<Fund>(fundId.Value, p => p.FiscalYear.Ledger.BusinessUnit);

            // Get the fiscal year
            FiscalYear year = fund.FiscalYear;

            // Add the entry for default.
            FundFromTo target = list.FirstOrDefault(p => p.OffsettingFundId == null);
            if (target != null)
            {
                target.Name = "(Default)";
                results.Add(target);
            }
            else
            {
                results.Add(new FundFromTo()
                {
                    Name = "(Default)",
                    FiscalYear = year,
                    FiscalYearId = year.Id,
                    Fund = fund,
                    FundId = fund.Id
                });
            }

            // Add entries for each offsetting fund
            foreach (var entry in UnitOfWork.GetEntities<Fund>(p => p.FundSegment).Where(p => p.FiscalYearId == year.Id).OrderBy(p => p.FundSegment.Code))
            {
                if (entry.Id == fundId)
                {
                    // Ignore the fund that matches the fundId passed in.
                    continue;
                }

                target = list.FirstOrDefault(p => p.OffsettingFundId == entry.Id);
                if (target != null)
                {
                    target.Name = entry.FundSegment.Code;
                    results.Add(target);
                }
                else
                {
                    results.Add(new FundFromTo()
                    {
                        Name = entry.FundSegment.Code,
                        FiscalYear = year,
                        FiscalYearId = year.Id,
                        Fund = fund,
                        FundId = fund.Id,
                        OffsettingFund = entry,
                        OffsettingFundId = entry.Id,                        
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
