using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.Gifts, ModuleType.FundRaising)]
    public class Donations : ModuleInfoBase
    {
        public override string Code => "FRDG";
        public override string Name => "Donations";
                
        public override bool HasCustomFields => true;
    }
}
