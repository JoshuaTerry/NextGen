using System;
using System.Linq;
using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics.GL;

namespace DDI.Business.GL
{
    public class FundLogic : EntityLogicBase<Fund>
    {

        private LedgerLogic _ledgerLogic = null;

        #region Constructors      

        public FundLogic(IUnitOfWork uow) : base(uow)
        {
            _ledgerLogic = uow.GetBusinessLogic<LedgerLogic>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the Fund entity for a GL account.
        /// </summary>
        public Fund GetFund(Account account)
        {
            if (account == null)
            {
                return null;
            }

            FiscalYear year = UnitOfWork.GetReference(account, p => p.FiscalYear);
            if (year == null)
            {
                return null;
            }

            return GetFund(account, year);
        }

        /// <summary>
        /// Get the Fund entity for a ledger account within a specific fiscal year.
        /// </summary>
        public Fund GetFund(LedgerAccount account, FiscalYear year)
        {
            if (account == null || year == null)
            {
                return null;
            }

            return GetFund(UnitOfWork.GetBusinessLogic<AccountLogic>().GetAccount(account, year), year);
        }

        public BusinessUnitFromTo GetBusinessUnitFromTo(Guid fiscalYearId, Guid offsettingUnitId)
        {
            return UnitOfWork.FirstOrDefault<BusinessUnitFromTo>(p => p.FiscalYearId == fiscalYearId && p.OffsettingBusinessUnitId == offsettingUnitId && p.FromLedgerAccountId != null && p.ToLedgerAccountId != null)
                    ??
             UnitOfWork.FirstOrDefault<BusinessUnitFromTo>(p => p.FiscalYearId == fiscalYearId && p.OffsettingBusinessUnitId == null && p.FromLedgerAccountId != null && p.ToLedgerAccountId != null);
        }

        public FundFromTo GetFundFromTo(Guid fundId, Guid offsettingFundId)
        {
            return UnitOfWork.FirstOrDefault<FundFromTo>(p => p.FundId == fundId && p.OffsettingFundId == offsettingFundId && p.FromLedgerAccountId != null && p.ToLedgerAccountId != null)
                        ??
                   UnitOfWork.FirstOrDefault<FundFromTo>(p => p.FundId == fundId && p.OffsettingFundId == null && p.FromLedgerAccountId != null && p.ToLedgerAccountId != null);
        }

        public override void Validate(Fund fund)
        {

            FiscalYear year = UnitOfWork.GetReference(fund, p => p.FiscalYear);
            if (year == null)
            {
                throw new ValidationException(UserMessagesGL.FundNoFiscalYear);
            }

            Ledger ledger = UnitOfWork.GetReference(year, p => p.Ledger);
            if (ledger.FundAccounting)
            {

                UnitOfWork.LoadReference(fund, p => p.FundSegment);
                if (fund.FundSegment == null)
                {
                    throw new ValidationException(UserMessagesGL.FundNoFundSegment);
                }

                if (fund.FiscalYearId != fund.FundSegment.FiscalYearId)
                {
                    throw new ValidationException(string.Format(UserMessagesGL.FundSegmentWrongFiscalYear, fund.FundSegment.Code));
                }
            }
            else
            {
                // No fund accounting, so the fund is linked to nothing.
                fund.FundSegment = null;
                fund.FundSegmentId = null;
            }
            
            if (fund.ClosingRevenueLedgerAccountId == null)
            {
                if (fund.ClosingExpenseLedgerAccountId == null)
                {
                    fund.ClosingRevenueLedgerAccount = fund.FundBalanceLedgerAccount;
                    fund.ClosingRevenueLedgerAccountId = fund.FundBalanceLedgerAccountId;
                }
                else
                {
                    fund.ClosingRevenueLedgerAccount = fund.ClosingExpenseLedgerAccount;
                    fund.ClosingRevenueLedgerAccountId = fund.ClosingExpenseLedgerAccountId;
                }
            }

            if (fund.ClosingExpenseLedgerAccountId == null)
            {
                if (fund.ClosingExpenseLedgerAccountId == null)
                {
                    fund.ClosingExpenseLedgerAccount = fund.FundBalanceLedgerAccount;
                    fund.ClosingExpenseLedgerAccountId = fund.FundBalanceLedgerAccountId;
                }
                else
                {
                    fund.ClosingExpenseLedgerAccount = fund.ClosingRevenueLedgerAccount;
                    fund.ClosingExpenseLedgerAccountId = fund.ClosingRevenueLedgerAccountId;
                }
            }

            LedgerAccount account = UnitOfWork.GetReference(fund, p => p.FundBalanceLedgerAccount);
            if (account != null)
            {
                if (account.LedgerId != ledger.Id)
                {
                    throw new ValidationException(UserMessagesGL.FundFBAccountWrongLedger);
                }
                if (ledger.FundAccounting)
                {
                    Fund otherFund = GetFund(account, year);
                    if (otherFund == null || otherFund.Id != fund.Id)
                    {
                        throw new ValidationException(string.Format(UserMessagesGL.FundFBAccountWrongFund, fund.FundSegment.Code));
                    }
                }
            }

            account = UnitOfWork.GetReference(fund, p => p.ClosingRevenueLedgerAccount);
            if (account != null)
            {
                if (account.LedgerId != ledger.Id)
                {
                    throw new ValidationException(UserMessagesGL.FundCRAccountWrongLedger);
                }
                if (ledger.FundAccounting)
                {
                    Fund otherFund = GetFund(account, year);
                    if (otherFund == null || otherFund.Id != fund.Id)
                    {
                        throw new ValidationException(string.Format(UserMessagesGL.FundCRAccountWrongFund, fund.FundSegment.Code));
                    }
                }
            }

            account = UnitOfWork.GetReference(fund, p => p.ClosingExpenseLedgerAccount);
            if (account != null)
            {
                if (account.LedgerId != ledger.Id)
                {
                    throw new ValidationException(UserMessagesGL.FundCEAccountWrongLedger);
                }
                if (ledger.FundAccounting)
                {
                    Fund otherFund = GetFund(account, year);
                    if (otherFund == null || otherFund.Id != fund.Id)
                    {
                        throw new ValidationException(string.Format(UserMessagesGL.FundCEAccountWrongFund, fund.FundSegment.Code));
                    }
                }
            }

        }

        #endregion

        #region Private Methods

        private Fund GetFund(Account account, FiscalYear year)
        {
            if (year != null && _ledgerLogic.GetCachedLedger(year.LedgerId)?.FundAccounting == true)
            {
                int fundLevel = _ledgerLogic.GetFundSegmentLevel(year);
                if (fundLevel > 0)
                {
                    Guid? segmentId = UnitOfWork.GetReference(account, p => p.AccountSegments).FirstOrDefault(p => p.Level == fundLevel)?.SegmentId;
                    if (segmentId != null)
                    {
                        return UnitOfWork.FirstOrDefault<Fund>(p => p.FiscalYearId == account.FiscalYearId && p.FundSegmentId == segmentId);
                    }
                }
            }

            return UnitOfWork.FirstOrDefault<Fund>(p => p.FiscalYearId == account.FiscalYearId && p.FundSegmentId == null);
        }

        #endregion
    }
}
