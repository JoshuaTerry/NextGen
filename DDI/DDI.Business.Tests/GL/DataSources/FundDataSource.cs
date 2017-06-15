using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.GL;
using DDI.Shared;
using DDI.Shared.Enums.GL;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.GL;

namespace DDI.Business.Tests.GL.DataSources
{
    public static class FundDataSource
    {
        private static IList<Ledger> _ledgers;
        private static IList<FiscalYear> _years;
        private static IList<Segment> _segments;
        private static IList<Account> _accounts;
        private static IList<Fund> _funds;
        private static IList<FundFromTo> _fundFromTos;
        private static IList<BusinessUnitFromTo> _unitFromTos;
        private static IQueryable<LedgerAccount> _ledgerAccounts;

        public static IList<Fund> GetDataSource(IUnitOfWork uow)
        {
            IList<Fund> existing = uow.GetRepositoryDataSource<Fund>();
            if (existing != null)
            {
                return existing;
            }

            _ledgers = LedgerDataSource.GetDataSource(uow);
            _years = FiscalYearDataSource.GetDataSource(uow);
            _segments = SegmentDataSource.GetDataSource(uow);
            _accounts = AccountDataSource.GetDataSource(uow);

            _ledgerAccounts = uow.GetEntities<LedgerAccount>();

            CreateFunds(uow);
            CreateFundFromTos(uow);
            CreateUnitFromTos(uow);
            return _funds;
        }

        private static void CreateFunds(IUnitOfWork uow)
        {
            _funds = new List<Fund>();
            AccountLogic accountLogic = uow.GetBusinessLogic<AccountLogic>();

            foreach (var year in _years.Where(p => p.Ledger.BusinessUnit.BusinessUnitType != BusinessUnitType.Organization))
            {
                if (year.Ledger.Code == LedgerDataSource.NEW_LEDGER_CODE)
                {
                    Fund fund = new Fund();
                    fund.FiscalYear = year;
                    fund.FundBalanceLedgerAccount = accountLogic.GetLedgerAccount(_accounts.FirstOrDefault(p => p.FiscalYearId == year.Id && p.AccountNumber == "3001.CORP"));
                    fund.ClosingRevenueLedgerAccount = accountLogic.GetLedgerAccount(_accounts.FirstOrDefault(p => p.FiscalYearId == year.Id && p.AccountNumber == "3002.CORP"));
                    fund.ClosingExpenseLedgerAccount = accountLogic.GetLedgerAccount(_accounts.FirstOrDefault(p => p.FiscalYearId == year.Id && p.AccountNumber == "3003.CORP"));
                    fund.Id = GuidHelper.NewSequentialGuid();
                    _funds.Add(fund);
                }
                else
                {
                    Fund fund = new Fund();
                    fund.FiscalYear = year;
                    fund.FundSegment = _segments.FirstOrDefault(p => p.FiscalYearId == year.Id && p.Code == "01");
                    fund.FundBalanceLedgerAccount = accountLogic.GetLedgerAccount(_accounts.FirstOrDefault(p => p.FiscalYearId == year.Id && p.AccountNumber == "01-310-50-02"));
                    fund.ClosingExpenseLedgerAccount = fund.FundBalanceLedgerAccount;
                    fund.ClosingRevenueLedgerAccount = fund.FundBalanceLedgerAccount;
                    fund.Id = GuidHelper.NewSequentialGuid();
                    _funds.Add(fund);

                    fund = new Fund();
                    fund.FiscalYear = year;
                    fund.FundSegment = _segments.FirstOrDefault(p => p.FiscalYearId == year.Id && p.Code == "02");
                    fund.FundBalanceLedgerAccount = accountLogic.GetLedgerAccount(_accounts.FirstOrDefault(p => p.FiscalYearId == year.Id && p.AccountNumber == "02-380-50-02"));
                    fund.ClosingExpenseLedgerAccount = fund.FundBalanceLedgerAccount;
                    fund.ClosingRevenueLedgerAccount = fund.FundBalanceLedgerAccount;
                    fund.Id = GuidHelper.NewSequentialGuid();
                    _funds.Add(fund);

                    fund = new Fund();
                    fund.FiscalYear = year;
                    fund.FundSegment = _segments.FirstOrDefault(p => p.FiscalYearId == year.Id && p.Code == "03");
                    fund.FundBalanceLedgerAccount = accountLogic.GetLedgerAccount(_accounts.FirstOrDefault(p => p.FiscalYearId == year.Id && p.AccountNumber == "03-390-50-02"));
                    fund.ClosingExpenseLedgerAccount = fund.FundBalanceLedgerAccount;
                    fund.ClosingRevenueLedgerAccount = fund.FundBalanceLedgerAccount;
                    fund.Id = GuidHelper.NewSequentialGuid();
                    _funds.Add(fund);
                }

            }

            uow.CreateRepositoryForDataSource(_funds);        
        }

        private static void CreateUnitFromTos(IUnitOfWork uow)
        {
            _unitFromTos = new List<BusinessUnitFromTo>();
            foreach (var year in _years.Where(p => p.Ledger.FundAccounting == true && p.Ledger.BusinessUnit.BusinessUnitType != BusinessUnitType.Organization))
            {
                var entity = new BusinessUnitFromTo();
                entity.FiscalYear = year;
                entity.BusinessUnit = year.Ledger.BusinessUnit;
                entity.OffsettingBusinessUnit = null;
                entity.FromLedgerAccount = _ledgerAccounts.FirstOrDefault(p => p.Ledger == year.Ledger && p.AccountNumber == "01-150-50-50");
                entity.ToLedgerAccount = _ledgerAccounts.FirstOrDefault(p => p.Ledger == year.Ledger && p.AccountNumber == "01-150-50-51");
                entity.Id = GuidHelper.NewSequentialGuid();
                _unitFromTos.Add(entity);
            }

            uow.CreateRepositoryForDataSource(_unitFromTos);
        }

        private static void CreateFundFromTos(IUnitOfWork uow)
        {
            _fundFromTos = new List<FundFromTo>();

            foreach (var year in _years.Where(p => p.Ledger.FundAccounting == true && p.Ledger.BusinessUnit.BusinessUnitType != BusinessUnitType.Organization))
            {
                CreateFundFromTo(uow, year, "01", "02", "01-150-50-42", "01-150-50-43");
                CreateFundFromTo(uow, year, "01", "03", "01-150-50-44", "01-150-50-45");
                CreateFundFromTo(uow, year, "02", "01", "02-150-50-40", "02-150-50-41");
                CreateFundFromTo(uow, year, "02", "03", "02-150-50-44", "02-150-50-45");
                CreateFundFromTo(uow, year, "03", "01", "03-150-50-40", "03-150-50-41");
                CreateFundFromTo(uow, year, "03", "02", "03-150-50-42", "01-150-50-43");
            }
            uow.CreateRepositoryForDataSource(_fundFromTos);
        }

        private static void CreateFundFromTo(IUnitOfWork uow, FiscalYear year, string fundCode, string offsettingFundCode, string dueFrom, string dueTo)
        {
            FundFromTo entity = new FundFromTo();
            entity.FiscalYear = year;
            entity.Fund = _funds.FirstOrDefault(p => p.FiscalYear == year && p.FundSegment.Code == fundCode);
            entity.OffsettingFund = _funds.FirstOrDefault(p => p.FiscalYear == year && p.FundSegment.Code == offsettingFundCode);
            entity.FromLedgerAccount = _ledgerAccounts.FirstOrDefault(p => p.Ledger == year.Ledger && p.AccountNumber == dueFrom);
            entity.ToLedgerAccount = _ledgerAccounts.FirstOrDefault(p => p.Ledger == year.Ledger && p.AccountNumber == dueTo);
            entity.Id = GuidHelper.NewSequentialGuid();
            _fundFromTos.Add(entity);
        }

    }
}
