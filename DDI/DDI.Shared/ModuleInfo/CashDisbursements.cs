using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.CashDisbursements, ModuleType.CashProcessing)]
    public class CashDisbursements : ModuleInfoBase
    {
        public override string Code => "CD";
        public override string Name => "Cash Disbursements";

    }
}
