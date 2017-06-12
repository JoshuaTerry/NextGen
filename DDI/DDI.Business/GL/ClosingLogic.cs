using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Helpers;
using DDI.Shared;
using DDI.Shared.Enums.GL;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics.GL;

namespace DDI.Business.GL
{
    public class ClosingLogic : IBusinessLogic
    {
        #region Private Fields



        #endregion

        #region Constructors 

        public ClosingLogic(IUnitOfWork uow)
        {
            UnitOfWork = uow;
            uow.AddBusinessLogic(this);           
        }

        #endregion

        #region Public Properties

        public IUnitOfWork UnitOfWork { get; private set; }

        #endregion

        #region Public Methods

        public void CloseFiscalPeriod(Guid fiscalPeriodId)
        {
            FiscalPeriod period = UnitOfWork.GetById<FiscalPeriod>(fiscalPeriodId, p => p.FiscalYear, p => p.FiscalYear.FiscalPeriods);

            if (period == null)
            {
                throw new InvalidOperationException(UserMessagesGL.BadFiscalPeriod);
            }

            if (period.FiscalYear.Status == FiscalYearStatus.Closed)
            {
                throw new InvalidOperationException(string.Format(UserMessagesGL.FiscalYearClosed, period.FiscalYear.Name));
            }

            if (period.Status == FiscalPeriodStatus.Closed)
            {
                throw new InvalidOperationException(string.Format(UserMessagesGL.FiscalPeriodClosed, period.PeriodNumber, period.FiscalYear.Name));
            }

            FiscalYear year = period.FiscalYear;

            // Get the next open period after the one being closed.
            FiscalPeriod newPeriod = year.FiscalPeriods.Where(p => p.PeriodNumber > period.PeriodNumber && p.IsAdjustmentPeriod == false && p.Status != FiscalPeriodStatus.Closed)
                .OrderBy(p => p.PeriodNumber)
                .FirstOrDefault();

            // If none exists, get the final non-adjustment period.
            if (newPeriod == null)
            {
                newPeriod = year.FiscalPeriods.Where(p => p.PeriodNumber > period.PeriodNumber && p.IsAdjustmentPeriod == false)
                    .OrderBy(p => p.PeriodNumber)
                    .FirstOrDefault();
            }

            if (newPeriod == null)
            {
                newPeriod = period;
            }

            // Close all periods up to and including the one being closed
            foreach (var entry in year.FiscalPeriods.Where(p => p.PeriodNumber <= period.PeriodNumber && p.Status != FiscalPeriodStatus.Closed))
            {
                entry.Status = FiscalPeriodStatus.Closed;
            }

            // Update the fiscal year
            year.CurrentPeriodNumber = newPeriod.PeriodNumber;

            UnitOfWork.SaveChanges();
        }

        public void CreateNewFiscalYear(Guid fiscalYearId, string newYearName, DateTime startDate, bool copyInactiveAccounts)
        {
            FiscalYear currentYear = UnitOfWork.GetById<FiscalYear>(fiscalYearId, p => p.Ledger.OrgLedger, p => p.Ledger.BusinessUnit, p => p.FiscalPeriods);
            if (currentYear == null)
            {
                throw new InvalidOperationException(UserMessagesGL.BadFiscalYear);
            }

            // Determine the parent ledger Id:  Either the org ledger id, or if there isn't one, the fiscal year's ledger id.
            Guid parentLedgerId = currentYear.Ledger.OrgLedgerId ?? currentYear.Ledger.Id;

            // Get set of ledgers to update:  All ledgers that are a child of the parent.  Also include the parent itself.
            IList<Ledger> ledgers = UnitOfWork.GetEntities<Ledger>(p => p.FiscalYears).Where(p => p.Id == parentLedgerId || p.OrgLedgerId == parentLedgerId).ToList();

            // Ensure the new year name doesn't exist in any of these ledgers, and that the start date doesn't exist in any of their fiscal years.
            foreach (var childLedger in ledgers)
            {
                if (childLedger.FiscalYears.Any(p => p.Name == newYearName))
                {
                    throw new InvalidOperationException(string.Format(UserMessagesGL.FiscalYearDuplicateName, newYearName));
                }
                if (childLedger.FiscalYears.Any(p => p.StartDate <= startDate && p.EndDate >= startDate))
                {
                    throw new InvalidOperationException(string.Format(UserMessagesGL.FiscalYearDuplicateStartDate, startDate.ToShortDateString()));
                }
            }

            var yearLogic = UnitOfWork.GetBusinessLogic<FiscalYearLogic>();            

            // Create the fiscal year in each child ledger.
            foreach (var childLedger in ledgers)
            {
                // Get the current fiscal year in this ledger.
                FiscalYear childYear = childLedger.FiscalYears.FirstOrDefault(p => p.StartDate == currentYear.StartDate);
                if (childYear != null)
                {
                    CreateNewFiscalYearForLedger(childLedger, currentYear, yearLogic, newYearName, startDate, copyInactiveAccounts);
                }
            }
        }

        #endregion

        #region Private Methods

        private void CreateNewFiscalYearForLedger(Ledger ledger, FiscalYear fromYear, FiscalYearLogic yearLogic, string newYearName, DateTime startDate, bool copyInactiveAccounts)
        {
            // Create the new fiscal year
            FiscalYear newYear = new FiscalYear();
            newYear.Ledger = ledger;
            newYear.Name = newYearName;
            newYear.StartDate = startDate;
            newYear.NumberOfPeriods = fromYear.NumberOfPeriods;
            newYear.Status = FiscalYearStatus.Empty;
            newYear.HasAdjustmentPeriod = fromYear.HasAdjustmentPeriod;
            newYear.CurrentPeriodNumber = 1;
            UnitOfWork.Insert(newYear);

            // Calculate start/end dates for each period.
            IList<DateTime[]> dates = yearLogic.CalculateDefaultStartEndDates(startDate, newYear.NumberOfPeriods, newYear.HasAdjustmentPeriod);

            // Create the fiscal periods.
            for (int n = 0; n < dates.Count; n++)
            {
                FiscalPeriod period = new FiscalPeriod();
                period.FiscalYear = newYear;
                period.PeriodNumber = n + 1;
                period.StartDate = dates[n][0];
                period.EndDate = dates[n][1];
                newYear.EndDate = period.EndDate;
                period.Status = FiscalPeriodStatus.Open;
                period.IsAdjustmentPeriod = (newYear.HasAdjustmentPeriod && period.PeriodNumber == newYear.NumberOfPeriods);
                UnitOfWork.Insert(period);
            }

            UnitOfWork.SaveChanges();

            // Copy entities into the new fiscal year.  The order is significant due to dependencies.
            EntityMapper<AccountGroup> accountGroups = CopyAccountGroups(fromYear, newYear);
            EntityMapper<Account> accounts = CopyAccounts(fromYear, newYear, copyInactiveAccounts, accountGroups);
            EntityMapper<Segment> segments = CopySegments(fromYear, newYear);
            CopyAccountSegments(fromYear, newYear, accounts, segments);
            CopyLedgerAccountYears(fromYear, newYear, accounts);

            EntityMapper<Fund> funds = CopyFunds(fromYear, newYear, segments);
            CopyFundFromTo(fromYear, newYear, funds);
            CopyBusinessUnitFromTo(fromYear, newYear);
        }

        private EntityMapper<AccountGroup> CopyAccountGroups(FiscalYear fromYear, FiscalYear toYear)
        {
            var mapper = new EntityMapper<AccountGroup>();

            // First pass copies the account groups.
            using (var batch = new BatchUnitOfWork<AccountGroup>().Where(p => p.FiscalYearId == fromYear.Id).AutoSaveChanges())
            {
                foreach (AccountGroup group in batch)
                {
                    AccountGroup newGroup = new AccountGroup();
                    newGroup.FiscalYearId = toYear.Id;
                    newGroup.Name = group.Name;
                    newGroup.Sequence = group.Sequence;
                    newGroup.Category = group.Category;
                    newGroup.AssignPrimaryKey();

                    mapper.Add(group.Id, newGroup.Id);
                    batch.UnitOfWork.Insert(newGroup);
                }
            }

            // Second pass links them up.
            using (var batch = new BatchUnitOfWork<AccountGroup>().Where(p => p.FiscalYearId == fromYear.Id && p.ParentGroupId != null)
                                                                  .AutoSaveChanges())
            {
                batch.OnNextBatch = (count, entities) =>
                {
                    foreach (AccountGroup group in entities)
                    {
                        Guid? newId = mapper.Get(group);
                        if (newId.HasValue)
                        {
                            AccountGroup newGroup = batch.UnitOfWork.GetById<AccountGroup>(newId.Value);
                            if (newGroup != null)
                            {
                                newGroup.ParentGroupId = mapper.Get(group.ParentGroupId);
                            }
                        }
                    }
                };

                batch.Process();
            }

            return mapper;
        }

        private EntityMapper<Account> CopyAccounts(FiscalYear fromYear, FiscalYear toYear, bool copyInactive, EntityMapper<AccountGroup> accountGroups)
        {
            var mapper = new EntityMapper<Account>();
            var closingAccounts = new EntityMapper<Account>();

            // Copy the accounts
            using (var batch = new BatchUnitOfWork<Account>().Where(p => p.FiscalYearId == fromYear.Id).AutoSaveChanges())
            {
                foreach (Account account in batch)
                {
                    // If this account is inactive and we're not copying inactive accounts, determine if this account
                    // has any posted transactions.  If not, skip the account.
                    if (!copyInactive && !account.IsActive)
                    {
                        bool hasTrans = false;
                        foreach (var entry in batch.UnitOfWork.GetReference(account, p => p.LedgerAccountYears))
                        {
                            hasTrans = batch.UnitOfWork.Any<PostedTransaction>(p => p.LedgerAccountYearId == entry.Id);
                            if (hasTrans)
                            {
                                break;
                            }
                        }

                        if (!hasTrans)
                        {
                            continue;
                        }
                    }

                    Account newAccount = new Account();
                    newAccount.FiscalYearId = account.FiscalYearId;
                    newAccount.AccountNumber = account.AccountNumber;
                    newAccount.Name = account.Name;
                    newAccount.Category = account.Category;
                    newAccount.Group1Id = accountGroups.Get(account.Group1Id);
                    newAccount.Group2Id = accountGroups.Get(account.Group2Id);
                    newAccount.Group3Id = accountGroups.Get(account.Group3Id);
                    newAccount.Group4Id = accountGroups.Get(account.Group4Id);
                    newAccount.SortKey = account.SortKey;
                    newAccount.IsNormallyDebit = account.IsNormallyDebit;
                    newAccount.IsActive = account.IsActive;
                    newAccount.BeginningBalance = 0m;
                    newAccount.ClosingAccountId = mapper.Get(account.ClosingAccountId);
                    newAccount.AssignPrimaryKey();

                    mapper.Add(account, newAccount);

                    if (account.ClosingAccountId != null && newAccount.ClosingAccountId == null)
                    {
                        closingAccounts.Add(newAccount.Id, account.ClosingAccountId.Value);
                    }
                }
            }

            // Now that all accounts have been copied, populate Account.ClosingAccount for those that couldn't be handled in the previous loop.
            using (var batch = new BatchUnitOfWork<Account>().Where(p => p.FiscalYearId == toYear.Id && p.ClosingAccountId == null).AutoSaveChanges())
            {
                foreach (Account account in batch)
                {
                    // Getting the prior year closingAccountId value.
                    Guid? closingAccountId = closingAccounts.Get(account.Id);
                    if (closingAccountId.HasValue)
                    {
                        // Get the new year closingAccountId value.
                        account.ClosingAccountId = mapper.Get(closingAccountId);
                    }
                }
            }

            return mapper;

        }

        private EntityMapper<Segment> CopySegments(FiscalYear fromYear, FiscalYear toYear)
        {
            var mapper = new EntityMapper<Segment>();

            Ledger ledger = UnitOfWork.GetReference(fromYear, p => p.Ledger);

            for (int level = 0; level < fromYear.Ledger.NumberOfSegments; level++)
            {
                using (var batch = new BatchUnitOfWork<Segment>().Where(p => p.FiscalYearId == fromYear.Id && p.Level == level).AutoSaveChanges())
                {
                    foreach (Segment segment in batch)
                    {
                        Segment newSegment = new Segment();
                        newSegment.FiscalYearId = toYear.Id;
                        newSegment.SegmentLevelId = segment.SegmentLevelId;
                        newSegment.Level = segment.Level;
                        newSegment.Code = segment.Code;
                        newSegment.Name = segment.Name;
                        newSegment.ParentSegmentId = mapper.Get(segment.ParentSegmentId);
                        newSegment.AssignPrimaryKey();

                        mapper.Add(segment, newSegment);

                        batch.UnitOfWork.Insert(newSegment);
                    }
                }
            }

            return mapper;
        }

        private void CopyAccountSegments(FiscalYear fromYear, FiscalYear toYear, EntityMapper<Account> accounts, EntityMapper<Segment> segments)
        {
            using (var batch = new BatchUnitOfWork<AccountSegment>().Where(p => p.Account.FiscalYearId == fromYear.Id).AutoSaveChanges())
            {
                foreach (AccountSegment segment in batch)
                {
                    AccountSegment newSegment = new AccountSegment();
                    newSegment.AccountId = accounts.Get(segment.AccountId);
                    newSegment.SegmentId = accounts.Get(segment.SegmentId);
                    newSegment.Level = segment.Level;
                    batch.UnitOfWork.Insert(newSegment);
                }
            }
        }

        private void CopyLedgerAccountYears(FiscalYear fromYear, FiscalYear toYear, EntityMapper<Account> accounts)
        {
            using (var batch = new BatchUnitOfWork<LedgerAccountYear>().Where(p => p.FiscalYearId == fromYear.Id).AutoSaveChanges())
            {
                foreach (LedgerAccountYear account in batch)
                {
                    LedgerAccountYear newAccount = new LedgerAccountYear();
                    newAccount.AccountId = accounts.Get(account.AccountId);
                    newAccount.FiscalYearId = toYear.Id;
                    newAccount.IsMerge = account.IsMerge;
                    newAccount.LedgerAccountId = account.LedgerAccountId;

                    batch.UnitOfWork.Insert(newAccount);
                }
            }
        }

        private EntityMapper<Fund> CopyFunds(FiscalYear fromYear, FiscalYear toYear, EntityMapper<Segment> segments)
        {
            var mapper = new EntityMapper<Fund>();

            using (var batch = new BatchUnitOfWork<Fund>().Where(p => p.FiscalYearId == fromYear.Id).AutoSaveChanges())
            {
                foreach (Fund fund in batch)
                {
                    Fund newFund = new Fund();
                    newFund.FiscalYearId = toYear.Id;
                    newFund.FundSegmentId = segments.Get(fund.FundSegmentId);
                    newFund.ClosingExpenseLedgerAccountId = fund.ClosingExpenseLedgerAccountId;
                    newFund.ClosingRevenueLedgerAccountId = fund.ClosingRevenueLedgerAccountId;
                    newFund.FundBalanceLedgerAccountId = fund.FundBalanceLedgerAccountId;
                    newFund.AssignPrimaryKey();

                    mapper.Add(fund, newFund);

                    batch.UnitOfWork.Insert(newFund);
                }
            }

            return mapper;
        }

        private void CopyFundFromTo(FiscalYear fromYear, FiscalYear toYear, EntityMapper<Fund> funds)
        {
            using (var batch = new BatchUnitOfWork<FundFromTo>().Where(p => p.FiscalYearId == fromYear.Id).AutoSaveChanges())
            {
                foreach (FundFromTo fund in batch)
                {
                    FundFromTo newFund = new FundFromTo();
                    newFund.FiscalYearId = toYear.Id;
                    newFund.FundId = funds.Get(fund.FundId);
                    newFund.OffsettingFundId = funds.Get(fund.OffsettingFundId);
                    newFund.FromLedgerAccountId = fund.FromLedgerAccountId;
                    newFund.ToLedgerAccountId = fund.ToLedgerAccountId;
                }
            }
        }

        private void CopyBusinessUnitFromTo(FiscalYear fromYear, FiscalYear toYear)
        {
            using (var batch = new BatchUnitOfWork<BusinessUnitFromTo>().Where(p => p.FiscalYearId == fromYear.Id).AutoSaveChanges())
            {
                foreach (BusinessUnitFromTo unit in batch)
                {
                    BusinessUnitFromTo newUnit = new BusinessUnitFromTo();
                    newUnit.FiscalYearId = toYear.Id;
                    newUnit.BusinessUnitId = unit.BusinessUnitId;
                    newUnit.OffsettingBusinessUnitId = unit.OffsettingBusinessUnitId;
                    newUnit.FromLedgerAccountId = unit.FromLedgerAccountId;
                    newUnit.ToLedgerAccountId = unit.ToLedgerAccountId;
                }
            }

        }

        #endregion

    }
}
