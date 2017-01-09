using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.CashProcessing)]
    public class CashProcessing : ModuleInfoBase
    {
        public override string Code => "CP";
        public override string Name => "Cash Processing";
    }
}
