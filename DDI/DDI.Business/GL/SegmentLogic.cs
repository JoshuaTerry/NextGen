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

namespace DDI.Business.GL
{
    public class SegmentLogic : EntityLogicBase<Segment>
    {
        #region Fields

        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(SegmentLogic));

        #endregion

        public SegmentLogic(IUnitOfWork uow) : base(uow)
        {
        }

        #region Public Methods

        public override void Validate(Segment entity)
        {

            LedgerLogic _ledgerLogic = UnitOfWork.GetBusinessLogic<LedgerLogic>();

            Ledger ledger = UnitOfWork.GetById<FiscalYear>(entity.FiscalYearId.Value, p => p.Ledger).Ledger;

            if (ledger == null)
            {
                throw new InvalidOperationException("Cannot determine ledger.");
            }

            SegmentLevel[] segmentInfo = _ledgerLogic.GetSegmentLevels(ledger);

            // Validate length
            if (segmentInfo[entity.Level - 1].Length > 0 && entity.Code.Length != segmentInfo[entity.Level - 1].Length)
            {
                throw new ValidationException(UserMessagesGL.GLSegmentLength, segmentInfo[entity.Level - 1].Length.ToString());
            }

            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                throw new ValidationException(UserMessagesGL.GLSegmentNameBlank);
            }

            string code = entity.Code;

            // Validate format
            if (segmentInfo[entity.Level - 1].Format == SegmentFormat.Numeric && Regex.IsMatch(code, "[^0-9]"))
            {
                throw new ValidationException(UserMessagesGL.GLSegmentNumeric);
            }
            else if (segmentInfo[entity.Level - 1].Format == SegmentFormat.Alpha && Regex.IsMatch(code, "[^A-Z]"))
            {
                throw new ValidationException(UserMessagesGL.GLSegmentAlpha);
            }
            else if (Regex.IsMatch(code, "[^A-Z0-9]"))
            {
                throw new ValidationException(UserMessagesGL.GLSegmentAlphaNumeric);
            }


        }

    }

    #endregion public methods

}
