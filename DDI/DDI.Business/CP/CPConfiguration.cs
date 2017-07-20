using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Business.Core;
using DDI.Shared;
using DDI.Shared.Enums;
using DDI.Shared.Enums.CP;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Business.CP
{
    public class CPConfiguration : ConfigurationBase
    {
        #region Properties

        public override ModuleType ModuleType { get; } = ModuleType.CashProcessing;

        public string CheckWrittenAmountFormat { get; set; }
        public bool EnableTwoStageReceipting { get; set; }
        public bool EnableSlipPrinting { get; set; }
        public ReceiptBatchEntryMode DefaultBatchEntryMode { get; set; }
        public bool EnableMailroomBatches { get; set; }
        public bool EnableCashierBatches { get; set; }

        #endregion

        #region Constructors

        public CPConfiguration()
        {
        }

        #endregion

        #region Method Overrides

        #endregion

    }

}
