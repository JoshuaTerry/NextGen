using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.AccountsReceivable, ModuleType.Accounting)]
    public class AccountsReceivable : ModuleInfoBase
    {
        public override string Code => "AR";
        public override string Name => "Accounts Receivable";

        public override bool HasCustomFields => true;

        public override bool CanDisburse => true;
        public override string CheckStubInvoiceLabel => "Refund #";
    }
}
