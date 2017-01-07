using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.LineOfCredit, ModuleType.ExtensionFund)]
    public class LineOfCredit : ModuleInfoBase
    {
        public override string Code => "EFLC";
        public override string Name => "Line of Credit";

        public override bool CanDisburse => true;
        public override string CheckStubInvoiceLabel => "L of C #";
    }
}
