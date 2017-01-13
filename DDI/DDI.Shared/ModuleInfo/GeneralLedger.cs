using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.GeneralLedger, ModuleType.Accounting)]
    public class GeneralLedger : ModuleInfoBase
    {
        public override string Code => "GL";
        public override string Name => "General Ledger";

        public override bool IsRequired => true;
        public override bool HasCustomFields => true;
    }
}
