using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Enums.Core
{
    public enum EntityNumberType
    {
        [Description("Constituent")]
        Constituent = 0,

        [Description("Journal")]
        Journal = 1,

        [Description("Recurring Journal")]
        RecurringJournal = 2,

        [Description("Journal Template")]
        JournalTemplate = 3,
        ReceiptBatch = 4,
        Receipt = 5,
        MiscReceipt = 6,
        DisbursementCheck = 7,
        DisbursementEFT = 8
        
    }
}
