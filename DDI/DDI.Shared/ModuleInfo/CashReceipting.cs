using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.CashReceipting, ModuleType.CashProcessing)]
    public class CashReceipting : ModuleInfoBase
    {
        public override string Code => "CR";
        public override string Name => "Cash Receipting";

        public override bool CanDisburse => true;

        public override string CheckStubInvoiceLabel => "Credit #";
        public override bool HasCustomFields => true;
    }
}
