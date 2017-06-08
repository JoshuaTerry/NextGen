using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics.GL;

namespace DDI.Business.GL
{
    public class FundFromToLogic : EntityLogicBase<FundFromTo>
    { 
        private LedgerLogic _ledgerLogic = null;
        private FundLogic _fundLogic = null;

        #region Constructors        

        public FundFromToLogic(IUnitOfWork uow) : base(uow)
        {
            _ledgerLogic = uow.GetBusinessLogic<LedgerLogic>();
            _fundLogic = uow.GetBusinessLogic<FundLogic>();
        }

        #endregion

        #region Public Methods

       
        public override void Validate(FundFromTo entity)
        {

            FiscalYear year = UnitOfWork.GetReference(entity, p => p.FiscalYear);
            if (year == null)
            {
                throw new ValidationException(UserMessagesGL.FundFromToNoFiscalYear);
            }

            Ledger ledger = _ledgerLogic.GetCachedLedger(year.LedgerId);
            if (!ledger.FundAccounting)
            {
                throw new ValidationException(UserMessagesGL.FundFromToNoFundAccounting);
            }

            Fund fund = UnitOfWork.GetReference(entity, p => p.Fund);

            if (fund == null || fund.FiscalYearId != entity.FiscalYearId)
            {
                throw new ValidationException(UserMessagesGL.FundFromToWrongFiscalYear);
            }
            
            LedgerAccount account = UnitOfWork.GetReference(entity, p => p.FromLedgerAccount);
            if (account != null) 
            {
                Fund otherFund = _fundLogic.GetFund(account, year);
                if (otherFund == null || otherFund.Id != entity.FundId)
                {
                    throw new ValidationException(UserMessagesGL.FundFromToDFAccountWrongFund);
                }
            }

            account = UnitOfWork.GetReference(entity, p => p.ToLedgerAccount);
            if (account != null)
            {
                Fund otherFund = _fundLogic.GetFund(account, year);
                if (otherFund == null || otherFund.Id != entity.FundId)
                {
                    throw new ValidationException(UserMessagesGL.FundFromToDTAccountWrongFund);
                }
            }

        }

        #endregion

        #region Private Methods      

        #endregion
    }
}
