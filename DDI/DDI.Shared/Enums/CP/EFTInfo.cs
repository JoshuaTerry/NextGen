using DDI.Shared.Enums.CP;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.CRM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Enums.CP
{
    [Table("ETFInfo")]
    public class EFTInfo : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        

    }
}
