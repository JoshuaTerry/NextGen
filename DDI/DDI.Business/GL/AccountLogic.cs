using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DDI.Business.Helpers;
using DDI.Logger;
using DDI.Shared;
using DDI.Shared.Enums.GL;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using DDI.Shared.Statics.GL;
using System.Reflection;

namespace DDI.Business.GL
{
    public class AccountLogic : EntityLogicBase<Account>
    {
        #region Fields

        public const string BusinessUnitSeparator = ":";

        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(AccountLogic));

        #endregion

        public AccountLogic(IUnitOfWork uow) : base(uow)
        {
        }

        #region Public Methods

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

            // Try to find the account in the fiscal year.
            Account result = new Account();
            result = UnitOfWork.FirstOrDefault<Account>(p => p.AccountNumber == entity.AccountNumber && p.FiscalYearId == entity.FiscalYearId);
            if (result != null)
            {
                if (result.Id != entity.Id)
                {
                    throw new ValidationException(UserMessagesGL.GLAcctDuplicate, entity.AccountNumber);
                }
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

        
        public IDataResponse<Account> MergeAccounts(Guid sourceAccountId, Guid destinationAccountId)
        {
            DataResponse<Account> response = new DataResponse<Account>(); ;

            try
            {
                var sourceAccount = UnitOfWork.GetById<Account>(sourceAccountId, p => p.FiscalYear.Ledger, p => p.Budgets);
                var destinationAccount = UnitOfWork.GetById<Account>(destinationAccountId, p => p.FiscalYear.Ledger, p => p.Budgets);

                response.Data = destinationAccount;

                if (sourceAccount.FiscalYearId != destinationAccount.FiscalYearId)
                    throw new InvalidOperationException("Source and Destination Account must have the same Fiscal Year.");

                // Update budgets
                MergeAccountBudgets(sourceAccount, destinationAccount);

                //Delete the Account Segments
                sourceAccount.AccountSegments.ToList().ForEach(s => UnitOfWork.Delete(s));

                //Create Merge Record
                CreateMergeRecord(sourceAccount, destinationAccount);

                //Delete Source Account
                UnitOfWork.Delete(sourceAccount);

                //Update Destination Account
                UnitOfWork.Update(destinationAccount);

                UnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                response.IsSuccessful = false;
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        //Merge Budgets for 2 different Accounts
        internal void MergeAccountBudgets(Account source, Account destination)
        {
            // Add the Beginning Account Balance
            destination.BeginningBalance += source.BeginningBalance;

            // Add the source Budget Amounts to Destination Budget Amounts
            var destinationBudgets = destination.Budgets.ToList();
            var sourceBudgets = source.Budgets.ToList();
            var amountProperties = typeof(PeriodAmountList).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.Name.StartsWith("Amount") && p.CanWrite).ToList();

            // Get each amount from the source PeriodAmountList and add them to the destination PeriodAmountList
            foreach (var budget in sourceBudgets)
            {
                var dbudget = destinationBudgets.FirstOrDefault(b => b.BudgetType == budget.BudgetType)?.Budget;
                amountProperties.ForEach(p => p.SetValue(dbudget, (decimal)p.GetValue(budget.Budget) + (decimal)p.GetValue(dbudget)));
               
                UnitOfWork.Update(dbudget);
            }
        }
       
        //Create a Merge Record for merging 2 accounts
        internal void CreateMergeRecord(Account source, Account destintation)
        {
            var merge = new LedgerAccountMerge();
            merge.FiscalYear = destintation.FiscalYear;
            merge.FromAccount = GetLedgerAccount(source);
            merge.ToAccount = GetLedgerAccount(destintation);
            UnitOfWork.Insert(merge);
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

            if (BusinessUnitHelper.GetIsMultiple(UnitOfWork))
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
            BusinessUnit unit = UnitOfWork.GetReference(ledger, p => p.BusinessUnit);
            return unit.Code + BusinessUnitSeparator + account.AccountNumber;            
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

            // Array to hold keys.
            string[] keys = new string[segmentLevels.Length];

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
            if (account == null)
            {
                return null;
            }

            LedgerAccountYear ledgerAccountYear = UnitOfWork.GetReference(account, p => p.LedgerAccountYears)?.FirstOrDefault();
            if (ledgerAccountYear != null)
            {
                return UnitOfWork.GetReference(ledgerAccountYear, p => p.LedgerAccount);
            }
            return null;
        }

        /// <summary>
        /// Get the default ledger account for an account.
        /// </summary>
        public LedgerAccount GetLedgerAccount(Guid? accountId)
        {
            if (accountId == null)
            {
                return null;
            }

            return GetLedgerAccount(UnitOfWork.GetById<Account>(accountId.Value, p => p.LedgerAccountYears.First().LedgerAccount));
        }

        /// <summary>
        /// Get a specific account for specfied fiscal year.
        /// </summary>
        /// <param name="ledgerAccount">Ledger account</param>
        /// <param name="year">Fiscal year</param>
        /// <param name="anyBusinessUnit">TRUE if fiscal year can be in another business unit.</param>
        public Account GetAccount(LedgerAccount ledgerAccount, FiscalYear year, bool anyBusinessUnit = false)
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
            if (!anyBusinessUnit || ledgerAccount.LedgerId == year.LedgerId)
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

        /// <summary>
        /// Get the default LedgerAccountYear for an account.
        /// </summary>
        public LedgerAccountYear GetLedgerAccountYear(Account account)
        {
            return UnitOfWork.GetReference(account, p => p.LedgerAccountYears)?.FirstOrDefault(p => p.IsMerge == false) ?? account.LedgerAccountYears.FirstOrDefault();
        }

        /// <summary>
        /// Get the default LedgerAccountYear for a LedgerAccount based on a fiscal year
        /// </summary>
        public LedgerAccountYear GetLedgerAccountYear(LedgerAccount account, Guid? yearId)
        {
            if (account == null || yearId == null)
            {
                return null;
            }

            if (account.LedgerAccountYears == null)
            {
                UnitOfWork.LoadReference(account, p => p.LedgerAccountYears);
            }

            return account.LedgerAccountYears.FirstOrDefault(p => p.FiscalYearId == yearId && p.IsMerge == false)
                    ??
                   account.LedgerAccountYears.FirstOrDefault(p => p.FiscalYearId == yearId);
        }

        /// <summary>
        /// Get the default LedgerAccountYear for a LedgerAccount based on a fiscal year
        /// </summary>
        public LedgerAccountYear GetLedgerAccountYear(LedgerAccount account, FiscalYear year)
        {
            return GetLedgerAccountYear(account, year?.Id);
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
                    result.ExplicitBusinessUnit = _ledgerLogic.LedgerCache.Entities.FirstOrDefault(p => p.BusinessUnit.Code == unitCode)?.BusinessUnit;
                    if (result.ExplicitBusinessUnit == null)
                    {
                        throw new ValidationException(UserMessagesGL.BadBusinessUnitCode, "Business unit", unitCode);
                    }

                    if (!allowBusinessUnitOverride && result.ExplicitBusinessUnit.Id != ledger.BusinessUnitId)
                    {
                        throw new ValidationException(UserMessagesGL.AccountMustBeInBusinessUnit, "Business unit", UnitOfWork.GetReference(ledger, p => p.BusinessUnit).Code);
                    }

                    if (fiscalYear != null)
                    {
                        // Find the same fiscal year in the explicit business unit.
                        FiscalYear otherYear = UnitOfWork.GetBusinessLogic<FiscalYearLogic>().GetFiscalYearForBusinessUnit(fiscalYear, result.ExplicitBusinessUnit);
                        if (otherYear == null)
                        {
                            throw new ValidationException(UserMessagesGL.BadFiscalYearForBusinessUnit, fiscalYear.Name, "Business unit", result.ExplicitBusinessUnit.Code);
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
                    throw new ValidationException(UserMessagesGL.GLSegmentLength, segmentInfo[segmentNumber].Length.ToString());
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
                        throw new ValidationException(UserMessages.InvalidCode, segmentInfo[segmentNumber].Name, code);
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
                    sb.Append(segmentInfo[index - 1].Separator);
                }
                sb.Append(result.SegmentCodes[index]);
            }

            accountNumber = sb.ToString();

            if (result.ExplicitBusinessUnit != null)
            {
                sb.Insert(0, result.ExplicitBusinessUnit.Code + BusinessUnitSeparator);
            }

            result.AccountNumber = sb.ToString();

            if (fiscalYear != null)
            {
                // Try to find the account in the fiscal year.
                result.Account = UnitOfWork.FirstOrDefault<Account>(p => p.AccountNumber == accountNumber && p.FiscalYearId == fiscalYear.Id);
                if (result.Account == null)
                {
                    if (validateAccount)
                    {
                        throw new ValidationException(UserMessagesGL.GLAccountNumberInvalid, accountNumber);
                    }
                }
                else
                {
                    result.LedgerAccount = GetLedgerAccount(result.Account);
                }
            }
            else if (ledger != null)
            {
                // No fiscal year, but try to find the LedgerAccount for this account number.
                result.LedgerAccount = UnitOfWork.FirstOrDefault<LedgerAccount>(p => p.AccountNumber == accountNumber);
                if (validateAccount && result.LedgerAccount == null)
                {
                    throw new ValidationException(UserMessagesGL.GLAccountNumberInvalid, accountNumber);
                }
            }

            return result;

        }

        /// <summary>
        /// Get the list of prior year accounts for a G/L account.  Prior year accounts include a factor (0 &lt x &lt= 1) that must be multiplied by the balance or activity figures. 
        /// </summary>
        /// <returns></returns>
        public List<MappedAccount> GetPriorYearAccounts(Account account, FiscalYear priorYear = null)
        {
            var list = new List<MappedAccount>();

            if (priorYear == null)
            {
                priorYear = UnitOfWork.GetBusinessLogic<FiscalYearLogic>().GetPriorFiscalYear(UnitOfWork.GetReference(account, p => p.FiscalYear), true);
                if (priorYear == null)
                {
                    return list;
                }
            }
            // Look in PriorYearAccounts collection.
            foreach (var entry in UnitOfWork.GetEntities<AccountPriorYear>(p => p.PriorAccount).Where(p => p.AccountId == account.Id))
            {
                    
                if (!list.Any(p => p.Account.Id == entry.PriorAccount.Id))
                {
                    list.Add(new MappedAccount() { Account = entry.PriorAccount, Factor = entry.Percentage / 100m });
                }
            }

            LedgerAccount ledgerAccount = GetLedgerAccount(account);

            // Get the account in the prior year.
            foreach (var entry in UnitOfWork.GetEntities<LedgerAccountYear>(p => p.FiscalYear, p => p.Account)
                                            .Where(p => p.LedgerAccountId == ledgerAccount.Id && p.FiscalYearId == priorYear.Id))
            {
                if (entry.Account != null && !list.Any(p => p.Account.Id == entry.Account.Id))
                {
                    list.Add(new MappedAccount() { Account = entry.Account, Factor = 1m });
                }
            }

            return list;
        }

        /// <summary>
        /// Get calculated account activity for an account.
        /// </summary>
        public AccountActivitySummary GetAccountActivity(Account account)
        {
            FiscalYear currentYear;

            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            // Load the budgets
            var budgets = UnitOfWork.GetReference(account, p => p.Budgets);
            AccountBudget workingBudget = budgets.FirstOrDefault(p => p.BudgetType == BudgetType.Working);
            AccountBudget fixedBudget = budgets.FirstOrDefault(p => p.BudgetType == BudgetType.Fixed);
            AccountBudget whatifBudget = budgets.FirstOrDefault(p => p.BudgetType == BudgetType.WhatIf);

            // Create the objects to return.
            var summary = new AccountActivitySummary();
            var detail = new List<AccountActivityDetail>();
            summary.Detail = detail;

            summary.AccountNumber = account.AccountNumber;
            summary.AccountName = account.Name;

            // Ensure the fiscal year is available and that the ledger and fiscal periods are populated.
            if (account.FiscalYear?.Ledger != null && account.FiscalYear?.FiscalPeriods != null)
            {
                currentYear = account.FiscalYear;
            }
            else
            {
                currentYear = UnitOfWork.GetById<FiscalYear>(account.FiscalYearId ?? Guid.Empty, p => p.Ledger, p => p.FiscalPeriods);
            }

            if (currentYear == null)
            {
                throw new InvalidOperationException($"Account {account.Id} has no fiscal year.");
            }

            Ledger ledger = currentYear.Ledger;
            if (ledger == null)
            {
                throw new InvalidOperationException($"Fiscal year {currentYear.Id} has no ledger.");
            }

            // Populate summary properties
            summary.FiscalYearName = currentYear.Name;

            FiscalYear priorYear = UnitOfWork.GetBusinessLogic<FiscalYearLogic>().GetPriorFiscalYear(currentYear);
            summary.PriorYearName = priorYear?.Name ?? string.Empty;
            summary.WorkingBudgetName = ledger.WorkingBudgetName;
            summary.FixedBudgetName = ledger.FixedBudgetName;
            summary.WhatIfBudgetName = ledger.WhatIfBudgetName;
            summary.Id = GuidHelper.NewSequentialGuid();

            // Build detail rows for each fical period
            foreach (var period in currentYear.FiscalPeriods.OrderBy(p => p.PeriodNumber))
            {
                var row = new AccountActivityDetail();
                row.Id = GuidHelper.NewSequentialGuid();
                detail.Add(row);

                row.PeriodNumber = period.PeriodNumber;
                if (period.IsAdjustmentPeriod)
                {
                    row.PeriodName = "Adjustments";
                }
                else if (period.StartDate.HasValue)
                {
                    row.PeriodName = $"{period.PeriodNumber:D2}: {period.StartDate.Value:MMM yy}";  // e.g. Jan 14
                }

                // Load budget amounts (adjustment period has no budget)
                if (!period.IsAdjustmentPeriod)
                {
                    if (workingBudget != null)
                    {
                        row.WorkingBudget = workingBudget.Budget.Amounts[period.PeriodNumber - 1];
                    }
                    if (fixedBudget != null)
                    {
                        row.FixedBudget = fixedBudget.Budget.Amounts[period.PeriodNumber - 1];
                    }
                    if (whatifBudget != null)
                    {
                        row.WhatIfBudget = whatifBudget.Budget.Amounts[period.PeriodNumber - 1];
                    }
                }

                // Beginning balance
                if (row.PeriodNumber == 1)
                {
                    row.BeginningBalance = account.BeginningBalance;
                }

            }

            // Get current balances, then copy them into the detail rows.

            var balances = UnitOfWork.Where<AccountBalance>(p => p.Id == account.Id);

            foreach (var entry in balances)
            {
                var row = detail.FirstOrDefault(p => p.PeriodNumber == entry.PeriodNumber);
                if (row != null)
                {
                    if (entry.DebitCredit.StartsWith("D"))
                    {
                        row.Debits = entry.TotalAmount;
                    }
                    else
                    {
                        row.Credits = entry.TotalAmount;
                    }
                }
            }

            // Get set of prior year accounts, then copy them into the detail rows.

            var mappedAccounts = GetPriorYearAccounts(account, priorYear);
            foreach (var priorAccount in mappedAccounts)
            {
                balances = UnitOfWork.Where<AccountBalance>(p => p.Id == priorAccount.Account.Id);

                var firstPeriod = detail.FirstOrDefault(p => p.PeriodNumber == 1);

                if (firstPeriod != null)
                {
                    firstPeriod.PriorBeginningBalance += priorAccount.Account.BeginningBalance * priorAccount.Factor;
                }

                foreach (var entry in balances)
                {
                    var row = detail.FirstOrDefault(p => p.PeriodNumber == entry.PeriodNumber);
                    if (row != null)
                    {
                        if (entry.DebitCredit.StartsWith("D"))
                        {
                            row.PriorDebits += entry.TotalAmount * priorAccount.Factor;
                        }
                        else
                        {
                            row.PriorCredits += entry.TotalAmount * priorAccount.Factor;
                        }
                    }
                }
            }

            // Generate all other calculated columns
            decimal balance = 0m;
            decimal priorBalance = 0m;
            decimal activityTotal = 0;

            foreach (var row in detail.OrderBy(p => p.PeriodNumber))
            {
                row.BeginningBalance += balance;
                row.Activity = row.Debits - row.Credits;
                row.EndingBalance = row.BeginningBalance + row.Activity;

                row.PriorBeginningBalance += priorBalance;
                row.PriorActivity = row.PriorDebits - row.PriorCredits;
                row.PriorEndingBalance = row.PriorBeginningBalance + row.PriorActivity;

                row.WhatIfBudgetVariance = row.WhatIfBudget - row.Activity;
                row.WorkingBudgetVariance = row.WorkingBudget - row.Activity;
                row.FixedBudgetVariance = row.FixedBudget - row.Activity;

                balance = row.EndingBalance;
                priorBalance = row.PriorEndingBalance;
                activityTotal += row.Activity;
            }

            summary.ActivityTotal = activityTotal;
            summary.FinalEndingBalance = balance;

            return summary;
        }
        		
        #endregion

        #region Private Methods

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

        #endregion

        #region Nested Classes

        /// <summary>
        /// An account with a factor (1 or less) for mapping a prior year account to a current year account.
        /// </summary>
        public class MappedAccount
        {
            public Account Account { get; set; }
            public decimal Factor { get; set; }

        }

        #endregion

    }
}
