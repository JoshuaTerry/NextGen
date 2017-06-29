using System.ComponentModel;

namespace DDI.Shared.Enums.CP
{
    public enum ReceiptBatchEntryMode
    {
        None = 0,

        [Description("User who entered receipts cannot distribute them")]
        Mailroom = 1,

        [Description("User who entered receipts can distribute them")]
        Cashier = 2
    }
}
