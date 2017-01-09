using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.AccountsPayable, ModuleType.Accounting)]
    public class Inventory : ModuleInfoBase
    {
        public override string Code => "IV";
        public override string Name => "Inventory";
    }
}
