using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Enums;
using DDI.Shared.ModuleInfo.Base;

namespace DDI.Shared.ModuleInfo
{
    [ModuleTypeAttribute(ModuleType.AccountsPayable, ModuleType.Accounting)]
    public class AccountsPayable : ModuleInfoBase
    {
        public override string Code => "AP";
        public override string Name => "Accounts Payable";

        public override bool HasCustomFields => true;

        public override bool CanDisburse => true;
        public override string CheckStubInvoiceLabel => "Invoice #";
    }
}
