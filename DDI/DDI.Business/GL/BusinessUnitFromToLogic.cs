using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics.GL;

namespace DDI.Business.GL
{
    public class BusinessUnitFromToLogic : EntityLogicBase<BusinessUnitFromTo>
    {

        private LedgerLogic _ledgerLogic = null;

        #region Constructors 

        public BusinessUnitFromToLogic() : this(new UnitOfWorkEF()) { }

        public BusinessUnitFromToLogic(IUnitOfWork uow) : base(uow)
        {
            _ledgerLogic = uow.GetBusinessLogic<LedgerLogic>();
        }

        #endregion

        #region Public Methods

       
        public override void Validate(BusinessUnitFromTo entity)
        {

            FiscalYear year = UnitOfWork.GetReference(entity, p => p.FiscalYear);
            if (year == null)
            {
                throw new ValidationException(UserMessagesGL.UnitFromToNoFiscalYear);
            }

            Ledger ledger = _ledgerLogic.GetCachedLedger(year.LedgerId);

            BusinessUnit unit = ledger?.BusinessUnit;

            if (unit == null || unit.Id != entity.BusinessUnitId)
            {
                throw new ValidationException(UserMessagesGL.UnitFromToWrongUnit);
            }
            
            LedgerAccount account = UnitOfWork.GetReference(entity, p => p.FromLedgerAccount);
            if (account != null && account.LedgerId != ledger.Id)
            {
                throw new ValidationException(UserMessagesGL.UnitFromToDFAccountWrongLedger);
            }

            account = UnitOfWork.GetReference(entity, p => p.ToLedgerAccount);
            if (account != null && account.LedgerId != ledger.Id)
            {
                throw new ValidationException(UserMessagesGL.UnitFromToDTAccountWrongLedger);
            }

        }

        #endregion

        #region Private Methods      

        #endregion
    }
}
