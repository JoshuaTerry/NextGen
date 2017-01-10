using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.FixedAssets, ModuleType.Accounting)]
    public class FixedAssets : ModuleInfoBase
    {
        public override string Code => "FA";
        public override string Name => "Fixed Assets";
    }
}
