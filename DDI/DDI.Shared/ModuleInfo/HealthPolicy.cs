using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.HealthPolicy)]
    public class HealthPolicy : ModuleInfoBase
    {
        public override string Code => "HP";
        public override string Name => "Health Policy";

        public override bool CanDisburse => true;
        public override string CheckStubInvoiceLabel => "Policy #";
    }
}
