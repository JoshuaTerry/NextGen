using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Enums.Core
{
    /// <summary>
    /// For an EntityTransaction, describes how the transaction affects the entity.
    /// </summary>
    public enum EntityTransactionCategory
    {
        [Description("None")]
        None = 0,

        [Description("Adjustment")]
        Adjustment = 1,

        [Description("Receipt")]
        Receipt = 2,

        [Description("Disbursement")]
        Disbursement = 3
    }
}
