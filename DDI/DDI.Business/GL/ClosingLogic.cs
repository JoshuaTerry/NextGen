using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Helpers;
using DDI.Shared;
using DDI.Shared.Enums.Core;
using DDI.Shared.Enums.GL;
using DDI.Shared.Extensions;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics.GL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        /// <summary>
        /// Close a fiscal period.
        /// </summary>
        /// <param name="fiscalPeriodId">Id of fiscal period to be closed.</param>
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

        /// <summary>
        /// Re-open a closed fiscal period.
        /// </summary>
        /// <param name="fiscalPeriodId">Id of fiscal period to be re-opened.</param>
        public void ReopenFiscalPeriod(Guid fiscalPeriodId)
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

            if (period.Status != FiscalPeriodStatus.Closed)
            {
                throw new InvalidOperationException(string.Format(UserMessagesGL.FiscalPeriodOpen, period.PeriodNumber, period.FiscalYear.Name));
            }

            FiscalYear year = period.FiscalYear;

            // Open all periods after the one being re-opened.
            foreach (var entry in year.FiscalPeriods.Where(p => p.PeriodNumber >= period.PeriodNumber && p.Status == FiscalPeriodStatus.Closed))
            {
                entry.Status = FiscalPeriodStatus.Reopened;
            }

            // Update the fiscal year
            if (year.CurrentPeriodNumber > period.PeriodNumber)
            {
                year.CurrentPeriodNumber = period.PeriodNumber;
            }

            UnitOfWork.SaveChanges();
        }

        /// <summary>
        /// Close a fiscal year.
        /// </summary>
        /// <param name="fiscalYearId">Id of fiscal year to be closed.</param>
        public void CloseFiscalYear(Guid fiscalYearId)
        {
            FiscalYear year = UnitOfWork.GetById<FiscalYear>(fiscalYearId, p => p.FiscalPeriods);

            if (year == null)
            {
                throw new InvalidOperationException(UserMessagesGL.BadFiscalYear);
            }

            if (year.Status == FiscalYearStatus.Closed)
            {
                throw new InvalidOperationException(string.Format(UserMessagesGL.FiscalYearClosed, year.Name));
            }

            // Get the final non-adjustment period in the year
            FiscalPeriod finalPeriod = year.FiscalPeriods.Where(p => p.IsAdjustmentPeriod == false).OrderByDescending(p => p.PeriodNumber).FirstOrDefault();
            
            // if this period isn't closed, then close it.
            if (finalPeriod != null && finalPeriod.Status != FiscalPeriodStatus.Closed)
            {
                CloseFiscalPeriod(finalPeriod.Id);
            }

            // Update the fiscal year.
            year.Status = FiscalYearStatus.Closed;
            UnitOfWork.SaveChanges();
            
            // Perform the year closing procedure.
            PerformYearClose(year);
        }

        /// <summary>
        /// Re-open a closed fiscal year.
        /// </summary>
        /// <param name="fiscalYearId">Id of fiscal year to be re-opened.</param>
        public void ReopenFiscalYear(Guid fiscalYearId)
        {
            FiscalYear year = UnitOfWork.GetById<FiscalYear>(fiscalYearId, p => p.FiscalPeriods);

            if (year == null)
            {
                throw new InvalidOperationException(UserMessagesGL.BadFiscalYear);
            }

            if (year.Status != FiscalYearStatus.Closed)
            {
                throw new InvalidOperationException(string.Format(UserMessagesGL.FiscalYearOpen, year.Name));
            }

            // Get the final non-adjustment period in the year
            FiscalPeriod finalPeriod = year.FiscalPeriods.Where(p => p.IsAdjustmentPeriod == false).OrderByDescending(p => p.PeriodNumber).FirstOrDefault();
            if (finalPeriod == null)
            {
                throw new InvalidOperationException(string.Format(UserMessagesGL.FiscalYearCantReopen, year.Name));
            }

            // Reopen the year.
            year.Status = FiscalYearStatus.Reopened;
            UnitOfWork.SaveChanges();

            // Reopen the final period.
            ReopenFiscalPeriod(finalPeriod.Id);
        }

        /// <summary>
        /// Re-close a closed fiscal year.
        /// </summary>
        /// <param name="fiscalYearId">Id of fiscal year to be re-closed.</param>
        public void RecloseFiscalYear(Guid fiscalYearId)
        {
            FiscalYear year = UnitOfWork.GetById<FiscalYear>(fiscalYearId, p => p.FiscalPeriods);

            if (year == null)
            {
                throw new InvalidOperationException(UserMessagesGL.BadFiscalYear);
            }

            if (year.Status != FiscalYearStatus.Closed)
            {
                throw new InvalidOperationException(string.Format(UserMessagesGL.FiscalYearOpen, year.Name));
            }

            if (UnitOfWork.GetBusinessLogic<FiscalYearLogic>().GetNextFiscalYear(year) == null)
            {
                throw new InvalidOperationException(string.Format(UserMessagesGL.FiscalYearCantReclose, year.Name));
            }

            PerformYearClose(year);
        }

        /// <summary>
        /// Create a new fiscal year.
        /// </summary>
        /// <param name="fiscalYearId">Id of fiscal year to be copied.</param>
        /// <param name="newYearName">Name of new fiscal year.</param>
        /// <param name="startDate">Start date of new fiscal year.</param>
        /// <param name="copyInactiveAccounts">TRUE to copy all inactive accounts.</param>
        public Guid CreateNewFiscalYear(Guid fiscalYearId, string newYearName, DateTime startDate, bool copyInactiveAccounts)
        {
            Guid newYearId = Guid.Empty;

            if (string.IsNullOrWhiteSpace(newYearName))
            {
                throw new InvalidOperationException(UserMessagesGL.FiscalYearNoName);
            }

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
                    Guid id = CreateNewFiscalYearForLedger(childLedger, childYear, yearLogic, newYearName, startDate, copyInactiveAccounts);
                    if (childYear == currentYear)
                    {
                        newYearId = id; // Returning the Id of the new fiscal year
                    }
                }
            }

            return newYearId;
        }

        #endregion

        #region Private Methods

        private Guid CreateNewFiscalYearForLedger(Ledger ledger, FiscalYear fromYear, FiscalYearLogic yearLogic, string newYearName, DateTime startDate, bool copyInactiveAccounts)
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
            newYear.AssignPrimaryKey();
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
                period.AssignPrimaryKey();
                UnitOfWork.Insert(period);
            }

            UnitOfWork.SaveChanges();

            if (fromYear.Ledger == newYear.Ledger)
            {
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

            return newYear.Id;
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
            using (var batch = new BatchUnitOfWork<Account>(p => p.LedgerAccountYears).Where(p => p.FiscalYearId == fromYear.Id).AutoSaveChanges())
            {
                foreach (Account account in batch)
                {
                    // If this account is inactive and we're not copying inactive accounts, determine if this account
                    // has any posted transactions.  If not, skip the account.
                    if (!copyInactive && !account.IsActive)
                    {
                        bool hasTrans = false;
                        foreach (var entry in account.LedgerAccountYears)
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
                    newAccount.FiscalYearId = toYear.Id;
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

                    batch.UnitOfWork.Insert(newAccount);

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

            for (int level = 1; level <= fromYear.Ledger.NumberOfSegments; level++)
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
            using (var batch = new BatchUnitOfWork<AccountSegment>().Where(p => p.Account != null && p.Account.FiscalYearId == fromYear.Id).AutoSaveChanges())
            {
                foreach (AccountSegment segment in batch)
                {
                    AccountSegment newSegment = new AccountSegment();
                    newSegment.AccountId = accounts.Get(segment.AccountId);                    
                    newSegment.SegmentId = segments.Get(segment.SegmentId);
                    newSegment.Level = segment.Level;
                    newSegment.AssignPrimaryKey();
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
                    newAccount.AssignPrimaryKey();
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
                    newFund.AssignPrimaryKey();
                    batch.UnitOfWork.Insert(newFund);
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
                    newUnit.AssignPrimaryKey();
                    batch.UnitOfWork.Insert(newUnit);
                }
            }

        }


        private void PerformYearClose(FiscalYear year)
        {
            var closeDict = new Dictionary<Account, CloseTo>();
            FiscalYearLogic fiscalYearLogic = UnitOfWork.GetBusinessLogic<FiscalYearLogic>();

            if (year == null)
                throw new ArgumentNullException(nameof(year));

            FiscalYear nextYear = fiscalYearLogic.GetNextFiscalYear(year);

            FiscalPeriod finalPeriod = year.FiscalPeriods.Where(p => p.IsAdjustmentPeriod == false).OrderByDescending(p => p.PeriodNumber).FirstOrDefault();

            int finalPeriodNumber = finalPeriod?.PeriodNumber ?? 0;

            IRepository<PostedTransaction> repo = UnitOfWork.GetRepository<PostedTransaction>();

            // Physically delete all PostedTran rows with a type of "CloseFrom" and "CloseTo" for the fiscal year.
            string sql = $"DELETE FROM {EntityFrameworkHelper.GetTableName<PostedTransaction>()} WHERE " +
                         $"[{nameof(PostedTransaction.FiscalYearId)}] = '{year.Id}' AND " +
                         $"([{nameof(PostedTransaction.PostedTransactionType)}] = {(int)PostedTransactionType.CloseFrom} OR " +
                         $"[{nameof(PostedTransaction.PostedTransactionType)}] = {(int)PostedTransactionType.CloseTo})";

            repo.Utilities.ExecuteSQL(sql);

            // Closing from:

            // Iterate through each revenue & expense account in the fiscal year
            using (var batch = new BatchUnitOfWork<Account>(p => p.ClosingAccount, p => p.FiscalYear, p => p.LedgerAccountYears)
                .Where(p => p.FiscalYearId == year.Id && (p.Category == AccountCategory.Revenue || p.Category == AccountCategory.Expense))
                .AutoSaveChanges())
            {
                long transactionNumber = repo.Utilities.GetNextSequenceValue(DatabaseSequence.TransactionNumber);
                int lineNumber = 0;

                foreach (Account account in batch)
                {
                    // Calculate activity balance
                    var balances = batch.UnitOfWork.Where<AccountBalance>(p => p.Id == account.Id);

                    decimal activityBalance = balances.Where(p => p.DebitCredit == "DB").Sum(p => (decimal?)p.TotalAmount) -
                                              balances.Where(p => p.DebitCredit == "CR").Sum(p => (decimal?)p.TotalAmount) ?? 0m;

                    // If zero, no further action is taken.
                    if (activityBalance == 0m)
                    {
                        continue;
                    }

                    AccountLogic accountLogic = batch.UnitOfWork.GetBusinessLogic<AccountLogic>();

                    // Determine the closing account.
                    Account closingAccount = accountLogic.GetClosingAccount(account);

                    if (closingAccount == null)
                    {
                        continue;
                    }

                    // Create the "Close From" transaction
                    PostedTransaction tran = new PostedTransaction();
                    tran.FiscalYearId = year.Id;
                    tran.PostedTransactionType = PostedTransactionType.CloseFrom;
                    tran.LedgerAccountYearId = accountLogic.GetLedgerAccountYear(account)?.Id;
                    tran.TransactionDate = year.EndDate;
                    tran.Amount = -activityBalance;
                    tran.PeriodNumber = finalPeriodNumber;
                    tran.TransactionType = TransactionType.ClosingBalance;
                    tran.TransactionNumber = transactionNumber;
                    tran.LineNumber = ++lineNumber;
                    batch.UnitOfWork.Insert(tran);

                    CloseTo info = closeDict.GetValueOrDefault(closingAccount);
                    if (info == null)
                    {
                        info = new CloseTo();
                        info.Id = accountLogic.GetLedgerAccountYear(closingAccount)?.Id ?? Guid.Empty;
                        closeDict.Add(closingAccount, info);
                    }
                    if (activityBalance >= 0m)
                    {
                        info.DebitAmount += activityBalance;
                    }
                    else
                    {
                        info.CreditAmount += activityBalance;
                    }
                }
            }

            // Closing To:
            using (var batch = new BatchUnitOfWork<CloseTo>(closeDict.Values).Where(p => p.DebitAmount != 0m || p.CreditAmount != 0m).AutoSaveChanges())
            {
                long transactionNumber = repo.Utilities.GetNextSequenceValue(DatabaseSequence.TransactionNumber);
                int lineNumber = 0;

                foreach (var entry in batch)
                {
                    // Separate transactions for debit amounts vs. credit amounts.
                    if (entry.DebitAmount != 0m)
                    {
                        PostedTransaction tran = new PostedTransaction();
                        tran.FiscalYearId = year.Id;
                        tran.PostedTransactionType = PostedTransactionType.CloseTo;
                        tran.LedgerAccountYearId = entry.Id;
                        tran.TransactionDate = year.EndDate;
                        tran.Amount = entry.DebitAmount;
                        tran.PeriodNumber = finalPeriodNumber;
                        tran.TransactionType = TransactionType.ClosingBalance;
                        tran.TransactionNumber = transactionNumber;
                        tran.LineNumber = ++lineNumber;
                        batch.UnitOfWork.Insert(tran);
                    }
                    if (entry.CreditAmount != 0m)
                    {
                        PostedTransaction tran = new PostedTransaction();
                        tran.FiscalYearId = year.Id;
                        tran.PostedTransactionType = PostedTransactionType.CloseTo;
                        tran.LedgerAccountYearId = entry.Id;
                        tran.TransactionDate = year.EndDate;
                        tran.Amount = entry.CreditAmount;
                        tran.PeriodNumber = finalPeriodNumber;
                        tran.TransactionType = TransactionType.ClosingBalance;
                        tran.TransactionNumber = transactionNumber;
                        tran.LineNumber = ++lineNumber;
                        batch.UnitOfWork.Insert(tran);
                    }
                }
            }

            // Ending and beginning balances
            if (nextYear != null)
            {
                using (var batch = new BatchUnitOfWork<Account>(p => p.LedgerAccountYears)
                    .Where(p => p.FiscalYearId == year.Id)
                    .AutoSaveChanges())
                {
                    long transactionNumberEnd = repo.Utilities.GetNextSequenceValue(DatabaseSequence.TransactionNumber);
                    long transactionNumberBegin = repo.Utilities.GetNextSequenceValue(DatabaseSequence.TransactionNumber);
                    int lineNumberEnd = 0;
                    int lineNumberBegin = 0;

                    foreach (var account in batch)
                    {
                        // Calculate the final balance, including all closing and ending balance posted transactions.
                        decimal finalBalance = account.BeginningBalance;
                        foreach (var entry in account.LedgerAccountYears)
                        {
                            finalBalance += batch.UnitOfWork.Where<PostedTransaction>(p => p.LedgerAccountYearId == entry.Id && p.PostedTransactionType != PostedTransactionType.BeginBal)
                                                      .Sum(p => (decimal?)p.Amount) 
                                                      ?? 0m;
                        }

                        if (finalBalance == 0m)
                        {
                            continue;
                        }

                        AccountLogic accountLogic = batch.UnitOfWork.GetBusinessLogic<AccountLogic>();

                        // Determine which GL account receives the beginning balance.
                        Account toAccount;
                        if (account.Category == AccountCategory.Asset || account.Category == AccountCategory.Liability || account.Category == AccountCategory.Fund)
                        {
                            toAccount = accountLogic.GetClosingAccount(account) ?? account;
                        }
                        else
                        {
                            toAccount = account;
                        }

                        // Determine the next year accounts that will receive the beginning balance.
                        var nextAccounts = accountLogic.GetNextYearAccounts(toAccount, nextYear);
                        if (nextAccounts.Count > 0)
                        {
                            // Create the "EndBal" transaction for the current year account (being closed)
                            PostedTransaction tran = new PostedTransaction();
                            tran.FiscalYearId = year.Id;
                            tran.PostedTransactionType = PostedTransactionType.EndBal;
                            tran.LedgerAccountYearId = accountLogic.GetLedgerAccountYear(toAccount)?.Id;
                            tran.TransactionDate = year.EndDate;
                            tran.Amount = -finalBalance;
                            tran.PeriodNumber = finalPeriodNumber;
                            tran.TransactionType = TransactionType.BeginningBalance;
                            tran.TransactionNumber = transactionNumberEnd;
                            tran.LineNumber = ++lineNumberEnd;
                            batch.UnitOfWork.Insert(tran);

                            // Create the "BeginBal" transaction(s) for the next year account(s)
                            decimal rem = 0m; // Roundoff error
                            foreach (var entry in nextAccounts)
                            {
                                decimal amt = (finalBalance * entry.Factor) + rem; // Unrounded amount

                                tran = new PostedTransaction();
                                tran.FiscalYearId = nextYear.Id;
                                tran.PostedTransactionType = PostedTransactionType.BeginBal;
                                tran.LedgerAccountYearId = accountLogic.GetLedgerAccountYear(entry.Account)?.Id;
                                tran.TransactionDate = nextYear.StartDate;
                                tran.Amount = Math.Round(amt, 2);
                                tran.PeriodNumber = 1;
                                tran.TransactionType = TransactionType.BeginningBalance;
                                tran.TransactionNumber = transactionNumberBegin;
                                tran.LineNumber = ++lineNumberBegin;
                                batch.UnitOfWork.Insert(tran);

                                rem += amt - tran.Amount; // Accumulate roundoff error
                                entry.Account.BeginningBalance += tran.Amount;
                            }
                        }                        
                    } // Each account in batch
                } // Using batch
            } // Ending and beginning balances
        } // PerformYearClose()

        #endregion

        #region Nested Classes

        private class CloseTo : IEntity
        {
            public Guid Id { get; set; }
            public decimal DebitAmount { get; set; }
            public decimal CreditAmount { get; set; }

            public string DisplayName => string.Empty;
            [NotMapped]
            public Byte[] RowVersion { get; set; }
            public void AssignPrimaryKey() { }            
        }

        #endregion

    }
}
