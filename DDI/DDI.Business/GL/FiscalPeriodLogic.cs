using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Business.Helpers;
using DDI.Logger;
using DDI.Shared;
using DDI.Shared.Caching;
using DDI.Shared.Enums.GL;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using DDI.Shared.Statics.GL;


namespace DDI.Business.GL
{

    public class FiscalPeriodLogic : EntityLogicBase<FiscalPeriod>
    {
        #region Fields

        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(FiscalPeriodLogic));

        private LedgerLogic _ledgerLogic;        

        #endregion

        #region Constructors

        public FiscalPeriodLogic(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _ledgerLogic = unitOfWork.GetBusinessLogic<LedgerLogic>();
        }

        #endregion

        public override void Validate(FiscalPeriod period)
        {
            FiscalYear year = UnitOfWork.GetById<FiscalYear>(period.FiscalYearId, p => p.FiscalPeriods);
            if (period.FiscalYear == null)
            {
                period.FiscalYear = year;
            }

            // Call the fiscal year validation logic, which will validate all the fiscal periods.
            UnitOfWork.GetBusinessLogic<FiscalYearLogic>().Validate(year);

        }

    }

}