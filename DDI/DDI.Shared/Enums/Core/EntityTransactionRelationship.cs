using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Enums.Core
{
    /// <summary>
    /// For an EntityTransaction, describes the relationship between a transaction and an entity.
    /// </summary>
    public enum EntityTransactionRelationship
    {
        Owner = 0,      // The owner entity, which created the transaction.
        OwnerLine = 1,  // An owner entity line item from which the transaction was created.
        FundingSource = 2,  // A funding source such as a cash receipt that funded the transaction.
        Parent = 3,     // A parent entity affected by the transaction based on the EntityTransactionCategory and TransactionAmountType.
        Settings = 4    // A settings entity that was used to generate the transaction.
    }
}
