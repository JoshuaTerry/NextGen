using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.Pools, ModuleType.ExtensionFund)]
    public class Pools : ModuleInfoBase
    {
        public override string Code => "EFPO";
        public override string Name => "Pools";
    }
}
