﻿using System;
using DDI.Business.GL;
using DDI.Shared;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.GL;
using Newtonsoft.Json.Linq;

namespace DDI.Services.GL
{
    public class FundService : ServiceBase<Fund>
    {
        protected override Action<Fund, string> FormatEntityForGet => AddLedgerAccountIds;
        private AccountLogic _accountLogic;

        public FundService(IUnitOfWork uow, AccountLogic accountLogic) : base(uow)
        {
            _accountLogic = accountLogic;
        }

        private void AddLedgerAccountIds (Fund entity, string fields)
        {
            entity.FundBalanceAccountId = _accountLogic.GetAccount(entity.FundBalanceLedgerAccount, entity.FiscalYear)?.Id;
            entity.ClosingRevenueAccountId = _accountLogic.GetAccount(entity.ClosingRevenueLedgerAccount, entity.FiscalYear)?.Id;
            entity.ClosingExpenseAccountId = _accountLogic.GetAccount(entity.ClosingExpenseLedgerAccount, entity.FiscalYear)?.Id;
        }               

        public override IDataResponse<Fund> Add(Fund entity)
        {
            if (entity.FundBalanceAccountId != null)
            {
                entity.FundBalanceLedgerAccount = _accountLogic.GetLedgerAccount(entity.FundBalanceAccountId);
                entity.FundBalanceLedgerAccountId = entity.FundBalanceLedgerAccount?.Id;
            }

            if (entity.ClosingRevenueAccountId != null)
            {
                entity.ClosingRevenueLedgerAccount = _accountLogic.GetLedgerAccount(entity.ClosingRevenueAccountId);
                entity.ClosingRevenueLedgerAccountId = entity.ClosingRevenueLedgerAccount?.Id;
            }

            if (entity.ClosingExpenseAccountId != null)
            {
                entity.ClosingExpenseLedgerAccount = _accountLogic.GetLedgerAccount(entity.ClosingExpenseAccountId);
                entity.ClosingExpenseLedgerAccountId = entity.ClosingExpenseLedgerAccount?.Id;
            }

            return base.Add(entity);
        }


        protected override bool ProcessJTokenUpdate(IEntity entity, string name, JToken token)
        {
            if (name == nameof(Fund.FundBalanceAccountId) && entity is Fund)
            {
                var fundFromTo = (Fund)entity;
                fundFromTo.FundBalanceLedgerAccount = _accountLogic.GetLedgerAccount(token.ToObject<Guid?>());
                fundFromTo.FundBalanceLedgerAccountId = fundFromTo.FundBalanceLedgerAccount?.Id;
                return true;
            }

            if (name == nameof(Fund.ClosingRevenueAccountId) && entity is Fund)
            {
                var fundFromTo = (Fund)entity;
                fundFromTo.ClosingRevenueLedgerAccount = _accountLogic.GetLedgerAccount(token.ToObject<Guid?>());
                fundFromTo.ClosingRevenueLedgerAccountId = fundFromTo.ClosingRevenueLedgerAccount?.Id;
                return true;
            }

            if (name == nameof(Fund.ClosingExpenseAccountId) && entity is Fund)
            {
                var fundFromTo = (Fund)entity;
                fundFromTo.ClosingExpenseLedgerAccount = _accountLogic.GetLedgerAccount(token.ToObject<Guid?>());
                fundFromTo.ClosingExpenseLedgerAccountId = fundFromTo.ClosingExpenseLedgerAccount?.Id;
                return true;
            }

            return false;
        }
    }

}
