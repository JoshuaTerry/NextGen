using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.Investments, ModuleType.ExtensionFund)]
    public class Investments : ModuleInfoBase
    {
        public override string Code => "EFIN";
        public override string Name => "Investments";

        public override bool CanDisburse => true;
        public override string CheckStubInvoiceLabel => "Invest #";
    }
}
