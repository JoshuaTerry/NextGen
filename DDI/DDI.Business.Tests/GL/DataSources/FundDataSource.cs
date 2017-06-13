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
                    fund.FundBalanceLedgerAccount = accountLogic.GetLedgerAccount(_accounts.FirstOrDefault(p => p.FiscalYearId == year.Id &&  p.AccountNumber == "01-310-50-02"));
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
            return _funds;
        }

    }
}
