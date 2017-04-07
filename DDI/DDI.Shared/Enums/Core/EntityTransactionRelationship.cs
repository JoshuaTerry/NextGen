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
        Parent = 2,     // A parent entity affected by the transaction based on the EntityTransactionCategory and TransactionAmountType.
        Reversal = 3,   // An owner entity is being reversed by this transaction.
        Settings = 4    // A settings entity that was used to generate the transaction.
    }
}
