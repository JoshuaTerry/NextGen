using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.ExtensionFund)]
    public class ExtensionFund : ModuleInfoBase
    {
        public override string Code => "CEF";
        public override string Name => "Connect-CEF";
    }
}
