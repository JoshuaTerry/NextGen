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
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using DDI.Shared.Statics.GL;

namespace DDI.Business.GL
{
    /// <summary>
    /// Business Logic class for posting GL transactions (Transaction to PostedTransaction)
    /// </summary>
    public class PostingLogic : IBusinessLogic
    {
        #region Private Fields

        private Dictionary<Guid, FiscalYear> _fiscalYears;
        private Dictionary<DateTime, BalanceInfo> _balances;
        
        private int _currentLineNumber;
        private decimal _tranBalance;
        private FundLogic _fundLogic;

        #endregion

        #region Constructors 

        public PostingLogic(IUnitOfWork uow)
        {
            UnitOfWork = uow;
            uow.AddBusinessLogic(this);

            _fiscalYears = new Dictionary<Guid, FiscalYear>();
            _balances = new Dictionary<DateTime, BalanceInfo>();
            _currentLineNumber = 0;
            _tranBalance = 0;
        }

        #endregion

        #region Public Properties

        public IUnitOfWork UnitOfWork { get; private set; }

        public List<Guid> BusinessUnitsToPost { get; set; }
        public List<Guid> BusinessUnitsToExclude { get; set; }

        public DateTime? MaxTransactionDate { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Post a set of transactions.
        /// </summary>
        /// <param name="transToPost">Enumerable set of Transaction entities.</param>
        public void PostTransaction(IEnumerable<Transaction> transToPost)
        {
            DateTime postDt = DateTime.Now;

            // Get the list of transaction numbers to be posted.
            IList<Int64> tranNums = transToPost.Select(p => p.TransactionNumber).Distinct().OrderBy(p => p).ToList();            

            using (IUnitOfWork uow = Factory.CreateUnitOfWork())
            {
                try
                {
                    // Post each transction number separately.
                    foreach (var transactionNumber in tranNums)
                    {
                        uow.BeginTransaction();
                        InitializeTransactionBalance(transactionNumber);

                        foreach (var tran in GetTransctionsToPost(uow.Where<Transaction>(p => p.TransactionNumber == transactionNumber).OrderBy(p => p.LineNumber)))
                        {
                            PostTransaction(uow, tran, postDt);
                        }

                        VerifyTransactionIsBalanced(transactionNumber);
                        uow.CommitTransaction();
                    }
                }
                catch
                {
                    uow.RollbackTransaction();
                    throw;
                }
            }
        }

        /// <summary>
        /// Post transactions for a specific transaction number.
        /// </summary>
        /// <param name="transactionNumber"></param>
        public void PostTransaction(Int64 transactionNumber)
        {
            DateTime postDt = DateTime.Now;

            using (IUnitOfWork uow = Factory.CreateUnitOfWork())
            {
                try
                {
                    uow.BeginTransaction();
                    InitializeTransactionBalance(transactionNumber);

                    foreach (var tran in GetTransctionsToPost(uow.Where<Transaction>(p => p.TransactionNumber == transactionNumber).OrderBy(p => p.LineNumber)))
                    {
                        PostTransaction(uow, tran, postDt);
                    }

                    VerifyTransactionIsBalanced(transactionNumber);
                    uow.CommitTransaction();
                }
                catch
                {
                    uow.RollbackTransaction();
                    throw;
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Main logic for posting a Transction entity.
        /// </summary>
        /// <param name="uow">Unit of work having an active DB transaction</param>
        /// <param name="tran">Transaction to post</param>
        /// <param name="postDate">Post date</param>
        private void PostTransaction(IUnitOfWork uow, Transaction tran, DateTime postDate)
        {            
            if (tran.Amount != 0m)
            {
                // Validation
                FiscalYear year = GetFiscalYear(tran.FiscalYearId);

                if (tran.TransactionDate == null)
                {
                    throw new InvalidOperationException(string.Format(UserMessages.TranNoTranDate, tran.DisplayName));
                }

                FiscalPeriod period = GetFiscalPeriod(year, tran.TransactionDate.Value, tran.IsAdjustment);
                if (period == null)
                {
                    throw new InvalidOperationException(string.Format(UserMessagesGL.NoFiscalPeriodForDate, tran.TransactionDate.ToShortDateString(), year.Name));
                }

                if (year.Ledger.PriorPeriodPostingMode == PriorPeriodPostingMode.Prohibit)
                {
                    if (year.Status == FiscalYearStatus.Closed && tran.IsAdjustment == false)
                    {
                        throw new InvalidOperationException(string.Format(UserMessagesGL.FiscalYearClosed, year.Name));
                    }
                    if (period.Status == FiscalPeriodStatus.Closed && tran.IsAdjustment == false)
                    {
                        throw new InvalidOperationException(string.Format(UserMessagesGL.FiscalPeriodClosed, period.PeriodNumber, year.Name));
                    }
                }

                if (tran.DebitAccountId != null && tran.CreditAccountId != null)
                {
                    // For two-sided transactions, ignore the sign of the transaction amount.
                    decimal amt = Math.Abs(tran.Amount);
                    CreatePostedTransaction(uow, tran.DebitAccountId, amt, period, tran);
                    CreatePostedTransaction(uow, tran.CreditAccountId, -amt, period, tran);                    
                }
                else
                {
                    // For one-sided transactions, only the debit account is used, and the transaction amount's sign is significant.
                    CreatePostedTransaction(uow, tran.DebitAccountId, tran.Amount, period, tran);
                }
                if (year.Status == FiscalYearStatus.Empty)
                {
                    year.Status = FiscalYearStatus.Open;
                }
            }

            tran.PostDate = postDate;
            tran.Status = TransactionStatus.Posted;            
        }

        private void CreatePostedTransaction(IUnitOfWork uow, Guid? ledgerAccountYearId, decimal amount, FiscalPeriod period, Transaction tran)
        {
            if (ledgerAccountYearId == null)
            {
                throw new ArgumentNullException(nameof(ledgerAccountYearId));
            }

            PostedTransaction postedTran = new PostedTransaction();
            postedTran.LedgerAccountYearId = ledgerAccountYearId;
            postedTran.FiscalYearId = period.FiscalYearId;
            postedTran.PeriodNumber = period.PeriodNumber;
            postedTran.PostedTransactionType = PostedTransactionType.Actual;
            postedTran.TransactionDate = tran.TransactionDate;
            postedTran.Amount = amount;
            postedTran.TransactionNumber = tran.TransactionNumber;
            postedTran.LineNumber = ++_currentLineNumber;
            postedTran.Description = tran.Description;
            postedTran.TransactionType = tran.TransactionType;
            postedTran.IsAdjustment = tran.IsAdjustment;
            postedTran.TransactionId = tran.Id;
            uow.Insert(postedTran);

            _tranBalance += amount;

            BalanceInfo balanceInfo = _balances.GetValueOrDefault(tran.TransactionDate.Value);
            if (balanceInfo == null)
            {
                balanceInfo = new BalanceInfo();
                _balances[tran.TransactionDate.Value] = balanceInfo;                
            }
            balanceInfo.TranBalance += amount;
            balanceInfo.EntityBalance[period.FiscalYear.Id] = balanceInfo.EntityBalance.GetValueOrDefault(period.FiscalYear.Id) + amount;

            if (period.FiscalYear.Ledger.FundAccounting)
            {
                if (_fundLogic == null)
                {
                    _fundLogic = UnitOfWork.GetBusinessLogic<FundLogic>();
                }

                var lay = UnitOfWork.GetById<LedgerAccountYear>(ledgerAccountYearId.Value, p => p.Account);
                if (lay == null)
                {
                    throw new InvalidOperationException(string.Format(UserMessages.TranCantGetFund, tran.DisplayName));
                }

                Fund fund = _fundLogic.GetFund(lay.Account);
                if (fund == null)
                {
                    throw new InvalidOperationException(string.Format(UserMessages.TranCantGetFund, tran.DisplayName));
                }

                balanceInfo.FundBalance[fund.Id] = balanceInfo.FundBalance.GetValueOrDefault(fund.Id) + amount;
            }
        }

        private IEnumerable<Transaction> GetTransctionsToPost(IEnumerable<Transaction> transactions)
        {
            // Select only transactions with Unposted status
            foreach (Transaction tran in transactions.Where(p => p.Status == TransactionStatus.Unposted))
            {

                // Filtering on Business units to post
                if (BusinessUnitsToPost != null && BusinessUnitsToPost.Count > 0)
                {
                    FiscalYear year = GetFiscalYear(tran.FiscalYearId);
                    if (year?.Ledger?.BusinessUnitId == null || !BusinessUnitsToPost.Contains(year.Ledger.BusinessUnitId.Value))
                    {
                        continue;
                    }
                }

                // Filtering on business units to exclude
                if (BusinessUnitsToExclude != null && BusinessUnitsToExclude.Count > 0)
                {
                    FiscalYear year = GetFiscalYear(tran.FiscalYearId);
                    if (year?.Ledger?.BusinessUnitId != null && BusinessUnitsToExclude.Contains(year.Ledger.BusinessUnitId.Value))
                    {
                        continue;
                    }
                }

                // Filtering on transaction date
                if (MaxTransactionDate.HasValue)
                {
                    if (tran.TransactionDate > MaxTransactionDate)
                    {
                        continue;
                    }

                    // Ensure transaction date is in fiscal year
                    FiscalYear year = GetFiscalYear(tran.FiscalYearId);
                    if (year == null || MaxTransactionDate < year.StartDate || MaxTransactionDate > year.EndDate)
                    {
                        continue;
                    }
                }
                
                yield return tran;
            }            
        }

        private void InitializeTransactionBalance(Int64 transactionNumber)
        {
            _tranBalance = 0m;
            _balances.Clear();
            _currentLineNumber = UnitOfWork.Where<PostedTransaction>(p => p.TransactionNumber == transactionNumber).Max<PostedTransaction, int?>(p => p.LineNumber) ?? 0;
        }

        private FiscalPeriod GetFiscalPeriod(FiscalYear year, DateTime tranDate, bool isAdjustment)
        {
            return year.FiscalPeriods.FirstOrDefault(p => p.StartDate <= tranDate && p.EndDate >= tranDate && p.IsAdjustmentPeriod == isAdjustment);
        }

        private FiscalYear GetFiscalYear(Guid? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            FiscalYear year = _fiscalYears.GetValueOrDefault(id.Value);
            if (year == null)
            {
                year = UnitOfWork.GetById<FiscalYear>(id.Value, p => p.FiscalPeriods, p => p.Ledger);
                if (year != null)
                {
                    _fiscalYears[id.Value] = year;
                }
                else
                {
                    throw new InvalidOperationException(UserMessagesGL.BadFiscalYear);
                }
            }

            return year;
        }

        private void VerifyTransactionIsBalanced(Int64 transactionNumber)
        {
            string tranName = $"Transaction {transactionNumber}";

            if (_tranBalance != 0m)
            {
                throw new InvalidOperationException(string.Format(UserMessages.TranImbalance, tranName, _tranBalance));
            }

            foreach(var entry in _balances)
            {
                DateTime tranDt = entry.Key;
                BalanceInfo balance = entry.Value;

                if (balance.TranBalance != 0m)
                {
                    throw new InvalidOperationException(string.Format(UserMessages.TranImbalanceForDate, tranName, balance.TranBalance, tranDt));
                }

                var unbalanced = balance.EntityBalance.FirstOrDefault(p => p.Value != 0m);
                if (unbalanced.Value != 0m)
                {
                    BusinessUnit unit = UnitOfWork.GetById<BusinessUnit>(unbalanced.Key);
                    throw new InvalidOperationException(string.Format(UserMessages.TranImbalanceForBU, tranName, unbalanced.Value,
                        unit?.Code ?? string.Empty, tranDt));
                }

                unbalanced = balance.FundBalance.FirstOrDefault(p => p.Value != 0m);
                if (unbalanced.Value != 0m)
                {
                    Fund fund = UnitOfWork.GetById<Fund>(unbalanced.Key, p => p.FundSegment);
                    throw new InvalidOperationException(string.Format(UserMessages.TranImbalanceForFund, tranName, unbalanced.Value,
                        fund?.FundSegment?.Code ?? string.Empty, tranDt));
                }
            }
        }

        #endregion

        #region Nested Classes

        /// <summary>
        /// Class for tracking balances for a single transaction number.
        /// </summary>
        private class BalanceInfo
        {
            public decimal TranBalance { get; set; }
            public Dictionary<Guid, decimal> EntityBalance { get; set; }
            public Dictionary<Guid, decimal> FundBalance { get; set; }

            public BalanceInfo()
            {
                TranBalance = 0m;
                EntityBalance = new Dictionary<Guid, decimal>();
                FundBalance = new Dictionary<Guid, decimal>();
            }

        }

        #endregion

    }
}
