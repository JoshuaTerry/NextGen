using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using DDI.Business.GL;
using DDI.Data;
using DDI.Data.Statics;
using DDI.Shared;
using DDI.Shared.Caching;
using DDI.Shared.Enums;
using DDI.Shared.Enums.Core;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.GL;

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

        public TransactionLogic() : this(new UnitOfWorkEF()) { }

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

            return _transactionRepo.Utilities.GetNextSequenceValue(Sequences.TransactionNumber);
        }

        public void SetNextTransactionNumber(Int64 newValue)
        {
            _transactionRepo.Utilities?.SetNextSequenceValue(Sequences.TransactionNumber, newValue);
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
                if (transaction.FiscalYearId == null)
                {
                    throw new InvalidOperationException(TRANSACTION_HAS_NO_FISCAL_YEAR);
                }
                transaction.DebitAccount = _accountLogic.GetLedgerAccountYear(account, transaction.FiscalYearId);
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
                if (transaction.FiscalYearId == null)
                {
                    throw new InvalidOperationException(TRANSACTION_HAS_NO_FISCAL_YEAR);
                }
                transaction.CreditAccount = _accountLogic.GetLedgerAccountYear(account, transaction.FiscalYearId);
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

        #endregion

        #region Private Methods



        #endregion

    }
}
