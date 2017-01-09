using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.CRM)]
    public class CRM : ModuleInfoBase
    {
        public override string Code => "CRM";
        public override string Name => "Connect-CRM";

        public override bool IsRequired => true;
        public override bool HasCustomFields => true;
    }
}
