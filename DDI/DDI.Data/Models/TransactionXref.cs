using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Data.Models
{
    /// <summary>
    /// This model is for an EF table that is used to link PostedTransaction to Transaction during data conversion.
    /// It will only be used by SSIS.
    /// </summary>
    [Table("TransactionXref")]
    internal class TransactionXref
    {
        #region Public Properties        

        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid PostedTransactionId { get; set; }

        [Key, Column(Order = 2), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid TransactionId { get; set; }

        #endregion

    }
}
