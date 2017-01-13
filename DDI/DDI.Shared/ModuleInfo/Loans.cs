using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.Loans, ModuleType.ExtensionFund)]
    public class Loans : ModuleInfoBase
    {
        public override string Code => "EFLN";
        public override string Name => "Loans";

        public override bool CanDisburse => true;
        public override string CheckStubInvoiceLabel => "Loan #";
    }
}
