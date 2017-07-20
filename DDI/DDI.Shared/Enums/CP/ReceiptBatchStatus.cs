using System.ComponentModel;

namespace DDI.Shared.Enums.CP
{
    public enum ReceiptBatchStatus
    {
        [Description("Unprocessed")]
        Unprocessed = 0,

        [Description("Processed")]
        Processed = 1,

        [Description("InUse")]
        InUse = 2
    }
}
