using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.NamedFunds, ModuleType.FundRaising)]
    public class NamedFunds : ModuleInfoBase
    {
        public override string Code => "FRNF";
        public override string Name => "Named Funds";
    }
}
