using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Business.GL;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.Services.GL
{
    public class BusinessUnitFromToService : ServiceBase<BusinessUnitFromTo>, IBusinessUnitFromToService
    {
        protected override Action<BusinessUnitFromTo,string> FormatEntityForGet => AddLedgerAccountIds;
        private AccountLogic _accountLogic;

        public BusinessUnitFromToService(IUnitOfWork uow, AccountLogic accountLogic) : base(uow)          
        {
            _accountLogic = accountLogic;
        }

        private void AddLedgerAccountIds(BusinessUnitFromTo entity, string fields)
        {
            entity.FromAccountId = _accountLogic.GetAccount(entity.FromLedgerAccount, entity.FiscalYear)?.Id;
            entity.ToAccountId = _accountLogic.GetAccount(entity.ToLedgerAccount, entity.FiscalYear)?.Id;
        }
        
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

            FormatEntityListForGet(results, FieldLists.AllFields);
            var response = GetIDataResponse(() => results.ToList<ICanTransmogrify>());
            response.TotalResults = results.Count;
            return response;
        }

        public override IDataResponse<BusinessUnitFromTo> Add(BusinessUnitFromTo entity)
        {
            if (entity.FromAccountId != null)
            {
                entity.FromLedgerAccount = _accountLogic.GetLedgerAccount(entity.FromAccountId);
                entity.FromLedgerAccountId = entity.FromLedgerAccount?.Id;
            }

            if (entity.ToAccountId != null)
            {
                entity.ToLedgerAccount = _accountLogic.GetLedgerAccount(entity.ToAccountId);
                entity.ToLedgerAccountId = entity.ToLedgerAccount?.Id;
            }
            return base.Add(entity);
        }

        protected override bool ProcessJTokenUpdate(IEntity entity, string name, JToken token)
        {
            if (name == nameof(BusinessUnitFromTo.FromAccountId) && entity is BusinessUnitFromTo)
            {
                var fundFromTo = (BusinessUnitFromTo)entity;
                fundFromTo.FromLedgerAccount = _accountLogic.GetLedgerAccount(token.ToObject<Guid?>());
                fundFromTo.FromLedgerAccountId = fundFromTo.FromLedgerAccount?.Id;
                return true;
            }

            if (name == nameof(BusinessUnitFromTo.ToAccountId) && entity is BusinessUnitFromTo)
            {
                var fundFromTo = (BusinessUnitFromTo)entity;
                fundFromTo.ToLedgerAccount = _accountLogic.GetLedgerAccount(token.ToObject<Guid?>());
                fundFromTo.ToLedgerAccountId = fundFromTo.ToLedgerAccount?.Id;
                return true;
            }

            return false;
        }
    }

}
