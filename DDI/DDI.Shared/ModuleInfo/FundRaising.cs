using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.FundRaising)]
    public class FundRaising : ModuleInfoBase
    {
        public override string Code => "FR";
        public override string Name => "Fund Raising";
    }
}
