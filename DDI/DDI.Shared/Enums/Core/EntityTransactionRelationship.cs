namespace DDI.Shared.Enums.Core
{
    /// <summary>
    /// For an EntityTransaction, describes the relationship between a transaction and an entity.
    /// </summary>
    public enum EntityTransactionRelationship
    {
        Owner = 0,      // The owner entity, which created the transaction.
        OwnerLine = 1,  // An owner entity line item from which the transaction was created.
        Receipt = 2,    // An entity is being receipted by this transaction.
        Disbursement = 3,  // An entity is being disbursed by this transaction.
        Adjustment = 4, // An entity is being adjusted by this transction.
        Reversal = 5,   // An owner entity is being reversed by this transaction.
        Settings = 6    // A settings entity that was used to generate the transaction.
    }
}
