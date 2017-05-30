using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Shared;
using DDI.Shared.Enums.GL;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.GL;

namespace DDI.Business.Tests.GL.DataSources
{
    public static class FiscalYearDataSource
    {
        public const string CLOSED_YEAR = "2016";
        public const string OPEN_YEAR = "2017";
        public const string EMPTY_YEAR = "2018";
        public const int CURRENT_PERIOD = 7;

        public static IList<FiscalYear> GetDataSource(IUnitOfWork uow)
        {
            IList<FiscalYear> existing = uow.GetRepositoryDataSource<FiscalYear>();
            if (existing != null)
            {
                return existing;
            }

            var ledgers = LedgerDataSource.GetDataSource(uow);
            var years = new List<FiscalYear>();
            var periods = new List<FiscalPeriod>();

            foreach (var ledger in ledgers)
            {
                if (ledger.Code != LedgerDataSource.NEW_LEDGER_CODE)
                {
                    years.Add(new FiscalYear()
                    {
                        Name = CLOSED_YEAR,
                        CurrentPeriodNumber = 12,
                        HasAdjustmentPeriod = false,
                        StartDate = DateTime.Parse($"1/1/{CLOSED_YEAR}"),
                        EndDate = DateTime.Parse($"12/31/{CLOSED_YEAR}"),
                        Ledger = ledger,
                        NumberOfPeriods = 12,
                        Status = FiscalYearStatus.Closed,
                        Id = GuidHelper.NewSequentialGuid()
                    });

                    years.Add(new FiscalYear()
                    {
                        Name = OPEN_YEAR,
                        CurrentPeriodNumber = CURRENT_PERIOD,
                        HasAdjustmentPeriod = true,
                        StartDate = DateTime.Parse($"1/1/{OPEN_YEAR}"),
                        EndDate = DateTime.Parse($"12/31/{OPEN_YEAR}"),
                        Ledger = ledger,
                        NumberOfPeriods = 13,
                        Status = FiscalYearStatus.Open,
                        Id = GuidHelper.NewSequentialGuid()
                    });
                }

                if (ledger.Code == LedgerDataSource.NEW_LEDGER_CODE || ledger.BusinessUnit.Code != BusinessUnitDataSource.UNIT_CODE_SEPARATE)
                {
                    years.Add(new FiscalYear()
                    {
                        Name = EMPTY_YEAR,
                        CurrentPeriodNumber = 1,
                        HasAdjustmentPeriod = true,
                        StartDate = DateTime.Parse($"1/1/{EMPTY_YEAR}"),
                        EndDate = DateTime.Parse($"12/31/{EMPTY_YEAR}"),
                        Ledger = ledger,
                        NumberOfPeriods = 13,
                        Status = FiscalYearStatus.Empty,
                        Id = GuidHelper.NewSequentialGuid()
                    });
                }

            }

            foreach (var ledger in ledgers)
            {
                ledger.FiscalYears = years.Where(p => p.Ledger == ledger).ToList();
            }

            foreach (var year in years)
            {
                year.FiscalPeriods = new List<FiscalPeriod>();
                AddPeriods(year, periods);
            }

            uow.CreateRepositoryForDataSource(years);
            uow.CreateRepositoryForDataSource(periods);
            return years;
        }    

        private static void AddPeriods(FiscalYear year, IList<FiscalPeriod> list)
        {
            int yearNumber = int.Parse(year.Name);
            for (int month = 1; month <= year.NumberOfPeriods; month++)
            {
                var period = new FiscalPeriod
                {
                    FiscalYear = year,
                    PeriodNumber = month,
                    IsAdjustmentPeriod = (month == 13)
                };

                if (month < 13)
                {
                    period.StartDate = new DateTime(yearNumber, month, 1);
                    period.EndDate = period.StartDate.Value.AddMonths(1).AddDays(-1);
                }
                else
                {
                    period.StartDate =
                    period.EndDate = new DateTime(yearNumber, 12, 31);
                }

                if (year.Status == FiscalYearStatus.Closed)
                {
                    period.Status = FiscalPeriodStatus.Closed;
                }
                else if (year.Status == FiscalYearStatus.Open)
                {
                    period.Status = (month < year.CurrentPeriodNumber ? FiscalPeriodStatus.Closed : FiscalPeriodStatus.Open);
                }
                else
                {
                    period.Status = FiscalPeriodStatus.Open;
                }
                list.Add(period);
                year.FiscalPeriods.Add(period);
            }
        }

    }

    
}
