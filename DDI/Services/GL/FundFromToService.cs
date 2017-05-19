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
using DDI.Business.GL;
using DDI.Data;

namespace DDI.Services.GL
{
    public class FundFromToService : ServiceBase<FundFromTo>, IFundFromToService
    {
        protected override Action<FundFromTo> FormatEntityForGet => AddLedgerAccountIds;
        private AccountLogic _accountLogic;

        public FundFromToService() : this(new UnitOfWorkEF())  { }

        public FundFromToService(IUnitOfWork uow) : base(uow)          
        {
            _accountLogic = uow.GetBusinessLogic<AccountLogic>();
        }

        private void AddLedgerAccountIds (FundFromTo entity)
        {
            entity.FromAccountId = _accountLogic.GetAccount(entity.FromLedgerAccount, entity.FiscalYear)?.Id;
            entity.ToAccountId = _accountLogic.GetAccount(entity.ToLedgerAccount, entity.FiscalYear)?.Id;
        }

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

        public override IDataResponse<FundFromTo> Add(FundFromTo entity)
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
            if (name == nameof(FundFromTo.FromAccountId) && entity is FundFromTo)
            {
                var fundFromTo = (FundFromTo)entity;
                fundFromTo.FromLedgerAccount = _accountLogic.GetLedgerAccount(token.ToObject<Guid?>());
                fundFromTo.FromLedgerAccountId = fundFromTo.FromLedgerAccount?.Id;
                return true;
            }

            if (name == nameof(FundFromTo.ToAccountId) && entity is FundFromTo)
            {
                var fundFromTo = (FundFromTo)entity;
                fundFromTo.ToLedgerAccount = _accountLogic.GetLedgerAccount(token.ToObject<Guid?>());
                fundFromTo.ToLedgerAccountId = fundFromTo.ToLedgerAccount?.Id;
                return true;
            }

            return false;
        }
    }

}
