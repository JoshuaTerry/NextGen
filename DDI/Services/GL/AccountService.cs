using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Business.GL;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics.GL;
using Newtonsoft.Json.Linq;

namespace DDI.Services.GL
{
    public class AccountService : ServiceBase<Account>, IAccountService
    {
        private AccountLogic _accountLogic = null;

        public AccountService(IUnitOfWork uow, AccountLogic accountLogic) : base(uow)
        {
            _accountLogic = accountLogic;
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

        protected override Action<Account,string> FormatEntityForGet => (account, fields) => PopulateAccountBalanceIds(account);

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
            if (account.AccountSegments != null)
            {
                account.AccountSegments = account.AccountSegments.OrderBy(p => p.Level).ToList();
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
                    // Ensure segment exists
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

            // Handle LedgerAccountYears collection.

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


        protected override bool ProcessJTokenUpdate(IEntity entity, string name, JToken token)
        {
            if (name == nameof(Account.AccountSegments) && entity is Account)
            {
                var account = (Account)entity;
                if (account.AccountSegments == null)
                {
                    UnitOfWork.LoadReference(account, p => p.AccountSegments);
                }

                foreach (var entry in account.AccountSegments.ToList())
                {
                    UnitOfWork.Delete(entry);
                }

                account.AccountSegments.Clear();

                AddUpdateFromJArray(account.AccountSegments, token as JArray);

                foreach (var entry in account.AccountSegments)
                {
                    // Ensure segment exists
                    Segment segment = entry.Segment;
                    if (segment == null && entry.SegmentId != null)
                    {
                        segment = UnitOfWork.GetById<Segment>(entry.SegmentId.Value);
                    }
                    else
                    {
                        segment = UnitOfWork.GetById<Segment>(entry.Segment.Id);
                    }
                    if (segment == null)
                    {
                        throw new ValidationException(UserMessagesGL.GLAccountSegmentInvalid);
                    }
                    entry.Segment = segment;
                    entry.SegmentId = segment.Id;
                    entry.Level = segment.Level;
                    entry.Account = account;
                }

                account.AccountNumber = _accountLogic.CalculateAccountNumber(account);
                account.SortKey = _accountLogic.CalculateSortKey(account);

                return true;
            }

            return false;
        }



    }
}
