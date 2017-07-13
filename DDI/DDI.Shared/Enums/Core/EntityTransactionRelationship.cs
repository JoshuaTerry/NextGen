using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Description("Owner")]
        Owner = 0,      // The owner entity, which created the transaction.
        [Description("Owner Line")]
        OwnerLine = 1,  // An owner entity line item from which the transaction was created.
		[Description("Receipt")]
        Receipt = 2,    // An entity is being receipted by this transaction.
		[Description("Disbursement")]
        Disbursement = 3,  // An entity is being disbursed by this transaction.
		[Description("Adjustment")]
        Adjustment = 4, // An entity is being adjusted by this transction.
		[Description("Reversal")]
        Reversal = 5,   // An owner entity is being reversed by this transaction.
		[Description("Settings")]
        Settings = 6    // A settings entity that was used to generate the transaction.
    }
}
