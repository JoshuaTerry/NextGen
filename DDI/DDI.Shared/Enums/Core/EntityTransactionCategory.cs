using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Enums.Core
{
    /// <summary>
    /// For an EntityTransaction, describes how the transaction affects the entity.
    /// </summary>
    public enum EntityTransactionCategory { None = 0, Adjustment = 1, Receipt = 2, Disbursement = 3 }
}
