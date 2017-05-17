using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Business.GL;
using DDI.Data;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics.GL;

namespace DDI.Services.GL
{
    public class AccountService : ServiceBase<Account>, IAccountService
    {
        private AccountLogic _accountLogic = null;

        public AccountService()
            : this(new UnitOfWorkEF())
        {
        }

        public AccountService(IUnitOfWork uow) : base(uow)
        {
            _accountLogic = uow.GetBusinessLogic<AccountLogic>();
        }


        public IDataResponse<AccountActivitySummary> GetAccountActivity(Guid accountId)
        {
            Account account = UnitOfWork.GetById<Account>(accountId, p => p.FiscalYear.Ledger, p => p.Budgets);

            var activity = _accountLogic.GetAccountActivity(account);
            return new DataResponse<AccountActivitySummary>(activity);
        }

        public IDataResponse<List<AccountActivityDetail>> GetAccountActivityDetail(Guid accountId)
        {
            Account account = UnitOfWork.GetById<Account>(accountId, p => p.FiscalYear.Ledger, p => p.Budgets);

            var activity = _accountLogic.GetAccountActivity(account);
            List<AccountActivityDetail> activityDetailList = activity.Detail.ToList();

            return new DataResponse<List<AccountActivityDetail>>(activityDetailList);
        }

        protected override Action<Account> FormatEntityForGet => account => PopulateAccountBalanceIds(account);

        /// <summary>
        /// The Ids coming back from the SQL view put the account ID into AccountBalance.Id.  This confuses the dynamic transmogrifier, 
        /// so we need to store actual Guids instead.  These are never used, since the rows in AccountBalance can't be updated or referenced individually.
        /// </summary>
        private void PopulateAccountBalanceIds(Account account)
        {
            if (account.AccountBalances != null)
            {
                foreach (var entry in account.AccountBalances)
                {
                    entry.Id = GuidHelper.NewSequentialGuid();
                }
            }
        }

        public override IDataResponse<Account> Add(Account account)
        {
            account.AssignPrimaryKey();

            // Handle account segments
            if (account.AccountSegments != null)
            {
                foreach (var entry in account.AccountSegments)
                {
                    Segment segment = null;
                    if (entry.SegmentId != null)
                    {
                        segment = UnitOfWork.GetById<Segment>(entry.SegmentId.Value);
                    }
                    if (segment == null)
                    {
                        return GetErrorResponse<Account>(UserMessagesGL.GLAccountSegmentInvalid);
                    }

                    entry.Level = segment.Level;
                    UnitOfWork.Insert(entry);
                    entry.Account = account;
                }
            }
            else
            {
                return GetErrorResponse<Account>(UserMessagesGL.GLAccountNoSegments);
            }

            LedgerAccount ledgerAccount = null;

            if (account.LedgerAccountYears == null)
            {
                account.LedgerAccountYears = new List<LedgerAccountYear>();
            }
            if (account.LedgerAccountYears.Count == 0)
            {
                // For new accounts, we need a LedgerAccountYear
                Guid? ledgerAccountId = null;
                LedgerAccountYear ledgerAccountYear = null;
                FiscalYear year = account.FiscalYear;
                if (year == null && account.FiscalYearId != null)
                {
                    year = UnitOfWork.GetById<FiscalYear>(account.FiscalYearId.Value);
                    if (year == null)
                    {
                        return GetErrorResponse<Account>(UserMessagesGL.BadFiscalYear);
                    }
                }
                FiscalYear priorYear = UnitOfWork.GetBusinessLogic<FiscalYearLogic>().GetPriorFiscalYear(year);
                if (priorYear != null)
                {
                    // Look in the prior year for an account with the same acocunt number.  
                    ledgerAccountYear = UnitOfWork.FirstOrDefault<LedgerAccountYear>(p => p.FiscalYearId == priorYear.Id && p.Account.AccountNumber == account.AccountNumber);
                    if (ledgerAccountYear != null)
                    {
                        ledgerAccountId = ledgerAccountYear.LedgerAccountId;
                    }
                }

                if (ledgerAccountId == null)
                {
                    // Need to create a new LedgerAccount
                    ledgerAccount = new LedgerAccount();
                    ledgerAccount.LedgerId = year.LedgerId;
                    ledgerAccount.AssignPrimaryKey();
                    ledgerAccountId = ledgerAccount.Id;
                    UnitOfWork.Insert(ledgerAccount);
                }

                // Create new LedgerAccountYear
                ledgerAccountYear = new LedgerAccountYear();
                ledgerAccountYear.LedgerAccountId = ledgerAccountId;
                ledgerAccountYear.FiscalYearId = year.Id;
                ledgerAccountYear.AccountId = account.Id;
                UnitOfWork.Insert(ledgerAccountYear);
            }
            
            account.AccountNumber = _accountLogic.CalculateAccountNumber(account);
            account.SortKey = _accountLogic.CalculateSortKey(account);

            if (ledgerAccount != null)
            {
                // Creating a ledger account: Ensure the account number and description match the account.
                ledgerAccount.AccountNumber = account.AccountNumber;
                ledgerAccount.Name = account.Name;
            }
            
            return base.Add(account);
        }

    }
}
