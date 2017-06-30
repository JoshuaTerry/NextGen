using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Business.GL;
using DDI.Business.Helpers;
using DDI.Shared;
using DDI.Shared.Enums.Core;
using DDI.Shared.Extensions;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;

namespace DDI.Business.Core
{
    public class TransactionLogic : EntityLogicBase<Transaction>
    {

        #region Private Fields

        IRepository<Transaction> _transactionRepo = null;
        AccountLogic _accountLogic = null;
        private const string TRANSACTION_HAS_NO_FISCAL_YEAR = "Transaction has no fiscal year defined.";

        #endregion

        #region Constructors 

        public TransactionLogic(IUnitOfWork uow) : base(uow)
        {
            _transactionRepo = uow.GetRepository<Transaction>();
            _accountLogic = uow.GetBusinessLogic<AccountLogic>();
        }

        #endregion

        #region Public Methods

        public Int64 GetNextTransactionNumber()
        {
            if (_transactionRepo.Utilities == null)
            {
                // If this is a mocked repo, get the last transaction # and add one.
                return 1 + (_transactionRepo.Entities.Max(p => p.TransactionNumber));
            }

            return _transactionRepo.Utilities.GetNextSequenceValue(DatabaseSequence.TransactionNumber);
        }

        public void SetNextTransactionNumber(Int64 newValue)
        {
            _transactionRepo.Utilities?.SetNextSequenceValue(DatabaseSequence.TransactionNumber, newValue);
        }

        /// <summary>
        /// Set the debit G/L account to a specific Account.
        /// </summary>
        public void SetDebitAccount(Transaction transaction, Account acct)
        {
            transaction.DebitAccount = _accountLogic.GetLedgerAccountYear(acct);
            transaction.DebitAccountId = transaction.DebitAccount?.Id;
        }

        /// <summary>
        /// Set the credit G/L account to a specific Account.
        /// </summary>
        public void SetCreditAccount(Transaction transaction, Account acct)
        {
            transaction.CreditAccount = _accountLogic.GetLedgerAccountYear(acct);
            transaction.CreditAccountId = transaction.CreditAccount?.Id;
        }

        /// <summary>
        /// Set the debit G/L account to a LedgerAccount using the transaction's fiscal year.
        /// </summary>
        public void SetDebitAccount(Transaction transaction, LedgerAccount account)
        {
            if (account == null)
            {
                transaction.DebitAccount = null;
                transaction.DebitAccountId = null;
            }
            else
            {
                if (transaction.FiscalYear == null)
                {
                    throw new InvalidOperationException(TRANSACTION_HAS_NO_FISCAL_YEAR);
                }
                transaction.DebitAccount = _accountLogic.GetLedgerAccountYear(account, transaction.FiscalYear);
                transaction.DebitAccountId = transaction.DebitAccount?.Id;
            }
        }

        /// <summary>
        /// Set the debit G/L account to a LedgerAccount using the transaction's fiscal year.
        /// </summary>
        public void SetCreditAccount(Transaction transaction, LedgerAccount account)
        {
            if (account == null)
            {
                transaction.CreditAccount = null;
                transaction.CreditAccountId = null;
            }
            else
            {
                if (transaction.FiscalYear == null)
                {
                    throw new InvalidOperationException(TRANSACTION_HAS_NO_FISCAL_YEAR);
                }
                transaction.CreditAccount = _accountLogic.GetLedgerAccountYear(account, transaction.FiscalYear);
                transaction.CreditAccountId = transaction.CreditAccount?.Id;
            }
        }

        /// <summary>
        /// Exchange the debit and credit G/L account values.
        /// </summary>
        public void SwapGLAccounts(Transaction transaction)
        {
            if (transaction.DebitAccountId != null && transaction.CreditAccountId != null)
            {
                Guid tempId = transaction.DebitAccountId.Value;
                transaction.DebitAccountId = transaction.CreditAccountId;
                transaction.CreditAccountId = tempId;

                LedgerAccountYear tempAccount = transaction.DebitAccount;
                transaction.DebitAccount = transaction.CreditAccount;
                transaction.CreditAccount = tempAccount;            
            }
        }

        // Negate a tranaction's amount and swap the G/L accounts (since amount's sign is not used when posting)
        public void Negate(Transaction transaction)
        {
            transaction.Amount = -transaction.Amount;
            SwapGLAccounts(transaction);
        }

        /// <summary>
        /// Create a duplicate of an existing transaction.  (A new line number will need to be assigned.)
        /// </summary>
        /// <returns></returns>
        public Transaction Duplicate(Transaction tran)
        {
            return new Transaction()
            {
                Amount = tran.Amount,
                CreditAccount = tran.CreditAccount,
                CreditAccountId = tran.CreditAccountId,
                DebitAccount = tran.DebitAccount,
                DebitAccountId = tran.DebitAccountId,
                Description = tran.Description,
                FiscalYear = tran.FiscalYear,
                FiscalYearId = tran.FiscalYearId,
                IsAdjustment = tran.IsAdjustment,
                TransactionNumber = tran.TransactionNumber,
                Status = tran.Status,
                TransactionDate = tran.TransactionDate,
                TransactionType = tran.TransactionType,
            };
        }

        /// <summary>
        /// Validate a set of transactions.  Any errors are returned as a string, otherwise the return value is null. 
        /// Note: {0} in the returned string must be replaced by a description of an entity (e.g. "Journal #1")
        /// </summary>
        /// <returns></returns>
        public string ValidateTransactions(IList<Transaction> transactions)
        {
            try
            {
                // Ensure transaction list is not null.
                if (transactions == null)
                {
                    throw new ArgumentNullException(nameof(transactions));
                }

                FiscalYearLogic yearLogic = UnitOfWork.GetBusinessLogic<FiscalYearLogic>();
                AccountLogic accountLogic = UnitOfWork.GetBusinessLogic<AccountLogic>();
                FundLogic fundLogic = UnitOfWork.GetBusinessLogic<FundLogic>();

                foreach (var transByNumber in transactions.Where(p => p.Status != TransactionStatus.Deleted && p.Status != TransactionStatus.NonPosting)
                                                .GroupBy(p => p.TransactionNumber))
                {
                    decimal tranBalance = 0m;
                    var balances = new Dictionary<DateTime, BalanceInfo>();

                    foreach (var transaction in transByNumber)
                    {
                        if (transaction.TransactionDate == null)
                        {
                            throw new ValidationException(string.Format(UserMessages.TranNoFiscalYear, transaction.ToString()));
                        }

                        FiscalYear year = yearLogic.GetCachedFiscalYear(transaction.FiscalYearId);
                        if (year == null)
                        {
                            throw new ValidationException(string.Format(UserMessages.TranInvalidDate, transaction.ToString()));
                        }

                        FiscalPeriod period = yearLogic.GetFiscalPeriod(year, transaction.TransactionDate.Value);
                        if (period == null)
                        {
                            throw new ValidationException(string.Format(UserMessages.TranInvalidDate, transaction.ToString()));
                        }

                        BalanceInfo balanceInfo = balances.GetValueOrDefault(transaction.TransactionDate.Value);
                        if (balanceInfo == null)
                        {
                            balanceInfo = new BalanceInfo();
                            balances[transaction.TransactionDate.Value] = balanceInfo;
                        }

                        if (transaction.DebitAccountId != null)
                        {
                            tranBalance += transaction.Amount;

                            LedgerAccountYear acct = UnitOfWork.GetReference(transaction, p => p.DebitAccount);
                            balanceInfo.EntityBalance[acct.FiscalYearId.Value] = balanceInfo.EntityBalance.GetValueOrDefault(acct.FiscalYearId.Value) + transaction.Amount;
                            if (year.Ledger.FundAccounting)
                            {
                                Fund fund = fundLogic.GetFund(UnitOfWork.GetReference(transaction.DebitAccount, p => p.Account));
                                if (fund == null)
                                {
                                    throw new InvalidOperationException(string.Format(UserMessages.TranCantGetFund, transaction.DisplayName));
                                }

                                balanceInfo.FundBalance[fund.Id] = balanceInfo.FundBalance.GetValueOrDefault(fund.Id) + transaction.Amount;

                            }
                        }

                        if (transaction.CreditAccountId != null)
                        {
                            tranBalance -= transaction.Amount;

                            LedgerAccountYear acct = UnitOfWork.GetReference(transaction, p => p.CreditAccount);
                            balanceInfo.EntityBalance[acct.FiscalYearId.Value] = balanceInfo.EntityBalance.GetValueOrDefault(acct.FiscalYearId.Value) - transaction.Amount;
                            if (year.Ledger.FundAccounting)
                            {
                                Fund fund = fundLogic.GetFund(UnitOfWork.GetReference(transaction.CreditAccount, p => p.Account));
                                if (fund == null)
                                {
                                    throw new InvalidOperationException(string.Format(UserMessages.TranCantGetFund, transaction.DisplayName));
                                }

                                balanceInfo.FundBalance[fund.Id] = balanceInfo.FundBalance.GetValueOrDefault(fund.Id) - transaction.Amount;
                            }
                        }

                    } // Each transaction

                    string tranName = "{0}";

                    // Validate balances

                    if (tranBalance != 0m)
                    {
                        throw new InvalidOperationException(string.Format(UserMessages.TranImbalance, tranName, tranBalance));
                    }

                    foreach (var entry in balances)
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
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public void SaveTransactions(IList<Transaction> transactions, IEntity entity)
        {
            string entityType = LinkedEntityHelper.GetEntityTypeName(entity.GetType());
            string entityLineType = null;
            IList<EntityTransaction> existingTrans, existingLineTrans;

            // Ensure transaction list and entity are not null.
            if (transactions == null)
            {
                throw new ArgumentNullException(nameof(transactions));
            }

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // See if the transactions are numbered.  If so, all must be numbered.
            bool hasTranNumbers = transactions.Any(p => p.TransactionNumber > 0);
            if (hasTranNumbers && !transactions.All(p => p.TransactionNumber > 0))
            {
                throw new InvalidOperationException("If some transactions have numbers, all transactions must have numbers.");
            }

            // See if transactions have line numbers.  If so, all must have line numbers.
            bool hasLineNumbers = transactions.Any(p => p.LineNumber > 0);
            if (hasLineNumbers && !transactions.All(p => p.LineNumber > 0))
            {
                throw new InvalidOperationException("If some transactions have line numbers, all transactions must have line numbers.");
            }

            // Validate the transactions.
            string message = ValidateTransactions(transactions);
            if (message != null)
            {
                throw new ValidationException(message, LinkedEntityHelper.GetEntityDisplayName(entity));
            }

            using (IUnitOfWork innerUOW = Factory.CreateUnitOfWork())
            {

                try
                {
                    innerUOW.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);

                    // Get list of existing pending entity transactions
                    existingTrans = innerUOW.GetEntities<EntityTransaction>(p => p.Transaction)
                                                  .Where(p => p.EntityType == entityType && p.ParentEntityId == entity.Id && p.Relationship == EntityTransactionRelationship.Owner && (p.Transaction.Status == TransactionStatus.Pending || 
                                                                               p.Transaction.Status == TransactionStatus.Unposted))
                                                  .ToList();

                    if (transactions.Any(p => p.EntityLine != null))
                    {
                        entityLineType = LinkedEntityHelper.GetEntityTypeName(transactions.First(p => p.EntityLine != null).EntityLine.GetType());
                        Guid[] lineIds = transactions.Where(p => p.EntityLine != null).Select(p => p.Id).ToArray();
                        existingLineTrans = innerUOW.GetEntities<EntityTransaction>(p => p.Transaction)
                                      .Where(p => p.EntityType == entityLineType && p.ParentEntityId != null &&
                                                  lineIds.Contains(p.ParentEntityId.Value) &&
                                                  p.Relationship == EntityTransactionRelationship.OwnerLine &&
                                                  (p.Transaction.Status == TransactionStatus.Pending ||
                                                   p.Transaction.Status == TransactionStatus.Unposted))
                                      .ToList();
                    }
                    else
                    {
                        existingLineTrans = null;
                    }

                    // Number the transactions
                    if (!hasTranNumbers)
                    {
                        Int64 transactionNumber;
                        if (existingTrans.Count > 0)
                        {
                            transactionNumber = existingTrans.Max(p => p.Transaction.TransactionNumber);
                        }
                        else
                        {
                            // Need to get a new transaction number.
                            transactionNumber = UnitOfWork.GetRepository<Transaction>().Utilities.GetNextSequenceValue(DatabaseSequence.TransactionNumber);
                        }
                        transactions.ForEach(p => p.TransactionNumber = transactionNumber);
                    }

                    // Assign line numbers
                    if (!hasLineNumbers)
                    {
                        foreach (var group in transactions.GroupBy(p => p.TransactionNumber))
                        {
                            int lineNumber = 0;
                            group.ForEach(p => p.LineNumber = ++lineNumber);
                        }
                    }

                    foreach (var tran in transactions)
                    {
                        bool lineItemLinked = false;

                        EntityTransaction existing = existingTrans.FirstOrDefault();
                        if (existing != null)
                        {
                            // There's an existing "Pending" transction, so just update it.
                            Transaction other = existing.Transaction;
                            other.Amount = tran.Amount;
                            other.CreatedBy = tran.CreatedBy;
                            other.CreatedOn = tran.CreatedOn;
                            other.CreditAccount = tran.CreditAccount;
                            other.CreditAccountId = tran.CreditAccountId;
                            other.DebitAccount = tran.DebitAccount;
                            other.DebitAccountId = tran.DebitAccountId;
                            other.Description = tran.Description;
                            other.FiscalYear = tran.FiscalYear;
                            other.FiscalYearId = tran.FiscalYearId;
                            other.IsAdjustment = tran.IsAdjustment;
                            other.LineNumber = tran.LineNumber;
                            other.PostDate = tran.PostDate;
                            other.Status = tran.Status;
                            other.TransactionDate = tran.TransactionDate;
                            other.TransactionNumber = tran.TransactionNumber;
                            other.TransactionType = tran.TransactionType;
                            tran.Id = other.Id;

                            // The EntityTransaction is valid and doesn't need to be updated.
                            existingTrans.Remove(existing);

                            if (existingLineTrans != null && tran.EntityLine != null)
                            {
                                // If there's an entity line, see if there's an EntityTransaction that points to this transaction.  If so, point it to the line item.
                                EntityTransaction existingLineTran = existingLineTrans.FirstOrDefault(p => p.TransactionId == other.Id);
                                if (existingLineTran != null)
                                {
                                    existingLineTran.ParentEntityId = tran.EntityLine.Id;
                                    lineItemLinked = true;
                                    existingLineTrans.Remove(existingLineTran);
                                }
                            }
                        }
                        else
                        {
                            // No existing "Pending" transaction, so we need to create a new one.
                            tran.AssignPrimaryKey();
                            innerUOW.Insert(tran);

                            // Create an EntityTransaction
                            EntityTransaction entityTran = new EntityTransaction()
                            {
                                EntityType = entityType,
                                ParentEntityId = entity.Id,
                                Relationship = EntityTransactionRelationship.Owner,
                                Transaction = tran,
                                TransactionId = tran.Id
                            };
                            innerUOW.Insert(entityTran);
                        }

                        // If there's a line item, it may need to be linked to the transaction unless an existing one was already linked above.
                        if (tran.EntityLine != null && !lineItemLinked)
                        {
                            EntityTransaction entityTran = new EntityTransaction()
                            {
                                EntityType = entityLineType,
                                ParentEntityId = tran.EntityLine.Id,
                                Relationship = EntityTransactionRelationship.OwnerLine,
                                Transaction = tran,
                                TransactionId = tran.Id
                            };
                            innerUOW.Insert(entityTran);
                        }

                    } // each tran

                    // First, any remaining pending EntityTransactions for the line items must be deleted.
                    if (existingLineTrans != null)
                    {
                        foreach (var entry in existingLineTrans)
                        {
                            innerUOW.Delete(entry);
                        }
                    }
                    // Next, any remaining pending EntityTransactions for the entity itself must be deleted, along with the pending transaction.
                    foreach (var entry in existingTrans)
                    {
                        if (entry.Transaction != null)
                        {
                            innerUOW.Delete(entry.Transaction);
                        }
                        innerUOW.Delete(entry);
                    }

                    // Finally, commit the transaction.
                    innerUOW.CommitTransaction();
                }
                catch
                {
                    innerUOW.RollbackTransaction();
                    throw;
                }
            } // Using inner unit of work.

        }

        #endregion

        #region Private Methods



        #endregion

        #region Nested Classes

        /// <summary>
        /// Class for validating balances for a transaction.
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
