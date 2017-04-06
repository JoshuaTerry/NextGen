using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DDI.Business.Helpers;
using DDI.Data;
using DDI.Data.Helpers;
using DDI.Logger;
using DDI.Shared;
using DDI.Shared.Caching;
using DDI.Shared.Enums.GL;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics.GL;

namespace DDI.Business.GL
{
    public class AccountLogic : EntityLogicBase<Account>
    {
        public const string BusinessUnitSeparator = ":";

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
                    return ledger.Code + BusinessUnitSeparator + account.AccountNumber;
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
            return ledger.Code + BusinessUnitSeparator + account.AccountNumber;            
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
            return ledger.Code + BusinessUnitSeparator + account.AccountNumber;
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

        /// <summary>
        /// Validate a G/L account number.
        /// </summary>
        /// <param name="ledgerObject">A ledger or fiscal year.</param>
        /// <param name="accountNumber">G/L account as a string value.</param>
        /// <param name="allowBusinessUnitOverride">True to allow XXXX: business unit prefix.</param>
        /// <param name="allowNewSegments">True to allow for new G/L account segments</param>
        /// <param name="validateAccount">True to throw an exception of G/L account doesn't exist.</param>
        public ValidatedAccount ValidateAccountNumber(IEntity ledgerObject, string accountNumber, bool allowBusinessUnitOverride = true, bool allowNewSegments = false, bool validateAccount = true)
        {
            ValidatedAccount result = new ValidatedAccount();
            Ledger ledger = null;
            FiscalYear fiscalYear = null;
            LedgerLogic _ledgerLogic = UnitOfWork.GetBusinessLogic<LedgerLogic>();
            BusinessUnitLogic _businessUnitLogic = UnitOfWork.GetBusinessLogic<BusinessUnitLogic>();

            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                return result;
            }

            if (ledgerObject == null)
            {
                BusinessUnit unit = _businessUnitLogic.GetDefaultBusinessUnit();
                if (unit != null)
                {
                    ledger = _ledgerLogic.GetCurrentLedger(unit.Id);
                }
            }
            else if (ledgerObject is Ledger)
            {
                ledger = (Ledger)ledgerObject;
            }
            else if (ledgerObject is FiscalYear)
            {
                fiscalYear = (FiscalYear)ledgerObject;
                ledger = UnitOfWork.GetReference(fiscalYear, p => p.Ledger);
            }

            if (ledger == null)
            {
                throw new InvalidOperationException("Cannot determine ledger.");
            }

            SegmentLevel[] segmentInfo = _ledgerLogic.GetSegmentLevels(ledger);

            // Handle explicit business unit separator.
            if (_businessUnitLogic.IsMultiple)
            {
                int index = accountNumber.IndexOf(BusinessUnitSeparator[0]);
                if (index > 0)
                {
                    // Extract the business unit code from the front of the account number.
                    string unitCode = accountNumber.Substring(0, index);
                    accountNumber = accountNumber.Substring(index + 1);

                    // Look up the explicit business unit 
                    result.ExplicitBusinesUnit = _ledgerLogic.LedgerCache.Entities.FirstOrDefault(p => p.BusinessUnit.Code == unitCode)?.BusinessUnit;
                    if (result.ExplicitBusinesUnit == null)
                    {
                        throw new ValidationException(string.Format(UserMessagesGL.BadBusinessUnitCode, "Business unit", unitCode));
                    }

                    if (!allowBusinessUnitOverride && result.ExplicitBusinesUnit.Id != ledger.BusinessUnitId)
                    {
                        throw new ValidationException(string.Format(UserMessagesGL.AccountMustBeInBusinessUnit, "Business unit", UnitOfWork.GetReference(ledger, p => p.BusinessUnit).Code));
                    }

                    if (fiscalYear != null)
                    {
                        // Find the same fiscal year in the explicit business unit.
                        FiscalYear otherYear = UnitOfWork.GetBusinessLogic<FiscalYearLogic>().GetFiscalYearForBusinessUnit(fiscalYear, result.ExplicitBusinesUnit);
                        if (otherYear == null)
                        {
                            throw new ValidationException(string.Format(UserMessagesGL.BadFiscalYearForBusinessUnit, fiscalYear.Name, "Business unit", result.ExplicitBusinesUnit.Code));
                        }

                        fiscalYear = otherYear;
                    }
                }
            }

            // Break out accountNumber into segments and populate result.SegmentCodes
            bool separators = true;
            for (int segmentNumber = 0; segmentNumber < segmentInfo.Length; )
            {
                int length = accountNumber.Length;
                int position = 0;
                string code = string.Empty;

                if (separators && segmentInfo[segmentNumber].Separator.Length > 0)
                {
                    position = accountNumber.IndexOf(segmentInfo[segmentNumber].Separator) + 1;
                    if (position == 0)
                    {
                        position = length + 1;
                    }
                }
                else
                {
                    if (segmentInfo[segmentNumber].Length > 0)
                    {
                        position = segmentInfo[segmentNumber].Length;
                    }
                    else
                    {
                        position = length;
                    }
                }

                if (segmentNumber == 0 && separators && position > length)
                {
                    separators = false;
                    continue;
                }

                if (separators && segmentInfo[segmentNumber].Separator.Length > 0)
                {
                    if (position > length)
                    {
                        code = accountNumber;
                        accountNumber = string.Empty;
                    }
                    else
                    {
                        code = accountNumber.Substring(0, position - 1);
                        accountNumber = accountNumber.Substring(position);
                    }
                }
                else
                {
                    if (position > length)
                    {
                        position = length;
                    }
                    code = accountNumber.Substring(0, position);
                    accountNumber = accountNumber.Substring(position);
                }

                if (string.IsNullOrWhiteSpace(code))
                {
                    break;
                }

                result.SegmentCodes.Add(code);
                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    break;
                }

                segmentNumber++;
            }

            Segment parentSegment = null;
            // Validate individual segments
            for (int segmentNumber = 0; segmentNumber < result.SegmentCodes.Count; segmentNumber++)
            {
                string code = result.SegmentCodes[segmentNumber];

                // Validate format
                if (segmentInfo[segmentNumber].Format == SegmentFormat.Numeric && Regex.IsMatch(code, "[^0-9]"))
                {
                    throw new ValidationException(UserMessagesGL.GLSegmentNumeric);
                }
                else if (segmentInfo[segmentNumber].Format == SegmentFormat.Alpha && Regex.IsMatch(code, "[^A-Z]"))
                {
                    throw new ValidationException(UserMessagesGL.GLSegmentAlpha);
                }
                else if (Regex.IsMatch(code, "[^A-Z0-9]"))
                {
                    throw new ValidationException(UserMessagesGL.GLSegmentAlphaNumeric);
                }

                // Validate length
                if (segmentInfo[segmentNumber].Length > 0 && code.Length != segmentInfo[segmentNumber].Length)
                {
                    throw new ValidationException(string.Format(UserMessagesGL.GLSegmentLength, segmentInfo[segmentNumber].Length));
                }

                // Validate segment in db

                if (fiscalYear != null)
                {
                    Segment segment = null;

                    if (segmentInfo[segmentNumber].IsLinked)
                    {
                        if (parentSegment != null)
                        {
                            segment = parentSegment.ChildSegments?.FirstOrDefault(p => p.Code == code)
                                      ??
                                      UnitOfWork.FirstOrDefault<Segment>(p => p.ParentSegmentId == parentSegment.Id && p.Code == code);
                        }
                    }
                    else
                    {
                        parentSegment = null;
                        segment = UnitOfWork.GetLocal<Segment>().FirstOrDefault(p => p.Level == segmentNumber + 1 && p.FiscalYearId == fiscalYear.Id && p.Code == code)
                                  ??
                                  UnitOfWork.FirstOrDefault<Segment>(p => p.Level == segmentNumber + 1 && p.FiscalYearId == fiscalYear.Id && p.Code == code);
                    }

                    if (segment == null && !allowNewSegments)
                    {
                        throw new ValidationException(string.Format(UserMessagesGL.InvalidCode, segmentInfo[segmentNumber].Name, code));
                    }

                    result.Segments.Add(segment);
                    parentSegment = segment;
                }
            }

            // Reformat the G/L account
            StringBuilder sb = new StringBuilder();

            for (int index = 0; index < result.SegmentCodes.Count; index++)
            {
                if (index > 0)
                {
                    sb.Append(segmentInfo[index].Separator);
                }
                sb.Append(result.SegmentCodes[index]);
            }

            accountNumber = sb.ToString();

            if (result.ExplicitBusinesUnit != null)
            {
                sb.Insert(0, result.ExplicitBusinesUnit.Code + BusinessUnitSeparator);
            }

            result.AccountNumber = sb.ToString();

            if (fiscalYear != null)
            {
                // Try to find the account in the fiscal year.
                result.Account = UnitOfWork.FirstOrDefault<Account>(p => p.AccountNumber == accountNumber && p.FiscalYearId == fiscalYear.Id);
                if (validateAccount && result.Account == null)
                {
                    throw new ValidationException(string.Format(UserMessagesGL.GLAccountNumberInvalid, accountNumber));
                }
                result.LedgerAccount = GetLedgerAccount(result.Account);
            }
            else if (ledger != null)
            {
                // No fiscal year, but try to find the LedgerAccount for this account number.
                result.LedgerAccount = UnitOfWork.FirstOrDefault<LedgerAccount>(p => p.AccountNumber == accountNumber);
                if (validateAccount && result.LedgerAccount == null)
                {
                    throw new ValidationException(string.Format(UserMessagesGL.GLAccountNumberInvalid, accountNumber));
                }
            }

            return result;

        }


    }
}
