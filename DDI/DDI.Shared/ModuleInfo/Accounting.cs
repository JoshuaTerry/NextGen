using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.Accounting)]
    public class Accounting : ModuleInfoBase
    {
        public override string Code => "ACCT";
        public override string Name => "Connect-Accounting";

        public override bool IsRequired => true;
    }
}
