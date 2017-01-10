using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.SystemAdministration)]
    public class SystemAdministration : ModuleInfoBase
    {
        public override string Code => "DDI";
        public override string Name => "System Administration";

        public override bool IsRequired => true;
    }
}
