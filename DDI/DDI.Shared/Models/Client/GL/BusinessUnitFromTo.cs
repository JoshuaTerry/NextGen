﻿using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    [Table("BusinessUnitFromTo")]
    public class BusinessUnitFromTo : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Index("IX_FiscalYear_BUs", IsUnique = true, Order = 1)]
        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }

        [Index("IX_FiscalYear_BUs", IsUnique = true, Order = 2)]
        public Guid? BusinessUnitId { get; set; }
        [ForeignKey(nameof(BusinessUnitId))]
        public BusinessUnit BusinessUnit { get; set; }

        [Index("IX_FiscalYear_BUs", IsUnique = true, Order = 3)]
        public Guid? OffsettingBusinessUnitId { get; set; }
        [ForeignKey(nameof(OffsettingBusinessUnitId))]
        public BusinessUnit OffsettingBusinessUnit { get; set; }

        public Guid? FromAccountId { get; set; }
        [ForeignKey(nameof(FromAccountId))]
        public LedgerAccount FromAccount { get; set; }

        public Guid? ToAccountId { get; set; }
        [ForeignKey(nameof(ToAccountId))]
        public LedgerAccount ToAccount { get; set; }
    }
}
