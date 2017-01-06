using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.Portfolio, ModuleType.ExtensionFund)]
    public class Portfolio : ModuleInfoBase
    {
        public override string Code => "EFPF";
        public override string Name => "Portfolio";

        public override bool CanDisburse => true;
        public override string CheckStubInvoiceLabel => "Port. #";
    }
}
