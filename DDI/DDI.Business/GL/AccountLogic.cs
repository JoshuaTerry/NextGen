using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DDI.Business.Helpers;
using DDI.Data;
using DDI.Logger;
using DDI.Shared;
using DDI.Shared.Caching;
using DDI.Shared.Enums.GL;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics.GL;

namespace DDI.Business.GL
{
    public class AccountLogic : EntityLogicBase<Account>
    {
        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(AccountLogic));
        public AccountLogic() : this(new UnitOfWorkEF()) { }

        public AccountLogic(IUnitOfWork unitOfWork) : base(unitOfWork)
        {            
        }

        public override void Validate(Account entity)
        {
            if (string.IsNullOrWhiteSpace(entity.AccountNumber))
            {
                throw new ValidationException(UserMessagesGL.GLAcctNumBlank);
            }

            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                throw new ValidationException(UserMessagesGL.GLAcctDescrBlank);
            }

            if (entity.Category == AccountCategory.None)
            {
                throw new ValidationException(UserMessagesGL.GLAcctCategoryNone);
            }

            if (entity.BeginningBalance != 0m && (entity.Category == AccountCategory.Revenue || entity.Category == AccountCategory.Expense))
            {
                throw new ValidationException(UserMessagesGL.GLAcctBeginBalRandE);
            }

            //Logic to check for changing the account number or name (description).
            var repository = UnitOfWork.GetRepository<Account>();
            if (repository.GetEntityState(entity) == EntityState.Modified)
            {
                List<string> modifiedProperties = repository.GetModifiedProperties(entity);
                if (modifiedProperties.Contains(nameof(Account.AccountNumber)) ||
                    modifiedProperties.Contains(nameof(Account.Name)))
                {

                    LedgerAccount ledgerAccount = GetLedgerAccount(entity);
                    if (ledgerAccount.AccountNumber != entity.AccountNumber || 
                        ledgerAccount.Name != entity.Name)
                    {
                        DateTime? yearStartDt = UnitOfWork.GetReference(entity, p => p.FiscalYear).StartDate;
                        if (yearStartDt.HasValue)
                        {
                            // Does the ledger account have any fiscal years that start after this date?
                            DateTime? maxYearStartDt = UnitOfWork.Where<LedgerAccountYear>(p => p.LedgerAccountId == ledgerAccount.Id && p.FiscalYear.StartDate != null)
                                                                 .OrderByDescending(p => p.FiscalYear.StartDate)
                                                                 .Select(p => p.FiscalYear.StartDate)
                                                                 .FirstOrDefault();
                            if (yearStartDt == maxYearStartDt)
                            {
                                // If so, update the ledger account.
                                ledgerAccount.Name = entity.Name;
                                ledgerAccount.AccountNumber = entity.AccountNumber;
                            }

                        }
                    }
                }
            }

        }

        /// <summary>
        /// Account number prefixed by business unit code.
        /// </summary>
        public string GetPrefixedAccountNumber(Account account)
        {
            if (account == null)
            {
                return string.Empty;
            }

            if (BusinessUnitHelper.IsMultiple)
            {
                Ledger ledger = GetLedgerForAccount(account);
                if (ledger != null)
                {
                    return ledger.Code + ":" + account.AccountNumber;
                }
            }
            return account.AccountNumber;                
        }
        
        /// <summary>
        /// Return the account number.  If a ledger Id or business unit Id can be specified and the account belongs to another ledger/business unit, the 
        /// account number will be prefixed by the account's business unit code.
        /// </summary>
        public string GetPrefixedAccountNumber(Account account, Guid? defaultId)
        {
            if (account == null)
            {
                return string.Empty;
            }

            if (defaultId == null)
            {
                return account.AccountNumber;
            }

            Ledger ledger = GetLedgerForAccount(account);

            if (ledger == null || defaultId == ledger.Id || defaultId == ledger.BusinessUnitId)
            {
                return account.AccountNumber;
            }
            return ledger.Code + ":" + account.AccountNumber;            
        }

        /// <summary>
        /// Return the account number.  If a ledger Id or business unit Id can be specified and the account belongs to another ledger/business unit, the 
        /// account number will be prefixed by the account's business unit code.
        /// </summary>
        public string GetPrefixedAccountNumber(LedgerAccount account, Guid? defaultId)
        {
            if (account == null)
            {
                return string.Empty;
            }

            if (defaultId == null)
            {
                return account.AccountNumber;
            }

            Ledger ledger = UnitOfWork.GetReference(account, p => p.Ledger);

            if (defaultId == ledger.Id || defaultId == ledger.BusinessUnitId)
            {
                return account.AccountNumber;
            }
            return ledger.Code + ":" + account.AccountNumber;
        }

        private Ledger GetLedgerForAccount(Account account)
        {
            Ledger ledger = account.FiscalYear?.Ledger;
            if (ledger == null)
            {
                var year = UnitOfWork.GetReference(account, p => p.FiscalYear);
                if (year != null)
                {
                    ledger = UnitOfWork.GetReference(year, p => p.Ledger);
                }
            }
            return ledger;
        }

        /// <summary>
        /// Calcualte an account number for an Account.
        /// </summary>
        public string CalculateAccountNumber(Account account)
        {
            if (account == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            var ledgerLogic = UnitOfWork.GetBusinessLogic<LedgerLogic>();

            // Ensure account's referenced entities are fully populated.
            if (account.AccountSegments == null || account.FiscalYear == null || account.AccountSegments.FirstOrDefault()?.Segment == null)
            {
                account = UnitOfWork.GetEntities<Account>(p => p.AccountSegments.First().Segment, p => p.FiscalYear).FirstOrDefault(p => p.Id == account.Id);
            }

            var segmentLevels = ledgerLogic.GetSegmentLevels(account.FiscalYear.LedgerId.Value);

            foreach (var segment in account.AccountSegments.OrderBy(p => p.Level).Select(p => p.Segment))
            {
                if (sb.Length > 0 && !string.IsNullOrWhiteSpace(segment.Code))
                {
                    sb.Append(segmentLevels[segment.Level - 2].Separator);                    
                }
                sb.Append(segment.Code);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Calcualte an account number for a list of Segment entities.
        /// </summary>
        public string CalculateAccountNumber(Ledger ledger, IList<Segment> segments)
        {
            if (segments == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();

            var ledgerLogic = UnitOfWork.GetBusinessLogic<LedgerLogic>();


            var segmentLevels = ledgerLogic.GetSegmentLevels(ledger);

            int thisLevel = 0;
            foreach (var segment in segments.OrderBy(p => p.Level))
            {
                // Create placeholders (e.g. ???) for missing segments.  (Trailing blank segments have already been removed from the enumeration.)
                while (++thisLevel < segment.Level)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(segmentLevels[thisLevel - 2].Separator);
                    }
                    sb.Append(new string('?', segmentLevels[thisLevel].Length));
                }

                if (sb.Length > 0 && !string.IsNullOrWhiteSpace(segment.Code))
                {
                    sb.Append(segmentLevels[segment.Level - 2].Separator);
                }
                sb.Append(segment.Code);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Calculate the account sorting key from the segments.
        /// </summary>
        public string CalculateSortKey(Account account)
        {

            var ledgerLogic = UnitOfWork.GetBusinessLogic<LedgerLogic>();

            // Ensure account's referenced entities are fully populated.
            if (account.AccountSegments == null || account.FiscalYear == null || account.AccountSegments.FirstOrDefault()?.Segment == null)
            {
                account = UnitOfWork.GetEntities<Account>(p => p.AccountSegments.First().Segment, p => p.FiscalYear).FirstOrDefault(p => p.Id == account.Id);
            }

            var segmentLevels = ledgerLogic.GetSegmentLevels(account.FiscalYear.LedgerId.Value);

            var tmp = segmentLevels.FirstOrDefault(p => p.Level == 2);
            tmp.SortOrder = 1;

            // Array to hold keys.
            string[] keys = new string[segmentLevels.Length - 1];

            // Initialize it to empty strings.
            for (int idx = 0; idx < keys.Length; idx++)
            {
                keys[idx] = string.Empty;
            }

            
            foreach (var entry in segmentLevels.OrderBy(p => p.SortOrder > 0 ? p.SortOrder : 100 + p.Level) // Order the segments by sort order, or if zero by their level - although sort order takes precedence.
                                               .Zip(Enumerable.Range(0, keys.Length), 
                                                    (segmentLevel, n) => new { Index = n, Level = segmentLevel.Level }) // Assign a numeric order (0 thru n - 1) to the levels (1 - n)
                                               .Join(account.AccountSegments, outer => outer.Level, 
                                                     accountSegment => accountSegment.Level, 
                                                     (outer, accountSegment) => new { Index = outer.Index, Segment = accountSegment.Segment})) // Join with account.AccountSegments
            {
                keys[entry.Index] = entry.Segment?.Code ?? string.Empty;  // Assign each segment code to keys.
            }

            // Return the sort key.
            return string.Join(" ", keys);
        }

        /// <summary>
        /// Get the default ledger account for an account.
        /// </summary>
        public LedgerAccount GetLedgerAccount(Account account)
        {
            var ledgerAccountYear = UnitOfWork.GetReference(account, p => p.LedgerAccountYears)?.FirstOrDefault();
            if (ledgerAccountYear != null)
            {
                return UnitOfWork.GetReference(ledgerAccountYear, p => p.LedgerAccount);
            }
            return null;
        }

        /// <summary>
        /// Get a specific account for specfied fiscal year.
        /// </summary>
        /// <param name="ledgerAccount">Ledger account</param>
        /// <param name="year">Fiscal year</param>
        /// <param name="anyEntity">TRUE if fiscal year can be in another ledger or business unit.</param>
        public Account GetAccount(LedgerAccount ledgerAccount, FiscalYear year, bool anyLedger = false)
        {
            if (ledgerAccount == null || year == null)
            {
                return null;
            }
            if (ledgerAccount.LedgerAccountYears == null || ledgerAccount.LedgerAccountYears.FirstOrDefault()?.Account == null)
            {
                // Ensure all necessary navigation properties are loaded.
                ledgerAccount = UnitOfWork.GetById<LedgerAccount>(ledgerAccount.Id, p => p.LedgerAccountYears, p => p.LedgerAccountYears.First().Account);
            }
            if (!anyLedger || ledgerAccount.LedgerId == year.LedgerId)
            {
                return ledgerAccount.LedgerAccountYears.FirstOrDefault(p => p.FiscalYearId == year.Id)?.Account;
            }
            
            // Ledgers are different.  Get fiscal year for the other ledger's business unit, then return the acocunt for that year.
            Ledger ledger = UnitOfWork.GetReference(ledgerAccount, p => p.Ledger);
            year = UnitOfWork.GetBusinessLogic<FiscalYearLogic>().GetFiscalYearForBusinessUnit(year, ledger.BusinessUnitId.Value);
            return GetAccount(ledgerAccount, year);
        }

        /// <summary>
        /// Get a specific account for the fiscal year containing the specified date.
        /// </summary>
        public Account GetAccount(LedgerAccount ledgerAccount, DateTime tranDt)
        {
            if (ledgerAccount == null)
            {
                return null;
            }
            if (ledgerAccount.LedgerAccountYears == null || 
                ledgerAccount.LedgerAccountYears.FirstOrDefault()?.Account == null || 
                ledgerAccount.LedgerAccountYears.FirstOrDefault()?.FiscalYear == null)
            {
                // Ensure all necessary navigation properties are loaded.
                ledgerAccount = UnitOfWork.GetById<LedgerAccount>(ledgerAccount.Id, p => p.LedgerAccountYears, p => p.LedgerAccountYears.First().Account, p => p.LedgerAccountYears.First().FiscalYear);
            }
            return ledgerAccount.LedgerAccountYears.FirstOrDefault(p => p.FiscalYear.StartDate <= tranDt && p.FiscalYear.EndDate >= tranDt)?.Account;
        }


        public string GetAccountNumber(LedgerAccount ledgerAccount, FiscalYear year)
        {
            if (ledgerAccount == null)
            {
                return string.Empty;
            }

            if (year != null)
            {
                Account account = GetAccount(ledgerAccount, year, true);
                if (account != null)
                {
                    return GetPrefixedAccountNumber(account, year.LedgerId);
                }
            }

            return ledgerAccount.AccountNumber;
        }


        public string GetAccountDescription(LedgerAccount ledgerAccount, FiscalYear year)
        {
            if (ledgerAccount == null)
            {
                return string.Empty;
            }

            if (year != null)
            {
                Account account = GetAccount(ledgerAccount, year, true);
                if (account != null)
                {
                    return account.Name;
                }
            }

            return ledgerAccount.Name;
        }


    }
}
