using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.Campaigns, ModuleType.FundRaising)]
    public class Campaigns : ModuleInfoBase
    {
        public override string Code => "FRCM";
        public override string Name => "Campaigns and Appeals";
    }
}
