using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.CropEvents, ModuleType.FundRaising)]
    public class CropEvents : ModuleInfoBase
    {
        public override string Code => "FREV";
        public override string Name => "CROP Events and Programs";

        public override bool CanDisburse => true;
        public override string CheckStubInvoiceLabel => "Event #";
    }
}
