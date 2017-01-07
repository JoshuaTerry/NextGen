using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.PlannedGiving, ModuleType.FundRaising)]
    public class PlannedGiving : ModuleInfoBase
    {
        public override string Code => "FRPG";
        public override string Name => "Planned Giving";
    }
}
