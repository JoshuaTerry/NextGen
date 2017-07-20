using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Enums.Core
{
    public enum TransactionStatus
    {
        [Description("Non Posting")]
        NonPosting = 0,

        [Description("Pending")]
        Pending = 1,

        [Description("Unposted")]
        Unposted = 2,

        [Description("Posted")]
        Posted = 3,

        [Description("Reversed")]
        Reversed = 4,

        [Description("Deleted")]
        Deleted = 5
    }
}
